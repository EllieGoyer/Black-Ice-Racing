using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset = new Vector3(0f, 2.25f, -5f);
    public float followSpeed = 10;
    public float lookOffset = 5;
    public float lookSpeed = 10;

    public void LookAtTarget()
    {
        Vector3 lookDirection = followTarget.position + followTarget.forward * lookOffset - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 targetPos = followTarget.position +
                            followTarget.forward * offset.z +
                            followTarget.right * offset.x +
                            followTarget.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }
}
