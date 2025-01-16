using System;
using UnityEngine;

[Serializable]
public class CarDataInitializer
{
    [SerializeField] private CarConfig _basicCarConfig;
    public void InitData(ref CarData CarData)
    {
        CarData newCarData = new CarData();

        newCarData.maxSpeed = _basicCarConfig.MaxSpee;
        newCarData.maxReverseSpeed = _basicCarConfig.MaxReverseSpeed;
        newCarData.accelerationMultiplier = _basicCarConfig.AccelerationMultiplier;
        newCarData.maxSteeringAngle = _basicCarConfig.MaxSteeringAngle;
        newCarData.steeringSpeed = _basicCarConfig.SteeringSpeed;
        newCarData.brakeForce = _basicCarConfig.BrakeForce;
        newCarData.decelerationMultiplier = _basicCarConfig.DecelerationMultiplier;
        newCarData.handbrakeDriftMultiplier = _basicCarConfig.HandbrakeDriftMultiplier;
        newCarData.bodyMassCenter = _basicCarConfig.BodyMassCenter;
        newCarData.canHandbrake = _basicCarConfig.CanHandbrake;

        CarData = newCarData;
    }
}