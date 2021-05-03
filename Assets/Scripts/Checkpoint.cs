using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    protected CheckpointManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<CheckpointManager>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CarAI ai = other.GetComponent<CarAI>();
        CarController car = other.GetComponent<CarController>();
        if (car != null)
        {
            if (ai != null)
            {
                manager.AIWinning();
            }
            else
            {
                manager.PlayerWinning();
            }
            Destroy(gameObject);
        }
    }
}
