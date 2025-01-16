using UnityEngine;

public class SteeringManager
{
    private Wheel[] _wheels;
    private CarData _carData;

    private float _steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.

    public void Init(CarData carData, Wheel[] wheels)
    {
        _carData = carData;
        _wheels = wheels;
    }

    public void TurnLeft()
    {
        TurnSteering(-1f);
    }

    public void TurnRight()
    {
        TurnSteering(1f);
    }

    //The following method takes the front car wheels to their default position (rotation = 0). The speed of this movement will depend
    // on the steeringSpeed variable.
    public void ResetSteeringAngle()
    {
        SmoothSteeringInput();

        if (Mathf.Abs(_wheels[0].wheelCollider.steerAngle) < 1f)
        {
            _steeringAxis = 0f;
        }

        var steeringAngle = _steeringAxis * _carData.maxSteeringAngle;
        SetSteeringAngle(steeringAngle);
    }

    private void TurnSteering(float direction)
    {
        _steeringAxis += direction * Time.deltaTime * 10f * _carData.steeringSpeed;
        _steeringAxis = Mathf.Clamp(_steeringAxis, -1f, 1f);

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
}