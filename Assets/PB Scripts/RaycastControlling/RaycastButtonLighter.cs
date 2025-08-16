// Version 0.8.5
// This script to manage the raycasting and button highlighting for the VR controllers.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RaycastButtonLighter : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public float rayDistance = 10f;

    public TextMeshProUGUI debugText;

    public GameObject playButton;
    public GameObject stopButton;
    public GameObject beatUpButton;
    public GameObject beatDownButton;
    public GameObject beatTopButton;
    public GameObject beatBottomButton;
    public GameObject synthBeatUpButton;
    public GameObject synthBeatDownButton;

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

        AddButton(playButton);
        AddButton(stopButton);
        AddButton(beatUpButton);
        AddButton(beatDownButton);
        AddButton(beatTopButton);
        AddButton(beatBottomButton);
        AddButton(synthBeatUpButton);
        AddButton(synthBeatDownButton);
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
