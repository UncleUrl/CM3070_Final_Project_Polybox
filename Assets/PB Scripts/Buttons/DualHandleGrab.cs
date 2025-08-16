// Version 0.8.5
// This script allows either hand to grab the Polybox in VR space.
// the object is grabbed from either side rather than the center, 
// to mimic more realistic handling.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class DualHandleGrab : MonoBehaviour
{
    public Transform LeftAttachPoint;
    public Transform RightAttachPoint;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject.transform;

        if (interactor.name == "LeftHand")
            grabInteractable.attachTransform = LeftAttachPoint;
        else if (interactor.name == "RightHand")
            grabInteractable.attachTransform = RightAttachPoint;

        grabInteractable.trackRotation = false; // Disable rotation tracking
    }

    // This code may be required on certain controllers per Unity user groups.
    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Maintain the object's rotation relative to the initial rotation (locked)
            transform.rotation = Quaternion.identity;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        grabInteractable.trackRotation = true; // Re-enable rotation tracking
        grabInteractable.attachTransform = null;
    }
}
