using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Engine", menuName = "Car Part/Engine")]
public class CarEngine : ScriptableObject
{
    public float mass;
    [Header("Positive modifiers")]
    public float topSpeedModifier;
    public float accelerationModifier;
    [Header("Negative modifiers")]
    public float handlingModifer;
    public float brakingModifer;

    [Header("Model prefab")]
    public GameObject model;

    [Header("Sprite")]
    public Sprite sprite;
}
