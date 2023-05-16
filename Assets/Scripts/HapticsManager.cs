using System.Collections;
using UnityEngine.XR;
using UnityEngine;

public class HapticsManager : MonoBehaviour
{
    private static HapticsManager _instance;

    private bool supported;
    public bool IsSupported {
        get => supported;
    }

    private InputDevice rightInput;
    private InputDevice leftInput;

    public static HapticsManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindGameObjectWithTag("HapticsManager").GetComponent<HapticsManager>();
        }

        return _instance;
    }

    private void Start()
    {
        rightInput = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftInput = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        rightInput.TryGetHapticCapabilities(out HapticCapabilities rightCapabilities);
        leftInput.TryGetHapticCapabilities(out HapticCapabilities leftCapabilities);

        supported = rightCapabilities.supportsImpulse && leftCapabilities.supportsImpulse;
    }

    public void PlayHaptics(float frequency, float amplitude, float duration, bool rightHand, bool leftHand)
    {
        StartCoroutine(PlayHapticsIEnum(frequency, amplitude, duration, rightHand, leftHand));
    }

    // Use when you don't want to auto-disable haptics.
    public void StartHaptics(float frequency, float amplitude, bool rightHand, bool leftHand)
    {

        if (rightHand) rightInput.SendHapticImpulse(0u, amplitude, float.PositiveInfinity);
        if (leftHand) leftInput.SendHapticImpulse(0u, amplitude, float.PositiveInfinity);

        Debug.Log($"Playing @ {amplitude},{frequency}");
    }

    public void EndHaptics(bool rightHand, bool leftHand)
    {
        if (rightHand) rightInput.StopHaptics();
        if (leftHand) leftInput.StopHaptics();

        Debug.Log($"Stopping");
    }

    public IEnumerator PlayHapticsIEnum(float frequency, float amplitude, float duration, bool rightHand, bool leftHand)
    {
        if (rightHand) rightInput.SendHapticImpulse(0u, amplitude, float.PositiveInfinity);
        if (leftHand) leftInput.SendHapticImpulse(0u, amplitude, float.PositiveInfinity);

        yield return new WaitForSeconds(duration);

        if (rightHand) rightInput.SendHapticImpulse(0u, amplitude, float.PositiveInfinity);
        if (leftHand) leftInput.SendHapticImpulse(0u, amplitude, float.PositiveInfinity);
    }
}
