using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class RaycastPoint : MonoBehaviour
{
   
    public LineRenderer leftLineRenderer;
    public LineRenderer rightLineRenderer;
    public float maxLength = 10.0f;

    private InputDevice leftDevice;
    private InputDevice rightDevice;

    void Start()
    {
        InitializeDevice(XRNode.LeftHand, ref leftDevice);
        InitializeDevice(XRNode.RightHand, ref rightDevice);
    }

    private void InitializeDevice(XRNode node, ref InputDevice device)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(node, devices);

        if (devices.Count > 0)
        {
            device = devices[0];
        }
    }

    void Update()
    {
        HandleRaycastAndInteraction(leftDevice, leftLineRenderer);
        HandleRaycastAndInteraction(rightDevice, rightLineRenderer);
    }

    private void HandleRaycastAndInteraction(InputDevice device, LineRenderer lineRenderer)
    {
        Vector3 startPosition = lineRenderer.transform.position;
        Vector3 direction = lineRenderer.transform.forward;

        lineRenderer.SetPosition(0, startPosition);

        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, maxLength))
        {
            lineRenderer.SetPosition(1, hit.point);

            Button button = hit.transform.GetComponent<Button>();
            if (button != null)
            {
                Debug.Log("Button found on: " + hit.transform.name);
                lineRenderer.material.color = Color.blue;

                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed)
                {
                    Debug.Log("Button clicked: " + hit.transform.name);
                    button.onClick.Invoke();
                }
            }
            else
            {
                Debug.LogWarning("No Button component found on: " + hit.transform.name);
                lineRenderer.material.color = Color.red;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPosition + direction * maxLength);
            lineRenderer.material.color = Color.red;
        }
    }
}


