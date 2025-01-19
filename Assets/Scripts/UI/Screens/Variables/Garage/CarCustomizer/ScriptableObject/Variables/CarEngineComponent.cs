using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Engine", menuName = "ScriptableObjects/UI/Car/Engines", order = 1)]
public class CarEngineComponent : BasicCarComponentConfig
{
    [SerializeField] private List<CarParameters> _carIntParameters;
    [SerializeField] private List<int> _intValue;

    public List<CarParameters> CarIntParameters => _carIntParameters;
    public List<int> IntValue => _intValue;
}