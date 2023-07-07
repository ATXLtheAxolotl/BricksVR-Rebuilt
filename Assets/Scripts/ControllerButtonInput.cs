using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine;

public class ControllerButtonInput : MonoBehaviour
{
    public GameObject realtimeObject;
    public GameObject rightMenuHand;
    public GameObject leftMenuHand;

    public InputActionReference rightInputAction;
    public InputActionReference leftInputAction;

    private Session _session;
    private bool _reset;
    private bool inMenu = true;

    private bool left = false;

    // Start is called before the first frame update
    public void Start()
    {
        _session = realtimeObject.GetComponent<Session>();
        _reset = false;

        
        rightMenuHand.SetActive(false);
        leftMenuHand.SetActive(true);

        rightInputAction.action.started += RightInput;
        leftInputAction.action.started += LeftInput;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_reset) return;
        ResetMenuState();
    }

    public void EnableMenuControls()
    {
        _reset = false;
        inMenu = true;
    }

    public void DisableMenuControls()
    {
        inMenu = false;
    }

    public bool CanSwitch()
    {
        if (inMenu) return true;
        else if (!_session.isPlaying && !_session.isLoading) return true;

        return false;
    }

    private void RightInput(InputAction.CallbackContext context)
    {
        if (!CanSwitch()) return;
        if (!left) return;
        left = false;

        rightMenuHand.SetActive(true);
        leftMenuHand.SetActive(false);
    }

    private void LeftInput(InputAction.CallbackContext context)
    {
        if (!CanSwitch()) return;
        if (left) return;
        left = true;

        rightMenuHand.SetActive(false);
        leftMenuHand.SetActive(true);
    }

    private void ResetMenuState()
    {
        // _activeController = OVRInput.Controller.None;
        _reset = true;
    }
}
