// Version 0.8.5
// This script maintains the Polybox to be locked facing the camera/user.  
// Much of this script is derived from Unity User group discussions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class LockedFacingGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Quaternion initialRelativeRotation;
    private Transform mainCamera;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        mainCamera = Camera.main.transform;
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // Store initial relative rotation to camera
        initialRelativeRotation = Quaternion.Inverse(Quaternion.LookRotation(mainCamera.forward)) 
                               * transform.rotation;
        
        // Disable default rotation handling
        grabInteractable.trackRotation = false;
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Maintain original facing relative to camera
            transform.rotation = Quaternion.LookRotation(mainCamera.forward) 
                               * initialRelativeRotation;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        grabInteractable.trackRotation = true;
    }
}