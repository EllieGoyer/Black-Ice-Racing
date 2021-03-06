﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car Body", menuName = "Car Part/Body")]
public class CarBody : ScriptableObject
{
    public float mass;
    [Header("The car body defines the base stats of the car.")]
    public float topSpeedBase;
    public float accelerationBase;
    public float handlingBase;
    public float brakingBase;

    [Header("Model prefab")]
    public GameObject model;

    [Header("Sprite")]
    public Sprite sprite;
}
