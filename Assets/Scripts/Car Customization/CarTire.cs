using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tire", menuName = "Car Part/Tire")]
public class CarTire : ScriptableObject
{
    public float mass;
    [Header("Positive modifiers")]
    public float accelerationModifier;
    public float handlingModifer;
    public float brakingModifer;

    [Header("Model prefab")]
    public GameObject model;

    [Header("Sprite")]
    public Sprite sprite;
}
