using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine;

public class SmoothTurn : MonoBehaviour
{
    public float turnSpeed = 1f;

    // Don't turn if the joystick is in the deadzone. Prevents drift from bad controllers and prevents accidental
    // rotation when pushing the joystick up/down
    public float deadZone = 0.15f;

    public float debugRotate = 0f;

    private XROrigin rig;
    private InputDevice inputDevice;

    private void Start()
    {
        rig = GetComponent<XROrigin>();
        inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    private void Update()
    {
        inputDevice.TryGetFeatureValue(new InputFeatureUsage<Vector2>("Primary2DAxisTouch"), out Vector2 value);
        float rotationAmount = value.x;

        if (Application.isEditor) rotationAmount = debugRotate;
        if (Mathf.Abs(rotationAmount) > deadZone)
        {
            Rotate(rotationAmount);
        }
    }

    private void Rotate(float amount)
    {
        rig.RotateAroundCameraUsingOriginUp(amount * Time.deltaTime * turnSpeed);
    }
}
