using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMenuSceneSliderText : MonoBehaviour
{
    public TextMeshProUGUI sliderLabel;
    public List<GameObject> menus;

    private void OnEnable()
    {
        SliderValueSet(UserSettings.GetInstance().BrickShininess);
    }
    public void SliderValueSet(float value)
    {
        int intVal = (int) value;
        switch(intVal) {
            case 0:
                sliderLabel.text = "Nature Scene";
                foreach(GameObject menu in menus) {
                    menu.SetActive(false);
                }
                menus[0].SetActive(true);
                break;
            case 1:
                sliderLabel.text = "Tables";
                foreach(GameObject menu in menus) {
                    menu.SetActive(false);
                }
                menus[1].SetActive(true);
                break;
        }
    }
}
