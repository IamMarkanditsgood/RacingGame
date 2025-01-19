using System;
using UnityEngine;

public static class CarCustomizationEvents 
{
    public static event Action<Material> OnMaterialChanged;

    public static void ChangeMaterial(Material material) => OnMaterialChanged?.Invoke(material);
}