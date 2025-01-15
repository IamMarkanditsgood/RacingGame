using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CarMovementManager
{
    [Header("Wheels")]
    [SerializeField] private GameObject frontLeftMesh;
    [SerializeField] private WheelCollider frontLeftCollider;
    [Space(10)]
    [SerializeField] private GameObject frontRightMesh;
    [SerializeField] private WheelCollider frontRightCollider;
    [Space(10)]
    [SerializeField] private GameObject rearLeftMesh;
    [SerializeField] private WheelCollider rearLeftCollider;
    [Space(10)]
    [SerializeField] private GameObject rearRightMesh;
    [SerializeField] private WheelCollider rearRightCollider;


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

    private WheelFrictionCurve FLwheelFriction;
    private float FLWextremumSlip;
    private WheelFrictionCurve FRwheelFriction;
    private float FRWextremumSlip;
    private WheelFrictionCurve RLwheelFriction;
    private float RLWextremumSlip;
    private WheelFrictionCurve RRwheelFriction;
    private float RRWextremumSlip;

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
        _carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
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

    public void CheckIdleConditions(CarInputSystem carInputSystem)
    {
        if (!carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.S) && !carInputSystem.IsPressed(new KeyboardInputSystem(), KeyCode.W))
        {
            ThrottleOff();
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
    private void InitWheels()
    {
        FLwheelFriction = new WheelFrictionCurve();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;

        FRwheelFriction = new WheelFrictionCurve();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;

        RLwheelFriction = new WheelFrictionCurve();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;

        RRwheelFriction = new WheelFrictionCurve();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;
    }
    //
    //STEERING METHODS
    //

    //The following method turns the front car wheels to the left. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnLeft()
    {
        _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * _carData.steeringSpeed);
        if (_steeringAxis < -1f)
        {
            _steeringAxis = -1f;
        }
        var steeringAngle = _steeringAxis * _carData.maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
    }

    //The following method turns the front car wheels to the right. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnRight()
    {
        _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * _carData.steeringSpeed);
        if (_steeringAxis > 1f)
        {
            _steeringAxis = 1f;
        }
        var steeringAngle = _steeringAxis * _carData.maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
    }

    //The following method takes the front car wheels to their default position (rotation = 0). The speed of this movement will depend
    // on the steeringSpeed variable.
    public void ResetSteeringAngle()
    {
        if (_steeringAxis < 0f)
        {
            _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * _carData.steeringSpeed);
        }
        else if (_steeringAxis > 0f)
        {
            _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * _carData.steeringSpeed);
        }
        if (Mathf.Abs(frontLeftCollider.steerAngle) < 1f)
        {
            _steeringAxis = 0f;
        }
        var steeringAngle = _steeringAxis * _carData.maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, _carData.steeringSpeed);
    }

    // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
    public void AnimateWheelMeshes()
    {
        try
        {
            Quaternion FLWRotation;
            Vector3 FLWPosition;
            frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
            frontLeftMesh.transform.position = FLWPosition;
            frontLeftMesh.transform.rotation = FLWRotation;

            Quaternion FRWRotation;
            Vector3 FRWPosition;
            frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
            frontRightMesh.transform.position = FRWPosition;
            frontRightMesh.transform.rotation = FRWRotation;

            Quaternion RLWRotation;
            Vector3 RLWPosition;
            rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
            rearLeftMesh.transform.position = RLWPosition;
            rearLeftMesh.transform.rotation = RLWRotation;

            Quaternion RRWRotation;
            Vector3 RRWPosition;
            rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
            rearRightMesh.transform.position = RRWPosition;
            rearRightMesh.transform.rotation = RRWRotation;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    //
    //ENGINE AND BRAKING METHODS
    //

    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward()
    {
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car is losing traction, then the car will start emitting particle systems.
        if (Mathf.Abs(_localVelocityX) > 2.5f)
        {
            _isDrifting = true;
            CarEvents.DriftEffect(_isDrifting);
        }
        else
        {
            _isDrifting = false;
            CarEvents.DriftEffect(_isDrifting);
        }
        // The following part sets the throttle power to 1 smoothly.
        _throttleAxis = _throttleAxis + (Time.deltaTime * 3f);
        if (_throttleAxis > 1f)
        {
            _throttleAxis = 1f;
        }
        //If the car is going backwards, then apply brakes in order to avoid strange
        //behaviours. If the local velocity in the 'z' axis is less than -1f, then it
        //is safe to apply positive torque to go forward.
        if (_localVelocityZ < -1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.RoundToInt(_carSpeed) < _carData.maxSpeed)
            {
                //Apply positive torque in all wheels to go forward if maxSpeed has not been reached.
                frontLeftCollider.brakeTorque = 0;
                frontLeftCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
                frontRightCollider.brakeTorque = 0;
                frontRightCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
                rearLeftCollider.brakeTorque = 0;
                rearLeftCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
                rearRightCollider.brakeTorque = 0;
                rearRightCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
            }
            else
            {
                // If the maxSpeed has been reached, then stop applying torque to the wheels.
                // IMPORTANT: The maxSpeed variable should be considered as an approximation; the speed of the car
                // could be a bit higher than expected.
                frontLeftCollider.motorTorque = 0;
                frontRightCollider.motorTorque = 0;
                rearLeftCollider.motorTorque = 0;
                rearRightCollider.motorTorque = 0;
            }
        }
    }

    // This method apply negative torque to the wheels in order to go backwards.
    public void GoReverse()
    {
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car is losing traction, then the car will start emitting particle systems.
        if (Mathf.Abs(_localVelocityX) > 2.5f)
        {
            _isDrifting = true;
            CarEvents.DriftEffect(_isDrifting);
        }
        else
        {
            _isDrifting = false;
            CarEvents.DriftEffect(_isDrifting);
        }
        // The following part sets the throttle power to -1 smoothly.
        _throttleAxis = _throttleAxis - (Time.deltaTime * 3f);
        if (_throttleAxis < -1f)
        {
            _throttleAxis = -1f;
        }
        //If the car is still going forward, then apply brakes in order to avoid strange
        //behaviours. If the local velocity in the 'z' axis is greater than 1f, then it
        //is safe to apply negative torque to go reverse.
        if (_localVelocityZ > 1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.Abs(Mathf.RoundToInt(_carSpeed)) < _carData.maxReverseSpeed)
            {
                //Apply negative torque in all wheels to go in reverse if maxReverseSpeed has not been reached.
                frontLeftCollider.brakeTorque = 0;
                frontLeftCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
                frontRightCollider.brakeTorque = 0;
                frontRightCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
                rearLeftCollider.brakeTorque = 0;
                rearLeftCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
                rearRightCollider.brakeTorque = 0;
                rearRightCollider.motorTorque = (_carData.accelerationMultiplier * 50f) * _throttleAxis;
            }
            else
            {
                //If the maxReverseSpeed has been reached, then stop applying torque to the wheels.
                // IMPORTANT: The maxReverseSpeed variable should be considered as an approximation; the speed of the car
                // could be a bit higher than expected.
                frontLeftCollider.motorTorque = 0;
                frontRightCollider.motorTorque = 0;
                rearLeftCollider.motorTorque = 0;
                rearRightCollider.motorTorque = 0;
            }
        }
    }

    //The following function set the motor torque to 0 (in case the user is not pressing either W or S).
    public void ThrottleOff()
    {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
    }

    // The following method decelerates the speed of the car according to the decelerationMultiplier variable, where
    // 1 is the slowest and 10 is the fastest deceleration. This method is called by the function InvokeRepeating,
    // usually every 0.1f when the user is not pressing W (throttle), S (reverse) or Space bar (handbrake).
    public void DecelerateCar()
    {
        if (Mathf.Abs(_localVelocityX) > 2.5f)
        {
            _isDrifting = true;
            CarEvents.DriftEffect(_isDrifting);
        }
        else
        {
            _isDrifting = false;
            CarEvents.DriftEffect(_isDrifting);
        }
        // The following part resets the throttle power to 0 smoothly.
        if (_throttleAxis != 0f)
        {
            if (_throttleAxis > 0f)
            {
                _throttleAxis = _throttleAxis - (Time.deltaTime * 10f);
            }
            else if (_throttleAxis < 0f)
            {
                _throttleAxis = _throttleAxis + (Time.deltaTime * 10f);
            }
            if (Mathf.Abs(_throttleAxis) < 0.15f)
            {
                _throttleAxis = 0f;
            }
        }
        _carRigidbody.velocity = _carRigidbody.velocity * (1f / (1f + (0.025f * _carData.decelerationMultiplier)));
        // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
        // If the magnitude of the car's velocity is less than 0.25f (very slow velocity), then stop the car completely and
        // also cancel the invoke of this method.
        if (_carRigidbody.velocity.magnitude < 0.25f)
        {
            _carRigidbody.velocity = Vector3.zero;
            CoroutineServices.instance.StopRoutine(_decelerateCar);
        }
    }

    // This function applies brake torque to the wheels according to the brake force given by the user.
    public void Brakes()
    {
        frontLeftCollider.brakeTorque = _carData.brakeForce;
        frontRightCollider.brakeTorque = _carData.brakeForce;
        rearLeftCollider.brakeTorque = _carData.brakeForce;
        rearRightCollider.brakeTorque = _carData.brakeForce;
    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    public void Handbrake()
    {
        CoroutineServices.instance.StopRoutine(_recoverTraction);
        // We are going to start losing traction smoothly, there is were our 'driftingAxis' variable takes
        // place. This variable will start from 0 and will reach a top value of 1, which means that the maximum
        // drifting value has been reached. It will increase smoothly by using the variable Time.deltaTime.
        _driftingAxis = _driftingAxis + (Time.deltaTime);
        float secureStartingPoint = _driftingAxis * FLWextremumSlip * _carData.handbrakeDriftMultiplier;

        if (secureStartingPoint < FLWextremumSlip)
        {
            _driftingAxis = FLWextremumSlip / (FLWextremumSlip * _carData.handbrakeDriftMultiplier);
        }
        if (_driftingAxis > 1f)
        {
            _driftingAxis = 1f;
        }
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car lost its traction, then the car will start emitting particle systems.
        if (Mathf.Abs(_localVelocityX) > 2.5f)
        {
            _isDrifting = true;
        }
        else
        {
            _isDrifting = false;
        }
        //If the 'driftingAxis' value is not 1f, it means that the wheels have not reach their maximum drifting
        //value, so, we are going to continue increasing the sideways friction of the wheels until driftingAxis
        // = 1f.
        if (_driftingAxis < 1f)
        {
            FLwheelFriction.extremumSlip = FLWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            frontLeftCollider.sidewaysFriction = FLwheelFriction;

            FRwheelFriction.extremumSlip = FRWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            frontRightCollider.sidewaysFriction = FRwheelFriction;

            RLwheelFriction.extremumSlip = RLWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            rearLeftCollider.sidewaysFriction = RLwheelFriction;

            RRwheelFriction.extremumSlip = RRWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            rearRightCollider.sidewaysFriction = RRwheelFriction;
        }

        // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
        // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
        _isTractionLocked = true;
        CarEvents.TireSkid(_isTractionLocked);
    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    public void RecoverTraction()
    {
        _isTractionLocked = false;
        CarEvents.TireSkid(_isTractionLocked);
        _driftingAxis = _driftingAxis - (Time.deltaTime / 1.5f);
        if (_driftingAxis < 0f)
        {
            _driftingAxis = 0f;
        }

        //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
        //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
        // car's grip.
        if (FLwheelFriction.extremumSlip > FLWextremumSlip)
        {
            FLwheelFriction.extremumSlip = FLWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            frontLeftCollider.sidewaysFriction = FLwheelFriction;

            FRwheelFriction.extremumSlip = FRWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            frontRightCollider.sidewaysFriction = FRwheelFriction;

            RLwheelFriction.extremumSlip = RLWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            rearLeftCollider.sidewaysFriction = RLwheelFriction;

            RRwheelFriction.extremumSlip = RRWextremumSlip * _carData.handbrakeDriftMultiplier * _driftingAxis;
            rearRightCollider.sidewaysFriction = RRwheelFriction;

            _recoverTraction = CoroutineServices.instance.StartRoutine(RecoverTractionDelay(Time.deltaTime));

        }
        else if (FLwheelFriction.extremumSlip < FLWextremumSlip)
        {
            FLwheelFriction.extremumSlip = FLWextremumSlip;
            frontLeftCollider.sidewaysFriction = FLwheelFriction;

            FRwheelFriction.extremumSlip = FRWextremumSlip;
            frontRightCollider.sidewaysFriction = FRwheelFriction;

            RLwheelFriction.extremumSlip = RLWextremumSlip;
            rearLeftCollider.sidewaysFriction = RLwheelFriction;

            RRwheelFriction.extremumSlip = RRWextremumSlip;
            rearRightCollider.sidewaysFriction = RRwheelFriction;

            _driftingAxis = 0f;
        }
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