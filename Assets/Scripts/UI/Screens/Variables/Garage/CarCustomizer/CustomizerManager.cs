using System;
using System.Collections.Generic;
using UnityEngine;
using static CustomizerManager;

[Serializable]
public class CustomizerManager
{
    [SerializeField] private List<CarComponentPanels> _panelsCollection;

    [SerializeField] private Transform _container;
    private List<BasicComponentPanel> _panels = new List<BasicComponentPanel>();

    private const int _basicComponentIndex = 0;
    [Serializable]
    public class CarComponentPanels
    {
        public CarCastomizedComponents type;
        public BasicComponentPanel basicComponentPanels;
        public List<BasicCarComponentConfig> basicCarComponentConfigs;       
    }

    public void SetCastomizer(CarCastomizedComponents componentsType)
    {
        Reset();

        foreach (CarComponentPanels panel in _panelsCollection)
        {
            if(panel.type == componentsType)
            {
                SetPanels(panel, componentsType);
            }
        }
    }

    public void Reset()
    {
        foreach(BasicComponentPanel panel in _panels)
        {
            UnityEngine.Object.Destroy(panel.gameObject);
        }
        _panels.Clear();
    }

    private void SetPanels(CarComponentPanels carComponentPanels, CarCastomizedComponents componentsType)
    {
        for (int i = 0; i < carComponentPanels.basicCarComponentConfigs.Count; i++)
        {
            
            BasicComponentPanel basicComponentPanel = UnityEngine.Object.Instantiate(carComponentPanels.basicComponentPanels, _container);

            basicComponentPanel.Init(carComponentPanels.basicCarComponentConfigs[i], componentsType);

            if (i == _basicComponentIndex && carComponentPanels.basicCarComponentConfigs.Count > 1)
            {
                basicComponentPanel.SaveBoughtCarComponent();
            }

            if(IsBought(carComponentPanels, i))
            {

                basicComponentPanel.IsBought = true;
            }

            basicComponentPanel.SetComponentPanel();

            _panels.Add(basicComponentPanel);
        }
    }
    private bool IsBought(CarComponentPanels carComponentPanels, int panelIndex)
    {
        List<string> savedComponents = new List<string>();
        savedComponents = SaveManager.PlayerPrefs.LoadStringList(carComponentPanels.type.ToString());
        for(int i = 0; i < savedComponents.Count; i++)
        {
            if (savedComponents[i] == carComponentPanels.basicCarComponentConfigs[panelIndex].name)
            {
                return true;
            }
        }
        return false;
    }
}
