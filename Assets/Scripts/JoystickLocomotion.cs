﻿using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine;

public class JoystickLocomotion : MonoBehaviour
{
    public GameObject head;
    public KeepPlayerOutOfWalls keepPlayerOutOfWalls;

    private Vector2 _currentLeftJoystickDirection;
    private Vector2 _currentRightJoystickDirection;

    private InputDevice rightInput;
    private InputDevice leftInput;

    private float minY = 0.1f;

    public float lateralMovementMultiplier;
    public float verticalMovementMultiplier;

    public float joystickDeadzone = 0.15f;

    private float _movementSpeedSetting = 1f; // In addition to the normal multiplier, we have another multiplier from the user settings

    private Transform _headTransform;

    public int lateralMovementIndex; // Increments when we move. Use to detect when the player moves. Kind of lame and hacky, should probably make an event.
    public int verticalMovementIndex;

    public float playerScaleMultiplier = 1f;

    private void Start()
    {
        // TODO: Work on VR Simulator script some more before turning this off
        //if (Application.isEditor)
        //{
        //    enabled = false;
        //}

        rightInput = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftInput = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        _headTransform = head.transform;
    }

    public void SetMovementSpeedSetting(float value)
    {
        _movementSpeedSetting = value;
    }

    private void Update()
    {
        rightInput.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 right);
        leftInput.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 left);


        _currentLeftJoystickDirection = right;
        _currentRightJoystickDirection = left;

        if (_currentLeftJoystickDirection.magnitude > joystickDeadzone || Mathf.Abs(_currentRightJoystickDirection.y) > joystickDeadzone) // We only care about the y axis for the right stick
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector3 movement =
            (head.transform.forward * _currentLeftJoystickDirection.y + head.transform.right * _currentLeftJoystickDirection.x)
            * (Time.deltaTime * lateralMovementMultiplier * _movementSpeedSetting * playerScaleMultiplier);

        movement.y = (Vector3.up * (_currentRightJoystickDirection.y * Time.deltaTime)).y * verticalMovementMultiplier * _movementSpeedSetting * playerScaleMultiplier;

        if ((head.transform.position + movement).y < minY)
        {
            movement.y = Mathf.Max(0, movement.y); // Erase negative y movement if we're at the floor, but still allow the player to move up.
        }

        Vector3 oldPosition = transform.position;
        transform.position += movement;

        //if (!keepPlayerOutOfWalls.IsHeadPositionAllowed(_headTransform.position))
        //    transform.position = oldPosition;

        if (Mathf.Abs(_currentLeftJoystickDirection.x) > 0.01f)
        {
            lateralMovementIndex += 1;
            lateralMovementIndex %= 50;
        }

        if (Mathf.Abs(_currentRightJoystickDirection.y) > 0.01f)
        {
            verticalMovementIndex += 1;
            verticalMovementIndex %= 50;
        }
    }
}
