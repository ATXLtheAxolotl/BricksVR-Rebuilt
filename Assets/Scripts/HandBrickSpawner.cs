using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Linq;
using UnityEngine;
using System;

public class HandBrickSpawner : MonoBehaviour
{
    public bool leftHand;
    public GameObject spawnLocation;
    public AudioClip sound;
    public SessionManager SessionManager;

    private BrickHover _brickHover;

    private Dictionary<InputHelpers.Button, ButtonDownInfo> _infoForButtons;
    private InputHelpers.Button[] _buttons;

    private XRDirectInteractor _interactor;
    private QuickInteractor _quickInteractor;

    private HapticsManager _hapticsManager;

    public Session _session;

    public int spawnerSetIndex;

    // Start is called before the first frame update
    private void Start()
    {
        _buttons = new InputHelpers.Button[2] { InputHelpers.Button.SecondaryButton, InputHelpers.Button.PrimaryButton };

        _infoForButtons = new Dictionary<InputHelpers.Button, ButtonDownInfo>();

        foreach (InputHelpers.Button button in _buttons)
        {
            _infoForButtons[button] = new ButtonDownInfo();
        }

        _brickHover = GetComponent<BrickHover>();
        _interactor = GetComponent<XRDirectInteractor>();
        _quickInteractor = GetComponent<QuickInteractor>();
        _hapticsManager = HapticsManager.GetInstance();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_session.isPlaying) return;

        foreach (InputHelpers.Button button in _buttons)
        {
            CheckButtonDown(button);
            HandleButtonHold(button);
        }
    }

    private void CheckButtonDown(InputHelpers.Button button)
    {
        UnityEngine.XR.InputDevice rightInput = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        UnityEngine.XR.InputDevice leftInput = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        rightInput.IsPressed(button, out bool right);
        leftInput.IsPressed(button, out bool left);


        ButtonDownInfo info = _infoForButtons[button];
        bool down = right || left || Mouse.current.rightButton.isPressed;
        if (down != info.ButtonDown)
        {
            if (!down && info.FramesDownFor > 0 && info.FramesDownFor < 30) HandleButtonPress(button);
            if (!down) info.ButtonHoldCallbackCalled = false;
            info.ButtonDown = down;

            _infoForButtons[button] = info;
        }
    }

    private void HandleButtonHold(InputHelpers.Button button)
    {
        if (_interactor.selectTarget) return;
        if (SessionManager.InGameMenuUp()) return;

        ButtonDownInfo info = _infoForButtons[button];
        if (info.ButtonHoldCallbackCalled) return;
        if (info.FramesDownFor < 30) return;

        GameObject hoveredBrick = HoveredBrick();
        if (hoveredBrick == null) return;

        BrickAttach attach = hoveredBrick.GetComponent<BrickAttach>();
        if (attach)
        {
            info.SavedPrefabName = attach.normalPrefabName;
            info.SavedColor =
                ColorInt.ColorToInt(attach.Color);
        }
        else // BrickPickerBrick
        {
            BrickPickerBrick brickPickerBrick = hoveredBrick.GetComponent<BrickPickerBrick>();
            info.SavedPrefabName = brickPickerBrick.PrefabName();
            info.SavedColor = ColorInt.ColorToInt(brickPickerBrick.Color());
        }

        info.ButtonHoldCallbackCalled = true;

        _hapticsManager.PlayHaptics(0.5f, 0.5f, 0.1f, !leftHand, leftHand);
        spawnerSetIndex += 1;

        _infoForButtons[button] = info;
    }

    private void HandleButtonPress(InputHelpers.Button button)
    {
        if (_interactor.selectTarget) return;
        if (SessionManager.InGameMenuUp()) return;

        ButtonDownInfo info = _infoForButtons[button];

        if (String.IsNullOrEmpty(info.SavedPrefabName)) return;

        GameObject brick = GameObject.Instantiate(
            Resources.Load<GameObject>(info.SavedPrefabName),
            spawnLocation.transform.position,
            spawnLocation.transform.rotation
        );

        BrickData.Brick brickData = BrickData.BrickByPrefabName(info.SavedPrefabName);

        brick.transform.Rotate(brickData.Rotation, Space.Self);
        brick.transform.localPosition += brickData.HandSpawnerPositionOffset;

        BrickAttach attach = brick.GetComponent<BrickAttach>();
        attach.Color = ColorInt.IntToColor32(info.SavedColor);
        brick.GetComponent<BrickUuid>().uuid = BrickId.FetchNewBrickID();
        brick.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Either a real brick, or a BrickPickerBrick object
    private GameObject HoveredBrick()
    {
        GameObject hoveredBrick = _brickHover.HoveredBrick();
        if (hoveredBrick != null) return hoveredBrick;

        Dictionary<GameObject, QuickInteractable> hoveredObjects = _quickInteractor.HoveredObjects();
        return hoveredObjects.Keys.FirstOrDefault(obj => obj != null && obj.GetComponent<BrickPickerBrick>() != null);
    }

    private struct ButtonDownInfo
    {
        public bool ButtonDown
        {
            get => _buttonDownAt != 0;
            set => _buttonDownAt = value ? Time.frameCount : 0;
        }
        public int FramesDownFor => ButtonDown ? Time.frameCount - _buttonDownAt : 0;
        private int _buttonDownAt;
        public string SavedPrefabName;
        public int SavedColor;
        public bool ButtonHoldCallbackCalled;
    }
}
