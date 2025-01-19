using UnityEngine;

[CreateAssetMenu(fileName = "BasicCar", menuName = "ScriptableObjects/Car/BasicCar", order = 1)]
public class CarConfig : ScriptableObject
{
    [Header("CarSetup")]
    [Space(10)]
    [Header("Car type")]
    [SerializeField] private CarTypes _carType = CarTypes.CarBasic;

    [Header("Wheels color")]
    [SerializeField] private Material _carMaterial;

    [Header("The maximum speed that the car can reach in km/h.")]
    [Range(20, 190)]
    [SerializeField] private int _maxSpeed = 90;

    [Header("The maximum speed that the car can reach while going on reverse in km/h.")]
    [Range(10, 120)]
    [SerializeField] private int _maxReverseSpeed = 45; 

    [Header("How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.")]
    [Range(1, 10)]
    [SerializeField] private int _accelerationMultiplier = 2; 

    [Header("The maximum angle that the tires can reach while rotating the steering wheel.")]
    [Range(10, 45)]
    [SerializeField] private int _maxSteeringAngle = 27; 

    [Header("The strength of the wheel brakes.")]
    [Range(100, 600)]
    [SerializeField] private int _brakeForce = 350; 

    [Header("How fast the car decelerates when the user is not using the throttle.")]
    [Range(1, 10)]
    [SerializeField] private int _decelerationMultiplier = 2; 

    [Header("How much grip the car loses when the user hit the handbrake.")]
    [Range(1, 10)]
    [SerializeField] private int _handBrakeDriftMultiplier = 5;

    [Header("How fast the steering wheel turns.")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _steeringSpeed = 0.5f;

    [Header("This is a vector that contains the center of mass of the car. I recommend to set this value")]
    [SerializeField] private Vector3 _bodyMassCenter; 

    [Space(20)]
    [Header("Handbrake")]
    [SerializeField] private bool _canHandbrake = false;
    public CarTypes CarType => _carType;
    public Material CarMaterial => _carMaterial;
    public int MaxSpeed => _maxSpeed;
    public int MaxReverseSpeed => _maxReverseSpeed;
    public int AccelerationMultiplier => _accelerationMultiplier;
    public int MaxSteeringAngle => _maxSteeringAngle;
    public int BrakeForce => _brakeForce;
    public int DecelerationMultiplier => _decelerationMultiplier;
    public int HandbrakeDriftMultiplier => _handBrakeDriftMultiplier;
    public float SteeringSpeed => _steeringSpeed;
    public Vector3 BodyMassCenter => _bodyMassCenter;
    public bool CanHandbrake => _canHandbrake;
}