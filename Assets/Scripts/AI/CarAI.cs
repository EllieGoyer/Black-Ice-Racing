using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public AIPath path;
    public CarController controller;
    public bool isBraking = false;
    public bool isBoosting = false;

    private bool finishedBraking = false;

    private List<Transform> nodes;
    private int currentNode = 0;

    private Rigidbody rb;

    private void Start()
    {
        nodes = path.nodes;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        ManageBraking();
        Braking();
        ManageBoosting();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * controller.maxSteerAngle;
        controller.frontRightW.steerAngle = newSteer;
        controller.frontLeftW.steerAngle = newSteer;
    }

    private void Drive()
    {
        if (!isBraking)
        {
            controller.verticalInput = 1;
            controller.Accelerate(controller.backLeftW, controller.backRightW);
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
            finishedBraking = false;
        }
    }

    private void ManageBraking()
    {
        float angle1;
        float angle2;
        if (currentNode == 0)
        {
            angle1 = GetNodeAngle(nodes.Count - 1);
        }
        else
        {
            angle1 = GetNodeAngle(currentNode - 1);
        }

        angle2 = GetNodeAngle(currentNode);

        if (((angle1 >= 45 && rb.velocity.magnitude >= controller.maxVelocity / 2) || (angle2 >= 45 && rb.velocity.magnitude > controller.maxVelocity)) 
            && !finishedBraking)
        {
            isBraking = true;
        }
        else
        {
            finishedBraking = true;
            isBraking = false;
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            controller.verticalInput = -1;
            controller.Brake(controller.backLeftW, controller.backRightW);
        }
        else
        {
            controller.backLeftW.brakeTorque = 0;
            controller.backRightW.brakeTorque = 0;
        }
    }

    private void ManageBoosting()
    {
        float angle1 = GetNodeAngle(currentNode);
        float angle2 = Vector3.Angle(transform.forward, nodes[currentNode].position - transform.position);

        int previousNode;
        if (currentNode == 0)
        {
            previousNode = nodes.Count - 1;
        }
        else
        {
            previousNode = currentNode - 1;
        }

        if (angle1 < 20 && angle2 < 10
            && Vector3.Distance(transform.position, nodes[currentNode].position) > Vector3.Distance(nodes[previousNode].position, nodes[currentNode].position) / 2
            && controller.boostCharge > 0)
        {
            isBoosting = true;
            controller.boostInput = true;
            controller.Boost();
            Debug.Log("Boosting");
        }
        else
        {
            isBoosting = false;
            controller.boostInput = false;
        }
    }

    private float GetNodeAngle(int node)
    {
        Vector3 a;
        Vector3 b;
        if (node == 0)
        {
            a = nodes[node].position - nodes[nodes.Count - 1].position;
            b = nodes[node + 1].position - nodes[node].position;
        }
        else if (node == nodes.Count - 1)
        {
            a = nodes[node].position - nodes[node - 1].position;
            b = nodes[0].position - nodes[node].position;
        }
        else
        {
            a = nodes[node].position - nodes[node - 1].position;
            b = nodes[node + 1].position - nodes[node].position;
        }

        return Vector3.Angle(a, b);
    }
}
