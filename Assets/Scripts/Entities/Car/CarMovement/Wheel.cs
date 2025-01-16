using System;
using UnityEngine;

public partial class CarMovementManager
{
    [Serializable]
    public class Wheel
    {
        public GameObject wheelMesh;
        public WheelCollider wheelCollider;
    }
}
