using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wheels", menuName = "ScriptableObjects/UI/Car/Wheels", order = 1)]
public class CarWheelsComponent : BasicCarComponentConfig
{
    [SerializeField] private List<CarParameters> _carIntParameters;
    [SerializeField] private List<int> _intValue;
    [SerializeField] private List<CarParameters> _carFloatParameters;
    [SerializeField] private List<float> _floatValue;

    public List<CarParameters> CarIntParameters => _carIntParameters;
    public List<int> IntValue => _intValue;
    public List<CarParameters> CarFloatParameters => _carFloatParameters;
    public List<float> FloatValue => _floatValue;
}
