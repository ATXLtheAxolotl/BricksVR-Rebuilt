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
        if ((!_session.isPlaying && !_session.isLoading) || inMenu)
        {
            MenuLogic();
        }
        else if (!_reset)
        {
            ResetMenuState();
        }
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

    private void RightInput(InputAction.CallbackContext context)
    {
        if (!left) return;
        left = false;

        rightMenuHand.SetActive(true);
        leftMenuHand.SetActive(false);
    }

    private void LeftInput(InputAction.CallbackContext context)
    {
        if (left) return;
        left = true;

        rightMenuHand.SetActive(false);
        leftMenuHand.SetActive(true);
    }

    private void MenuLogic()
    {
        // Switch laser if we press the opposite trigger.
        //     if (_activeController == OVRInput.Controller.None)
        //     {
        //         OVRInput.Controller controller = OVRInput.GetActiveController();
        //         if (controller == OVRInput.Controller.Touch || controller == OVRInput.Controller.RTouch)
        //         {
        //             _activeController = OVRInput.Controller.RTouch;
        //             rightMenuHand.SetActive(true);
        //             leftMenuHand.SetActive(false);
        //         }
        //         else if (controller == OVRInput.Controller.LTouch)
        //         {
        //             _activeController = OVRInput.Controller.LTouch;
        //             leftMenuHand.SetActive(true);
        //             rightMenuHand.SetActive(false);
        //         }
        //         else
        //         {
        //             _activeController = OVRInput.Controller.RTouch;
        //             leftMenuHand.SetActive(false);
        //             rightMenuHand.SetActive(true);
        //         }
        //     }
    }

    private void ResetMenuState()
    {
        // _activeController = OVRInput.Controller.None;
        _reset = true;
    }
}
