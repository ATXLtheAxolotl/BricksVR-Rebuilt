using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using System.Linq;

// [RequireComponent(typeof(Rigidbody))]
public class QuickInteractor : MonoBehaviour
{
    public bool leftHand;
    public LayerMask interactionMask;

    private Dictionary<GameObject, QuickInteractable> _hoveredObjects;

    private GameObject _tempHoveredObject;
    private QuickInteractable _tempHoveredInteractable;
    private BrickHover _brickHover;

    private InputDevice rightInput;
    private InputDevice leftInput;

    private bool _debugGrabEnabled;

    private bool _interacting;

    void Awake()
    {
        _hoveredObjects = new Dictionary<GameObject, QuickInteractable>();
        _brickHover = GetComponent<BrickHover>();
        _debugGrabEnabled = leftHand && Application.isEditor;

        rightInput = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftInput = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    private void OnTriggerEnter(Collider c)
    {
        GameObject obj = c.gameObject;

        if (Contains(interactionMask, obj.layer) )
        {
            QuickInteractable interactable = obj.GetComponentInParent<QuickInteractable>();
            if (interactable != null && !_hoveredObjects.ContainsKey(interactable.gameObject))
            {
                _hoveredObjects[interactable.gameObject] = interactable;
                _brickHover.AddToHoverList(interactable.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        QuickInteractable interactable = c.gameObject.GetComponentInParent<QuickInteractable>();
        if (interactable == null) return;

        _hoveredObjects.Remove(interactable.gameObject);
        _brickHover.RemoveFromHoverList(interactable.gameObject);
    }
    void Update()
    {
        if (_interacting)
        {
            if ((!_debugGrabEnabled && !TriggerPressed()) || (_debugGrabEnabled && !Input.GetMouseButton(0)))
            {
                _interacting = false;
            }
        }
        else
        {
            if (_hoveredObjects.Count == 0) return;
            if (!TriggerPressed() && !(_debugGrabEnabled && Input.GetMouseButton(0))) return;

            CleanHoveredList();
            if (_hoveredObjects.Count == 0) return;

            _tempHoveredInteractable = _hoveredObjects.Count == 1
                ? _hoveredObjects.First().Value
                : _hoveredObjects.OrderBy(o => (o.Key.transform.position - transform.position).sqrMagnitude).First().Value;

            if (_tempHoveredInteractable == null) return;
            if (_tempHoveredInteractable.gameObject != _brickHover.HoveredBrick() && _tempHoveredInteractable.GetComponent<BrickPickerBrick>() == null) return;

            _tempHoveredInteractable.Interact(this);
            _interacting = true;
        }
    }

    private void CleanHoveredList()
    {
        _hoveredObjects
            .Keys
            .Where(k => k == null || !k.activeInHierarchy)
            .ToList()
            .ForEach(k => _hoveredObjects.Remove(k));
    }

    public Dictionary<GameObject, QuickInteractable> HoveredObjects()
    {
        return _hoveredObjects;
    }

    private static bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private bool TriggerPressed()
    {
        if (leftHand)
        {
            leftInput.IsPressed(InputHelpers.Button.TriggerButton, out bool pressed);
            return pressed;
        }
        else
        {
            leftInput.IsPressed(InputHelpers.Button.TriggerButton, out bool pressed);
            return pressed;
        }
    }
}
