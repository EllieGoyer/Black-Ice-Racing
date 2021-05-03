using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerCar : MonoBehaviour
{
    private CarController controller;
    public Transform parent;
    public float rotateSpeed;

    private void Awake()
    {
        controller = GetComponent<CarController>();
        controller.backLeftW.brakeTorque = 9999;
        controller.backRightW.brakeTorque = 9999;
    }

    private void Update()
    {
        parent.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
