﻿using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickHover : MonoBehaviour
{
    private List<GameObject> _hoveredObjects;
    private GameObject _hovered;
    private BrickAttach _hoveredAttach;
    private Color32 _hoverColor;

    // Temporary objects to reduce GC
    private GameObject _tempHoveredObject;
    private BrickAttach _tempAttach;

    private bool _holdingSomething;

    public LayerMask interactionLayerMask;

    public bool left;

    private void Start()
    {
        _hoveredObjects = new List<GameObject>();
        _hoverColor = new Color32(213, 226, 255, 255);
    }

    public GameObject HoveredBrick()
    {
        return _holdingSomething ? null : _hovered;
    }

    private void Update()
    {
        if (_hovered != null && _holdingSomething)
        {
            _hoveredAttach.SetHovered(false, left);
            _hoveredAttach = null;
            _hovered = null;
            return;
        }

        // Protects against destroyed objects
        if (_holdingSomething && _hovered == null)
        {
            _hovered = null;
            _holdingSomething = false;
        }

        _hoveredObjects.RemoveAll(o => o == null || Vector3.Distance(o.transform.position, transform.position) > 0.65f);

        if (!_hoveredObjects.Any()) return;
        if (_holdingSomething) return;

        _tempHoveredObject = _hoveredObjects.Count == 1 ? _hoveredObjects[0] : _hoveredObjects.OrderBy(o => (o.transform.position - transform.position).sqrMagnitude).ToList()[0];
        if (_tempHoveredObject == null || _tempHoveredObject == _hovered) return;

        _tempAttach = _tempHoveredObject.GetComponent<BrickAttach>();

        _tempAttach.SetHovered(true, left);

        if (_hovered != null)
        {
            _hoveredAttach.SetHovered(false, left);
        }

        _hovered = _tempHoveredObject;
        _hoveredAttach = _tempAttach;
    }

    private void OnEnable()
    {
        GetComponent<XRBaseInteractor>().onHoverEntered.AddListener(HandHoverEnter);
        GetComponent<XRBaseInteractor>().onHoverExited.AddListener(HandHoverExit);
        GetComponent<XRBaseInteractor>().onSelectEntered.AddListener(HandSelectEnter);
        GetComponent<XRBaseInteractor>().onSelectExited.AddListener(HandSelectExit);
    }

    private void OnDisable()
    {
        GetComponent<XRBaseInteractor>().onHoverEntered.RemoveListener(HandHoverEnter);
        GetComponent<XRBaseInteractor>().onHoverExited.RemoveListener(HandHoverExit);
        GetComponent<XRBaseInteractor>().onSelectEntered.RemoveListener(HandSelectEnter);
        GetComponent<XRBaseInteractor>().onSelectExited.RemoveListener(HandSelectExit);
    }

    private void HandSelectEnter(XRBaseInteractable interactable)
    {
        _holdingSomething = true;
        HandHoverEnter(interactable);

        BrickAttach attach = interactable.GetComponent<BrickAttach>();
        if (attach)
            attach.SetHeld(true);
    }

    private void HandSelectExit(XRBaseInteractable interactable)
    {
        _holdingSomething = false;

        BrickAttach attach = interactable.GetComponent<BrickAttach>();
        if (attach)
            attach.SetHeld(false);
    }

    private void HandHoverEnter(XRBaseInteractable interactable)
    {
        if (!_hoveredObjects.Contains(interactable.gameObject) && IsObjectInInteractionLayerMask(interactable.gameObject))
        {
            _hoveredObjects.Add(interactable.gameObject);
        }
    }

    private void HandHoverExit(XRBaseInteractable interactable)
    {
        if (interactable == null) return;

        _hoveredObjects.Remove(interactable.gameObject);
        if (_hovered == interactable.gameObject)
        {
            _hovered.GetComponentInChildren<BrickAttach>().SetHovered(false, left);
            _hovered = null;
        }
    }

    public void AddToHoverList(GameObject obj)
    {
        if (!_hoveredObjects.Contains(obj) && obj.GetComponent<BrickAttach>() && IsObjectInInteractionLayerMask(obj))
        {
            _hoveredObjects.Add(obj);
        }
    }

    public void RemoveFromHoverList(GameObject obj)
    {
        _hoveredObjects.Remove(obj);
        if (_hovered == obj)
        {
            _hovered.GetComponentInChildren<BrickAttach>().SetHovered(false, left);
            _hovered = null;
        }
    }

    private bool IsObjectInInteractionLayerMask(GameObject o)
    {
        return interactionLayerMask == (interactionLayerMask | (1 << o.gameObject.layer));
    }
}
