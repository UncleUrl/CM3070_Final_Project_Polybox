// Version 0.8.5
// This script allows for user audible feedback when raycasting on buttons.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RaycastSoundLighter : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public float rayDistance = 10f;

    public TextMeshProUGUI debugText;

    public GameObject kick1;
    public GameObject kick2;
    public GameObject snare1;
    public GameObject snare2;
    public GameObject hiHat1;
    public GameObject hiHat2;
    public GameObject clap;
    public GameObject shaker;

    private Dictionary<GameObject, ButtonColourChanger> buttonMap;

    private InputAction leftTrigger;
    private InputAction rightTrigger;

    void Awake()
    {
        leftTrigger = new InputAction(binding: "<XRController>{LeftHand}/trigger");
        rightTrigger = new InputAction(binding: "<XRController>{RightHand}/trigger");
        leftTrigger.Enable();
        rightTrigger.Enable();

        buttonMap = new Dictionary<GameObject, ButtonColourChanger>();

        AddButton(kick1);
        AddButton(kick2);
        AddButton(snare1);
        AddButton(snare2);
        AddButton(hiHat1);
        AddButton(hiHat2);
        AddButton(clap);
        AddButton(shaker);
    }

    void AddButton(GameObject button)
    {
        if (button != null)
        {
            var changer = button.GetComponent<ButtonColourChanger>();
            if (changer != null)
            {
                buttonMap.Add(button, changer);
            }
        }
    }

    void Update()
    {
        HandleRaycast(leftController, leftTrigger);
        HandleRaycast(rightController, rightTrigger);
    }

    void HandleRaycast(Transform controller, InputAction trigger)
    {
        if (controller == null || trigger == null) return;

        Ray ray = new Ray(controller.position, controller.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Optional tag filter to ensure only "Sound" tagged buttons respond
            if (!hitObject.CompareTag("Sound")) return;

            UpdateDebugText("Ray hitting: " + hitObject.name);

            if (trigger.WasPerformedThisFrame() && buttonMap.ContainsKey(hitObject))
            {
                buttonMap[hitObject].ChangeButtonColourAndRevert();
            }
        }
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
