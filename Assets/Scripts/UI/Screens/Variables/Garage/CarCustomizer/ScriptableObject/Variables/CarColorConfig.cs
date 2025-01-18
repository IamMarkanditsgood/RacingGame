using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarColor", menuName = "ScriptableObjects/UI/Car/CarColor", order = 1)]
public class CarColorConfig : BasicCarComponentConfig
{
    [SerializeField] private CarParameters _carParameter;
    [SerializeField] private Material _material;
    [SerializeField] private Color _ImageColor;

    public CarParameters CarParameter => _carParameter;
    public Material Material => _material;
    public Color ImageColor => _ImageColor;
}
