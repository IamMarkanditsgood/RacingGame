using System;
using System.Collections;
using UnityEngine;

public class HandbrakeManager
{
    private Wheel[] _wheels;
    private WheelFrictionData[] _wheelsFriction = new WheelFrictionData[4];
    private CarData _carData;

    private bool _isTractionLocked;
    private float _driftingAxis;
    private Coroutine _recoverTraction;
    public event Action OnDrift;

    public void Init(CarData carData, Wheel[] wheels, WheelFrictionData[] wheelsFriction)
    {
        _carData = carData;
        _wheels = wheels;
        _wheelsFriction = wheelsFriction;
    }
    public void HandbrakeActivate()
    {
        Handbrake();
    }
    public void RecoverTraction()
    {
        _isTractionLocked = false;
        CarEvents.TireSkid(_isTractionLocked);
        _driftingAxis = Mathf.Max(0f, _driftingAxis - Time.deltaTime / 1.5f);

        if (_driftingAxis > 0f)
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                UpdateWheelFriction(_wheelsFriction[i].wheelFriction, _wheelsFriction[i].wextremumSlip, _wheels[i].wheelCollider);
            }

            _recoverTraction = CoroutineServices.instance.StartRoutine(RecoverTractionDelay(Time.deltaTime));
        }
        else
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                ResetWheelFriction(_wheels[i].wheelCollider, _wheelsFriction[i].wheelFriction, _wheelsFriction[i].wextremumSlip);
            }
            _driftingAxis = 0f;
        }
    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    private void Handbrake()
    {
        CoroutineServices.instance.StopRoutine(_recoverTraction);

        _driftingAxis = Mathf.Min(1f, _driftingAxis + Time.deltaTime);

        float secureStartingPoint = _driftingAxis * _wheelsFriction[0].wextremumSlip * _carData.handBrakeDriftMultiplier;

        if (secureStartingPoint < _wheelsFriction[0].wextremumSlip)
        {
            _driftingAxis = _wheelsFriction[0].wextremumSlip / (_wheelsFriction[0].wextremumSlip * _carData.handBrakeDriftMultiplier);
        }

        OnDrift?.Invoke();

        if (_driftingAxis < 1f)
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                UpdateWheelFriction(_wheelsFriction[i].wheelFriction, _wheelsFriction[i].wextremumSlip, _wheels[i].wheelCollider);
            }
        }

        _isTractionLocked = true;
        CarEvents.TireSkid(_isTractionLocked);
    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    private void UpdateWheelFriction(WheelFrictionCurve wheelFriction, float extremumSlip, WheelCollider wheelCollider)
    {
        wheelFriction.extremumSlip = extremumSlip * _carData.handBrakeDriftMultiplier * _driftingAxis;
        wheelCollider.sidewaysFriction = wheelFriction;
    }

    private void ResetWheelFriction(WheelCollider wheelCollider, WheelFrictionCurve wheelFriction, float extremumSlip)
    {
        wheelFriction.extremumSlip = extremumSlip;
        wheelCollider.sidewaysFriction = wheelFriction;
    }

    private IEnumerator RecoverTractionDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        RecoverTraction();
        CoroutineServices.instance.StopRoutine(_recoverTraction);
    }
}