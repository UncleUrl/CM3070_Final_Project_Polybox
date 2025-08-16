// Version 0.8.5
// Script used to confirm object raycast hit used for debugging.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaycastDebug : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public float rayLength = 10f;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
        {
            UpdateDebugText("Hit: " + hit.collider.gameObject.name);
        }
    }

    void UpdateDebugText(string message)
    {
        if (debugText != null)
            debugText.text = message;
    }
}
