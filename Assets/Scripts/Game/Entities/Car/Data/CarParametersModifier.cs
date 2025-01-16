using TMPro;
using UnityEngine;

public class CarParametersModifier
{
    public void CarType(ref CarData carData, object value)
    {
        carData.carType = (CarTypes)value;
    }
    public void CarMaterial(ref CarData carData, object value)
    {
        carData.carMaterial = (Material)value;
    }
    public void MaxSpeed(ref CarData carData, object value)
    {
        carData.maxSpeed = (int) value;
    }
    public void MaxReverseSpeed(ref CarData carData, object value)
    {
        carData.maxReverseSpeed = (int)value;
    }
    public void AccelerationMultiplier(ref CarData carData, object value)
    {
        carData.accelerationMultiplier = (int)value;
    }
    public void MaxSteeringAngle(ref CarData carData, object value)
    {
        carData.maxSteeringAngle = (int)value;
    }
    public void BrakeForce(ref CarData carData, object value)
    {
        carData.brakeForce = (int)value;
    }
    public void DecelerationMultiplier(ref CarData carData, object value)
    {
        carData.decelerationMultiplier = (int)value;
    }
    public void HandbrakeDriftMultiplier(ref CarData carData, object value)
    {
        carData.handbrakeDriftMultiplier = (int)value;
    }
    public void SteeringSpeed(ref CarData carData, object value)
    {
        carData.steeringSpeed = (float)value;
    }
    public void BodyMassCenter(ref CarData carData, object value)
    {
        carData.bodyMassCenter = (Vector3)value;
    }
    public void CanHandbrake(ref CarData carData, object value)
    {
        carData.canHandbrake = (bool)value;
    }
}