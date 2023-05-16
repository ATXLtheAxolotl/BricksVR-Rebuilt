using UnityEngine.XR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerButtonInput : MonoBehaviour
{
    public GameObject realtimeObject;
    public GameObject leftMenuHand;
    public GameObject rightMenuHand;

    private InputDevice _activeController;

    private Session _session;
    private bool _reset;
    private bool inMenu = true;

    private InputDevice rightHand;
    private InputDevice leftHand;

    // Start is called before the first frame update
    public void Start()
    {
        _session = realtimeObject.GetComponent<Session>();
        _reset = false;

        leftMenuHand.SetActive(true);
        rightMenuHand.SetActive(false);
        _activeController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
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

    private void MenuLogic()
    {
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

        // Switch laser if we press the opposite trigger
        rightHand.IsPressed(InputHelpers.Button.TriggerButton, out bool primaryPressed, 0.5f);
        leftHand.IsPressed(InputHelpers.Button.TriggerButton, out bool secondaryPressed, 0.5f);

        if (_activeController == leftHand && secondaryPressed || Input.GetMouseButtonDown(1))
        {
            _activeController = rightHand;

            rightMenuHand.SetActive(true);
            leftMenuHand.SetActive(false);
        }
        else if (_activeController == rightHand && (primaryPressed || Input.GetMouseButtonDown(1)))
        {
            _activeController = leftHand;

            rightMenuHand.SetActive(false);
            leftMenuHand.SetActive(true);
        }
    }

    private void ResetMenuState()
    {
        // _activeController = OVRInput.Controller.None;
        _reset = true;
    }
}
