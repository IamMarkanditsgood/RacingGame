using System;
using UnityEngine;

public class CarDataManager : MonoBehaviour
{
    [SerializeField] private CarConfig _carConfig;

    [SerializeField] private CarData _carData = new CarData();

    public CarData CarData => _carData;

    public void Subscribe()
    {
        CarEvents.OnCarParameterModified += ModifyParameter;
    }
    public void Unsubscribe()
    {
        CarEvents.OnCarParameterModified -= ModifyParameter;
    }

    public void Init()
    {
        if (SaveManager.JsonStorage.Exists(GameSaveKeys.CarData))
        {
            LoadData();
        }
        else
        {
            SetData();
            SaveManager.JsonStorage.SaveToJson(GameSaveKeys.CarData, _carData);
        }
        
    }
    private void SetData()
    {
        _carData.carType = _carConfig.CarType;
        _carData.carMaterial = _carConfig.CarMaterial;
        _carData.maxSpeed = _carConfig.MaxSpeed;
        _carData.maxReverseSpeed = _carConfig.MaxReverseSpeed;
        _carData.accelerationMultiplier = _carConfig.AccelerationMultiplier;
        _carData.maxSteeringAngle = _carConfig.MaxSteeringAngle;
        _carData.steeringSpeed = _carConfig.SteeringSpeed;
        _carData.brakeForce = _carConfig.BrakeForce;
        _carData.decelerationMultiplier = _carConfig.DecelerationMultiplier;
        _carData.handBrakeDriftMultiplier = _carConfig.HandbrakeDriftMultiplier;
        _carData.bodyMassCenter = _carConfig.BodyMassCenter;
        _carData.canHandbrake = _carConfig.CanHandbrake;
    }

    private void LoadData()
    {
        _carData = SaveManager.JsonStorage.LoadFromJson<CarData>(GameSaveKeys.CarData);
    }

    private void ModifyParameter(CarParameters carParameter, object value)
    {
        CarParametersModifier carParametersModifier = new CarParametersModifier();

        switch (carParameter)
        {
            case CarParameters.CarType:
                carParametersModifier.CarType(ref _carData, value);
                break;
            case CarParameters.CarColor:
                carParametersModifier.CarMaterial(ref _carData, value);
                break;
            case CarParameters.MaxSpeed:
                carParametersModifier.MaxSpeed(ref _carData, value);
                break;
            case CarParameters.MaxReverseSpeed:
                carParametersModifier.MaxReverseSpeed(ref _carData, value);
                break;
            case CarParameters.AccelerationMultiplier:
                carParametersModifier.AccelerationMultiplier(ref _carData, value);
                break;
            case CarParameters.MaxSteeringAngle:
                carParametersModifier.MaxSteeringAngle(ref _carData, value);
                break;
            case CarParameters.BrakeForce:
                carParametersModifier.BrakeForce(ref _carData, value);
                break;
            case CarParameters.DecelerationMultiplier:
                carParametersModifier.DecelerationMultiplier(ref _carData, value);
                break;
            case CarParameters.HandbrakeDriftMultiplier:
                carParametersModifier.HandbrakeDriftMultiplier(ref _carData, value);
                break;
            case CarParameters.SteeringSpeed:
                carParametersModifier.SteeringSpeed(ref _carData, value);
                break;
            case CarParameters.BodyMassCenter:
                carParametersModifier.BodyMassCenter(ref _carData, value);
                break;
            case CarParameters.CanHandbrake:
                carParametersModifier.CanHandbrake(ref _carData, value);
                break;
        }
        SaveManager.JsonStorage.SaveToJson(GameSaveKeys.CarData, _carData);
    }
}
