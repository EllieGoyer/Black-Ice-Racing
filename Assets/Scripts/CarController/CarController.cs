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
    private float verticalInput;
    private bool boostInput;
    private float steeringAngle;

    public WheelCollider frontRightW, frontLeftW, backRightW, backLeftW;
    public Transform frontRightT, frontLeftT, backRightT, backLeftT;
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
    [Header("Boost Speed")]
    public float boostSpeed = 10;
    [Header("Boost Charge Rate")]
    public float boostChargeRate = 5;
    [Header("Boost Drain Rate")]
    public float boostDrainRate = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        boostInput = Input.GetKey(KeyCode.Space);
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

    private void Accelerate()
    {
        if (verticalInput > 0)
        {
            frontLeftW.brakeTorque = 0;
            frontRightW.brakeTorque = 0;
            if (rb.velocity.magnitude < maxVelocity)
            {
                frontLeftW.motorTorque = verticalInput * maxMotorTorque;
                frontRightW.motorTorque = verticalInput * maxMotorTorque;
            }
            else
            {
                frontLeftW.motorTorque = 0;
                frontRightW.motorTorque = 0;
            }
        }
        else if (verticalInput < 0)
        {
            frontLeftW.motorTorque = 0;
            frontRightW.motorTorque = 0;
            frontLeftW.brakeTorque = verticalInput * -maxBrakeTorque;
            frontRightW.brakeTorque = verticalInput * -maxBrakeTorque;
            
            if (rb.velocity.magnitude > maxVelocity * 0.05f)
            {
                rb.AddForce(-transform.forward * brakeMod * 1000);
                boostCharge += boostChargeRate * Time.deltaTime;
            }
        }
        else
        {
            frontLeftW.motorTorque = 0;
            frontRightW.motorTorque = 0;
            frontLeftW.brakeTorque = 0;
            frontRightW.brakeTorque = 0;
        }
    }

    private void Boost()
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
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

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

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Boost();
        SteeringHelper();
        StabilizeWheels();
        UpdateWheelPoses();
    }
}
