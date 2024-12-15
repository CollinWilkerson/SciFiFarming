using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RackDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject plantDisplay;
    private Image[] plantImages;
    [SerializeField] private Image tankFill;

    private void Start()
    {
        plantImages = plantDisplay.GetComponentsInChildren<Image>();
        tankFill.fillAmount = 0;
    }

    public void SetFill(float fillPorportion)
    {
        tankFill.fillAmount = fillPorportion;
    }

    public void SetPlantDisplays((int type, int value, int stage)[] crops)
    {
        for (int i = 0; i < crops.Length; i++){
            if (crops[i].type != -1 && i < plantImages.Length - 1)
            {
                plantImages[i + 1].fillAmount =  crops[i].stage / (float) PlantLibrary.library[crops[i].type].harvestStage;
            }
        }
    }
}
