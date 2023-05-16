using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;
using System;

public class TemporarilyDisableHand : MonoBehaviour
{
    private XRDirectInteractor _xrDirectInteractor;
    private LayerMask _previousLayerMask;
    private Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _xrDirectInteractor = GetComponent<XRDirectInteractor>();
    }

    public void TemporarilyDisable()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(TemporarilyDisableIEnum(0.4f));
    }

    [Obsolete]
    private IEnumerator TemporarilyDisableIEnum(float time)
    {
        _previousLayerMask = _xrDirectInteractor.interactionLayerMask;
        _xrDirectInteractor.interactionLayerMask = 0;

        yield return new WaitForSeconds(time);

        if (_xrDirectInteractor.interactionLayerMask == 0)
        {
            _xrDirectInteractor.interactionLayerMask = _previousLayerMask;
        }

        _coroutine = null;
    }
}
