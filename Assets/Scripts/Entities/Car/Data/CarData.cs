using System;
using UnityEngine;

[Serializable]
public class CarData
{
    public int maxSpeed;
    public int maxReverseSpeed;
    public int accelerationMultiplier;
    public int maxSteeringAngle;
    public float steeringSpeed;
    public int brakeForce;
    public int decelerationMultiplier;
    public int handbrakeDriftMultiplier;
    public Vector3 bodyMassCenter;
    public bool canHandbrake;
}