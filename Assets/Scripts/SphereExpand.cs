using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SphereExpand : MonoBehaviour
{
    public Transform player;
    public XRNode controllerNode = XRNode.RightHand;
    public float interactionRange = 1f;
    public float scaleMultiplier = 20f;
    private bool isScaled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactionRange && IsButtonPressed() && !isScaled)
        {
            ScaleBall();
        }
    }

    void ScaleBall()
    {
        transform.localScale *= scaleMultiplier;
        isScaled = true;
    }

    private bool IsButtonPressed()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed))
        {
            return isPressed;
        }
        return false;
    }
}
