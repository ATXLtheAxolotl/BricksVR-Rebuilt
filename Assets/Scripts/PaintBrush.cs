using Normal.Realtime;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    [SerializeField]
    private BrickPickerManager pickerManager;

    private void OnCollisionEnter(Collision mainCollision)
    {
        if(mainCollision.gameObject.tag != "Lego") return;
        mainCollision.gameObject.GetComponent<RealtimeTransform>().RequestOwnership();
        mainCollision.gameObject.GetComponent<BrickAttach>().Color = pickerManager.GetColor();
        Debug.Log(mainCollision.gameObject.name);
    }
}
