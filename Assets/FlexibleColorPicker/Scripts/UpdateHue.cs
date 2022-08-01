using UnityEngine;

public class UpdateHue : MonoBehaviour
{
    [SerializeField]
    private FlexibleColorPicker colorPicker;
    [SerializeField]
    private string property;
    [SerializeField]
    private CollapsibleColorPicker colorManager;
    [SerializeField]
    private BrickPickerManager pickerManager;

    public void UpdateColor(float value) {
        switch(property) {
            case "H":
                colorPicker.SetColor(colorPicker.bufferedColor.PickH(value * 6).color);
                break;
            case "S":
                colorPicker.SetColor(colorPicker.bufferedColor.PickS(value).color);
                break;
            case "V":
                colorPicker.SetColor(colorPicker.bufferedColor.PickV(value).color);
                break;
        }
        pickerManager.SetColor(colorPicker.GetColor());
    }
}
