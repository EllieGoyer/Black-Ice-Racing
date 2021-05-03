using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheckpoint : Checkpoint
{
    bool AIWins = false;

    protected override void OnTriggerEnter(Collider other)
    {
        CarAI ai = other.GetComponent<CarAI>();
        CarController car = other.GetComponent<CarController>();
        if (car != null)
        {
            if (ai != null)
            {
                AIWins = true;
            }
            else
            {
                if (AIWins)
                {
                    manager.AIWins();
                }
                else
                {
                    manager.PlayerWins();
                }
                Destroy(gameObject);
            }
        }
    }
}
