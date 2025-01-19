using UnityEngine;

[CreateAssetMenu(fileName = "BasicCarComponent", menuName = "ScriptableObjects/UI/Car/CarComponent", order = 1)]
public class BasicCarComponentConfig : ScriptableObject 
{
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _componentImage;

    public string Name => _name;
    public int Price => _price;
    public Sprite ComponentImage => _componentImage;
}