using UnityEngine;

public class CarVisualCustomizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer _carRenderer;

    private CarData _carData = new CarData();

    public void Init()
    {
        _carData = SaveManager.JsonStorage.LoadFromJson<CarData>(GameSaveKeys.CarData);
        Config();
        Subscribe();
    }
    private void OnDestroy()
    {
        Unsubscribe();
    }
    private void Config()
    {
        CustomizeMaterial(_carData.carMaterial);
    }
    private void Subscribe()
    {
        CarCustomizationEvents.OnMaterialChanged += CustomizeMaterial;
    }

    private void Unsubscribe()
    {
        CarCustomizationEvents.OnMaterialChanged -= CustomizeMaterial;
    }

    private void CustomizeMaterial(Material material)
    {
        Material[] materials = _carRenderer.materials;
        materials[0] = material;
        _carRenderer.materials = materials;
    }
}
