using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostDisplay : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] boostSprites;
    public CarController controller;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (controller.boostCharge <= 0)
        {
            sr.sprite = null;
        }
        else if (controller.boostCharge > controller.maxBoost * (7.0 / 8))
        {
            sr.sprite = boostSprites[7];
        }
        else if (controller.boostCharge > controller.maxBoost * (6.0 / 8))
        {
            sr.sprite = boostSprites[6];
        }
        else if (controller.boostCharge > controller.maxBoost * (5.0 / 8))
        {
            sr.sprite = boostSprites[5];
        }
        else if (controller.boostCharge > controller.maxBoost * (4.0 / 8))
        {
            sr.sprite = boostSprites[4];
        }
        else if (controller.boostCharge > controller.maxBoost * (3.0 / 8))
        {
            sr.sprite = boostSprites[3];
        }
        else if (controller.boostCharge > controller.maxBoost * (2.0 / 8))
        {
            sr.sprite = boostSprites[2];
        }
        else if (controller.boostCharge > controller.maxBoost * (1.0 / 8))
        {
            sr.sprite = boostSprites[1];
        }
        else
        {
            sr.sprite = boostSprites[0];
        }
    }
}
