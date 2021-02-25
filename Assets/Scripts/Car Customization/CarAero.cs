using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aero", menuName = "Car Part/Aero")]
public class CarAero : ScriptableObject
{
    public float mass;
    [Header("Negative modifiers")]
    public float topSpeedModifier;
    [Header("Positive modifiers")]
    public float handlingModifier;
    public float brakingModifier;

    [Header("Model prefab")]
    public GameObject model;
}
