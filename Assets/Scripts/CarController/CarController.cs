using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private Rigidbody rb;
    private float antiRollValue = 500;
    private float handlingMod = 400;
    private float maxBrakeTorque = 800;
    [HideInInspector] public float boostCharge = 0;
    
    private float horizontalInput;
    [HideInInspector] public float throttleInput;
    [HideInInspector] public bool boostInput;
    private float steeringAngle;

    private bool AIEnabled = false;

    public WheelCollider frontRightW, frontLeftW, backRightW, backLeftW;
    public Transform frontRightT, frontLeftT, backRightT, backLeftT;

    [Header("Car Parts")]
    public CarBody body;
    public CarEngine engine;
    public CarTire tire;
    public CarAero aero;

    [Header("Downward Force Modifier")]
    public float downwardForce = 100f;
    [Header ("Handling")]
    [Range(30, 60)]
    public float maxSteerAngle = 30;
    [Header ("Acceleration")]
    public float maxMotorTorque = 300;
    [Header("Brake Speed")]
    [Range (1, 5)]
    public float brakeMod = 1;
    [Header ("Max Speed")]
    public float maxVelocity = 25;
    [Header("Max Reverse Speed")]
    public float maxReverseSpeed = 5;
    [Header("Boost Speed")]
    public float boostSpeed = 10;
    [Header("Boost Charge Rate")]
    public float boostChargeRate = 5;
    [Header("Boost Drain Rate")]
    public float boostDrainRate = 5;

    [Space]
    public bool rearWheelDrive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CarAI ai = GetComponent<CarAI>();
        if (ai != null)
        {
            AIEnabled = true;
        }
        Setup();
    }

    public void Setup()
    {
        rb.mass = body.mass + engine.mass + tire.mass + aero.mass;
        maxVelocity = body.topSpeedBase + engine.topSpeedModifier + aero.topSpeedModifier;
        maxMotorTorque = body.accelerationBase + engine.accelerationModifier + tire.accelerationModifier;
        maxSteerAngle = body.handlingBase + engine.handlingModifer + tire.handlingModifer + aero.handlingModifier;
        brakeMod = body.brakingBase + engine.brakingModifer + tire.brakingModifer + aero.brakingModifier;
        AssignTire(false, frontRightT, tire.model);
        AssignTire(true, frontLeftT, tire.model);
        AssignTire(false, backRightT, tire.model);
        AssignTire(true, backLeftT, tire.model);
        GameObject newBody = Instantiate(body.model, transform.GetChild(0));
        newBody.transform.Rotate(Vector3.up, 90);
        GameObject newEngine = Instantiate(engine.model, newBody.transform);
    }

    private void AssignTire(bool isLeft, Transform parent, GameObject tire)
    {
        if (isLeft)
        {
            AssignTire(parent, tire, 90);
        }
        else
        {
            AssignTire(parent, tire, -90);
        }
    }

    private void AssignTire(Transform parent, GameObject tire, float angle)
    {
        GameObject newTire = Instantiate(tire, parent.position, parent.rotation, parent);
        newTire.transform.Rotate(Vector3.up, angle);
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        throttleInput = Input.GetAxis("Throttle");
        boostInput = Input.GetButton("Boost");
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        frontRightW.steerAngle = steeringAngle;
        frontLeftW.steerAngle = steeringAngle;
    }

    private void SteeringHelper()
    {
        if (rb.velocity.magnitude >= maxVelocity * 0.3f && steeringAngle > 5)
        {
            Debug.DrawRay(frontRightW.transform.position, transform.right);
            rb.AddForceAtPosition(transform.right * handlingMod, frontRightW.transform.position);
        }
        if (rb.velocity.magnitude >= maxVelocity * 0.3f && steeringAngle < -5)
        {
            Debug.DrawRay(frontLeftW.transform.position, -transform.right);
            rb.AddForceAtPosition(-transform.right * handlingMod, frontLeftW.transform.position);
        }
    }

    private void Move(bool rearWheelDrive)
    {
        if (rearWheelDrive)
        {
            Move(backLeftW, backRightW);
        }
        else
        {
            Move(frontLeftW, frontRightW);
        }
    }

    private void Move(WheelCollider leftW, WheelCollider rightW)
    {
        if (throttleInput > 0)
        {
            Accelerate(leftW, rightW);
        }
        else if (throttleInput < 0)
        {
            Brake(leftW, rightW);  
        }
        else
        {
            leftW.motorTorque = 0;
            rightW.motorTorque = 0;
            leftW.brakeTorque = 0;
            rightW.brakeTorque = 0;
        }
    }

    public void Accelerate(WheelCollider leftW, WheelCollider rightW)
    {
        leftW.brakeTorque = 0;
        rightW.brakeTorque = 0;
        if (rb.velocity.magnitude < maxVelocity)
        {
            leftW.motorTorque = throttleInput * maxMotorTorque;
            rightW.motorTorque = throttleInput * maxMotorTorque;
        }
        else
        {
            leftW.motorTorque = 0;
            rightW.motorTorque = 0;
        }
    }

    public void Brake(WheelCollider leftW, WheelCollider rightW)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        if (localVelocity.z > 0)
        {
            leftW.motorTorque = 0;
            rightW.motorTorque = 0;
            leftW.brakeTorque = throttleInput * -maxBrakeTorque;
            rightW.brakeTorque = throttleInput * -maxBrakeTorque;

            if (rb.velocity.magnitude > maxVelocity * 0.05f)
            {
                rb.AddForce(-transform.forward * brakeMod * 1000);
                boostCharge += boostChargeRate * Time.deltaTime;
            }
        }
        else
        {
            leftW.brakeTorque = 0;
            rightW.brakeTorque = 0;
            if (rb.velocity.magnitude < maxReverseSpeed)
            {
                leftW.motorTorque = -maxMotorTorque;
                rightW.motorTorque = -maxMotorTorque;
            }
            else
            {
                leftW.motorTorque = 0;
                rightW.motorTorque = 0;
            }
        }
    }

    public void Boost()
    {
        if (boostInput && boostCharge > 0)
        {
            rb.AddForce(transform.forward * boostSpeed * 100);
            boostCharge = Mathf.Max(boostCharge - boostDrainRate * Time.deltaTime, 0);
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion quat;

        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }

    private void StabilizeWheels()
    {
        StabilizeWheels(frontLeftW, frontRightW);
        StabilizeWheels(backLeftW, backRightW);
    }

    private void StabilizeWheels(WheelCollider leftW, WheelCollider rightW)
    {
        float antiRollForce;

        float rightSuspension = CalculateWheelSuspension(rightW);
        float leftSuspension = CalculateWheelSuspension(leftW);

        antiRollForce = (leftSuspension - rightSuspension) * antiRollValue;

        if (leftW.isGrounded)
        {
            rb.AddForceAtPosition(leftW.transform.up * -antiRollForce, leftW.transform.position);
        }
        if (rightW.isGrounded)
        {
            rb.AddForceAtPosition(rightW.transform.up * antiRollForce, rightW.transform.position);
        }
    }

    private float CalculateWheelSuspension(WheelCollider collider)
    {
        WheelHit hit;
        float travelDist;
        bool grounded = collider.GetGroundHit(out hit);
        if (grounded)
        {
            travelDist = (-collider.transform.InverseTransformPoint(hit.point).y - collider.radius) / collider.suspensionDistance;
        }
        else
        {
            travelDist = 1f;
        }

        return travelDist;
    }

    private void ApplyDownwardForce()
    {
        rb.AddForce(-transform.up * downwardForce);
    }

    private void Update()
    {
        ApplyDownwardForce();
    }

    private void FixedUpdate()
    {
        if (!AIEnabled)
        {
            GetInput();
            Steer();
            Move(rearWheelDrive);
            Boost();
        }
        SteeringHelper();
        StabilizeWheels();
        UpdateWheelPoses();
    }
}
