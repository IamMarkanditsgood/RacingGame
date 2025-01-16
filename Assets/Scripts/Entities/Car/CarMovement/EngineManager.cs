using System;
using System.Collections;
using UnityEngine;
using static CarMovementManager;

public class EngineManager
{
    private Wheel[] _wheels;

    private Rigidbody _carRigidbody;
   
    private CarData _carData;

    private float _localVelocityZ;
    private float _throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.

    private Coroutine _decelerateCar;

    public event Action OnDrift;
    public void Init(CarData carData, Wheel[] wheels, Rigidbody carRigidbody)
    {
        _carData = carData;
        _wheels = wheels;
        _carRigidbody = carRigidbody;
    }
    public void UpdateLocalVelocityZ(float newLocalVelocityZ)
    {
        _localVelocityZ = newLocalVelocityZ;
    }
    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward(ref float carSpeed)
    {
        OnDrift?.Invoke();
        // The following part sets the throttle power to 1 smoothly.
        SmoothThrottleDecrease(1f);

        if (_localVelocityZ < -1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.RoundToInt(carSpeed) < _carData.maxSpeed)
            {
                ApplyTorque();
            }
            else
            {
                ThrottleChange(0);
            }
        }
    }

    // This method apply negative torque to the wheels in order to go backwards.
    public void GoReverse(ref float carSpeed)
    {
        OnDrift?.Invoke();
        // The following part sets the throttle power to -1 smoothly.
        SmoothThrottleDecrease(-1f);

        if (_localVelocityZ > 1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < _carData.maxReverseSpeed)
            {
                ApplyTorque();
            }
            else
            {
                ThrottleChange(0);
            }
        }
    }
    public void ThrottleChange(float torque)
    {
        foreach (var wheel in _wheels)
        {
            wheel.wheelCollider.motorTorque = torque;
        }
    }
    public void StartCarDevelerating()
    {
        _decelerateCar = CoroutineServices.instance.StartRoutine(DecelerateCarRepeating(0.1f));
    }
    public void StopCarDevelerating()
    {
        CoroutineServices.instance.StopRoutine(_decelerateCar);
    }
    private void Brakes()
    {
        BrakeTorqueChange(_carData.brakeForce);
    }
    private void DecelerateCar()
    {
        OnDrift?.Invoke();
        SmoothThrottleReset();
        ApplyDeceleration();
        StopCarIfSlow();
    }
    private void SmoothThrottleDecrease(float direction)
    {
        _throttleAxis += direction * Time.deltaTime * 3f;
        _throttleAxis = Mathf.Clamp(_throttleAxis, direction, 1f * direction);
    }

    private void ApplyTorque()
    {
        ThrottleChange((_carData.accelerationMultiplier * 50f) * _throttleAxis);
        BrakeTorqueChange(0);
    }
    // This function applies brake torque to the wheels according to the brake force given by the user.
    private void BrakeTorqueChange(float torque)
    {
        foreach (var wheel in _wheels)
        {
            wheel.wheelCollider.brakeTorque = torque;
        }
    }
    private void SmoothThrottleReset()
    {
        if (_throttleAxis != 0f)
        {
            _throttleAxis -= Mathf.Sign(_throttleAxis) * Time.deltaTime * 10f;
            if (Mathf.Abs(_throttleAxis) < 0.15f)
            {
                _throttleAxis = 0f;
            }
        }
    }

    private void ApplyDeceleration()
    {
        _carRigidbody.velocity *= 1f / (1f + (0.025f * _carData.decelerationMultiplier));

        ThrottleChange(0f);
    }

    private void StopCarIfSlow()
    {
        if (_carRigidbody.velocity.magnitude < 0.25f)
        {
            _carRigidbody.velocity = Vector3.zero;
            CoroutineServices.instance.StopRoutine(_decelerateCar);
        }
    }
    public IEnumerator DecelerateCarRepeating(float delay)
    {
        while (true)
        {
            DecelerateCar();
            yield return new WaitForSeconds(delay);
        }
    }
}