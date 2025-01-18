using System;
using UnityEngine;

[Serializable]
public class CarData
{
    public CarTypes carType;
    public Material carMaterial;
    public int maxSpeed;
    public int maxReverseSpeed;
    public int accelerationMultiplier;
    public int maxSteeringAngle;
    public int brakeForce;
    public int decelerationMultiplier;
    public int handBrakeDriftMultiplier;
    public float steeringSpeed;
    public Vector3 bodyMassCenter;
    public bool canHandbrake;
}