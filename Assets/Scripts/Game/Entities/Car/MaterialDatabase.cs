using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialDatabase
{
    private static Material[] materials = new Material[] {
        Resources.Load<Material>("BlueCar"),
        Resources.Load<Material>("RedCar"),
        Resources.Load<Material>("GrayCar"),
        Resources.Load<Material>("OrangeCar"),
        Resources.Load<Material>("PurpleCar"),
    };

    public static int GetIndex(Material material)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] == material)
                return i;
        }
        return -1; // якщо матер≥ал не знайдено
    }

    public static Material GetMaterial(int index)
    {
        if (index >= 0 && index < materials.Length)
            return materials[index];
        return null;
    }
}
