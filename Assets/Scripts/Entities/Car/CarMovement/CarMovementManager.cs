﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CarMovementManager
{
    [Header("Wheels (FL,FR,RL,RR)")]
    [SerializeField] private Wheel[] _wheels;

    [Serializable]
    public class Wheel
    {
        public GameObject wheelMesh;
        public WheelCollider wheelCollider;
    }

    [SerializeField] private float _carSpeed;
    [SerializeField] private bool _isDrifting;
    [SerializeField] private bool _isTractionLocked;

    private Rigidbody _carRigidbody;
    private float _steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
    private float _throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
    private float _driftingAxis;
    private float _localVelocityZ;
    private float _localVelocityX;
    private bool _deceleratingCar;

    private WheelFrictionData[] _wheelsFriction = new WheelFrictionData[4];
    public class WheelFrictionData
    {
        public WheelFrictionCurve wheelFriction;
        public float wextremumSlip;
    }

    private CarData _carData = new CarData();

    private Coroutine _decelerateCar;
    private Coroutine _recoverTraction;

    public void Init(CarData carData, GameObject carBody)
    {
        _carData = carData;

        _carRigidbody = carBody.GetComponent<Rigidbody>();
        _carRigidbody.centerOfMass = carData.bodyMassCenter;

        InitWheels();   
    }

    public void UpdateCarData(GameObject car)
    {
        _carSpeed = (2 * Mathf.PI * _wheels[0].wheelCollider.radius * _wheels[0].wheelCollider.rpm * 60) / 1000;

        _localVelocityX = car.transform.InverseTransformDirection(_carRigidbody.velocity).x;
        _localVelocityZ = car.transform.InverseTransformDirection(_carRigidbody.velocity).z;
    }
    public void Subscribe()
    {
        InputEvents.OnWPressed += GoForwardHandler;
        InputEvents.OnSPressed += GoReverseHandler;
        InputEvents.OnAPressed += TurnLeftHandler;
        InputEvents.OnDPressed += TurnRightHandler;
        InputEvents.OnSpacePressed += HandbrakeHandler;
    }
    public void Unsubscribe()
    {
        InputEvents.OnWPressed -= GoForwardHandler;
        InputEvents.OnSPressed -= GoReverseHandler;
        InputEvents.OnAPressed -= TurnLeftHandler;
        InputEvents.OnDPressed -= TurnRightHandler;
        InputEvents.OnSpacePressed -= HandbrakeHandler;
    }
    public void CheckIdleConditions(CarInputSystem carInputSystem)
    {
        if (!carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.S) && !carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.W))
        {
            ThrottleChange(0);
        }

        if (!carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.S) && !carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.W) && !carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.Space) && !_deceleratingCar)
        {
            _decelerateCar = CoroutineServices.instance.StartRoutine(DecelerateCarRepeating(0.1f));
            _deceleratingCar = true;
        }

        if (!carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.A) && !carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.D))
        {
            ResetSteeringAngle();
        }
    }
    public void AnimateWheelMeshes()
    {
        foreach (var wheel in _wheels)
        {
            UpdateWheelPose(wheel.wheelCollider, wheel.wheelMesh);
        }
    }
    private void GoForwardHandler()
    {
        CoroutineServices.instance.StopRoutine(_decelerateCar);
        _deceleratingCar = false;
        GoForward();
    }

    private void GoReverseHandler()
    {
        CoroutineServices.instance.StopRoutine(_decelerateCar);
        _deceleratingCar = false;
        GoReverse();
    }

    private void TurnLeftHandler()
    {
        TurnLeft();
    }

    private void TurnRightHandler()
    {
        TurnRight();
    }

    private void HandbrakeHandler(int state)
    {
        if (state == 1)
        {
            CoroutineServices.instance.StopRoutine(_decelerateCar);
            _deceleratingCar = false;
            Handbrake();
        }
        else if (state == 0)
        {
            RecoverTraction();
        }
    }

    private void InitWheels()
    {
        for (int i = 0; i < _wheelsFriction.Length; i++)
        {
            _wheelsFriction[i] = new WheelFrictionData();
        }

        for (int i = 0; i < _wheels.Length; i++)
        {
            InitWheelFriction(_wheels[i].wheelCollider, ref _wheelsFriction[i].wheelFriction, ref _wheelsFriction[i].wextremumSlip);
        }
    }

    private void InitWheelFriction(WheelCollider collider, ref WheelFrictionCurve wheelFriction, ref float wheelFrictionExtremumSlip)
    {
        wheelFriction = new WheelFrictionCurve
        {
            extremumSlip = collider.sidewaysFriction.extremumSlip,
            extremumValue = collider.sidewaysFriction.extremumValue,
            asymptoteSlip = collider.sidewaysFriction.asymptoteSlip,
            asymptoteValue = collider.sidewaysFriction.asymptoteValue,
            stiffness = collider.sidewaysFriction.stiffness
        };

        wheelFrictionExtremumSlip = collider.sidewaysFriction.extremumSlip;
    }

    private void TurnLeft()
    {
        TurnSteering(-1f);
    }

    private void TurnRight()
    {
        TurnSteering(1f);
    }

    private void TurnSteering(float direction)
    {
        _steeringAxis += direction * Time.deltaTime * 10f * _carData.steeringSpeed;
        _steeringAxis = Mathf.Clamp(_steeringAxis, -1f, 1f);

        var steeringAngle = _steeringAxis * _carData.maxSteeringAngle;
        SetSteeringAngle(steeringAngle);
    }

    //The following method takes the front car wheels to their default position (rotation = 0). The speed of this movement will depend
    // on the steeringSpeed variable.
    private void ResetSteeringAngle()
    {
        SmoothSteeringInput();

        if (Mathf.Abs(_wheels[0].wheelCollider.steerAngle) < 1f)
        {
            _steeringAxis = 0f;
        }

        var steeringAngle = _steeringAxis * _carData.maxSteeringAngle;
        SetSteeringAngle(steeringAngle);
    }

    private void SmoothSteeringInput()
    {
        if (_steeringAxis < 0f)
        {
            _steeringAxis += Time.deltaTime * 10f * _carData.steeringSpeed;
        }
        else if (_steeringAxis > 0f)
        {
            _steeringAxis -= Time.deltaTime * 10f * _carData.steeringSpeed;
        }
    }

    private void SetSteeringAngle(float steeringAngle)
    {
        _wheels[0].wheelCollider.steerAngle = Mathf.Lerp(_wheels[0].wheelCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
        _wheels[1].wheelCollider.steerAngle = Mathf.Lerp(_wheels[1].wheelCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, GameObject wheelMesh)
    {
        wheelCollider.GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);

        wheelMesh.transform.position = wheelPosition;
        wheelMesh.transform.rotation = wheelRotation;
    }

    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward()
    {
        HandleDriftEffect();
        // The following part sets the throttle power to 1 smoothly.
        SmoothThrottleDecrease(1f);

        if (_localVelocityZ < -1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.RoundToInt(_carSpeed) < _carData.maxSpeed)
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
    public void GoReverse()
    {
        HandleDriftEffect();
        // The following part sets the throttle power to -1 smoothly.
        SmoothThrottleDecrease(-1f);

        if (_localVelocityZ > 1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.Abs(Mathf.RoundToInt(_carSpeed)) < _carData.maxReverseSpeed)
            {
                ApplyTorque();
            }
            else
            {
                ThrottleChange(0);
            }
        }
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

    private void ThrottleChange(float torque)
    {
        foreach (var wheel in _wheels)
        {
            wheel.wheelCollider.motorTorque = torque;
        }
    }
    private void BrakeTorqueChange(float torque)
    {
        foreach (var wheel in _wheels)
        {
            wheel.wheelCollider.brakeTorque = torque;
        }
    }

    private void DecelerateCar()
    {
        HandleDriftEffect();
        SmoothThrottleReset();
        ApplyDeceleration();
        StopCarIfSlow();
    }

    private void HandleDriftEffect()
    {
        _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
        CarEvents.DriftEffect(_isDrifting);
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

    // This function applies brake torque to the wheels according to the brake force given by the user.
    private void Brakes()
    {
        BrakeTorqueChange(_carData.brakeForce);
    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    private void Handbrake()
    {
        CoroutineServices.instance.StopRoutine(_recoverTraction);

        _driftingAxis = Mathf.Min(1f, _driftingAxis + Time.deltaTime);

        float secureStartingPoint = _driftingAxis * _wheelsFriction[0].wextremumSlip * _carData.handbrakeDriftMultiplier;

        if (secureStartingPoint < _wheelsFriction[0].wextremumSlip)
        {
            _driftingAxis = _wheelsFriction[0].wextremumSlip / (_wheelsFriction[0].wextremumSlip * _carData.handbrakeDriftMultiplier);
        }

        _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;

        if (_driftingAxis < 1f)
        {
            for(int i = 0; i < _wheels.Length; i++)
            {
                UpdateWheelFriction(_wheelsFriction[i].wheelFriction, _wheelsFriction[i].wextremumSlip, _wheels[i].wheelCollider);
            }
        }

        _isTractionLocked = true;
        CarEvents.TireSkid(_isTractionLocked);
    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    private void RecoverTraction()
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
                ResetWheelFriction( _wheels[i].wheelCollider, _wheelsFriction[i].wheelFriction, _wheelsFriction[i].wextremumSlip);
            }
            _driftingAxis = 0f;
        }
    }

    private void UpdateWheelFriction(WheelFrictionCurve wheelFriction, float extremumSlip, WheelCollider wheelCollider)
    {
        wheelFriction.extremumSlip = extremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
        wheelCollider.sidewaysFriction = wheelFriction;
    }

    private void ResetWheelFriction(WheelCollider wheelCollider, WheelFrictionCurve wheelFriction, float extremumSlip)
    {
        wheelFriction.extremumSlip = extremumSlip;
        wheelCollider.sidewaysFriction = wheelFriction;
    }

    private IEnumerator DecelerateCarRepeating(float delay)
    {
        while (true)
        {
            DecelerateCar();
            yield return new WaitForSeconds(delay);
        }
    }
    private IEnumerator RecoverTractionDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        RecoverTraction();
        CoroutineServices.instance.StopRoutine(_recoverTraction);
    }
}