using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(-Vector3.right, 720 * Time.deltaTime);
    }
}
