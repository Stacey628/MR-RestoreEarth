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
    public Color hoverColor = Color.blue;  // Color to change to on hover
    public Color originalColor = Color.white;  // Original color to reset to

    private InputDevice leftDevice;
    private InputDevice rightDevice;

    private Image lastHoveredButton = null;  // Store reference to last hovered button's image

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
                Image buttonImage = button.GetComponent<Image>();

                if (buttonImage != lastHoveredButton)
                {
                    ResetButtonColor();  // Reset previous button color

                    if (buttonImage != null)
                    {
                        buttonImage.color = hoverColor;  // Change color on hover
                        lastHoveredButton = buttonImage;
                    }
                }

                lineRenderer.material.color = Color.green;

                if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed)
                {
                    Debug.Log("Clicked on Button: " + hit.transform.name);
                    button.onClick.Invoke();
                }
            }
            else
            {
                lineRenderer.material.color = Color.red;
                ResetButtonColor();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPosition + direction * maxLength);
            lineRenderer.material.color = Color.red;
            ResetButtonColor();
        }
    }

    private void ResetButtonColor()
    {
        if (lastHoveredButton != null)
        {
            lastHoveredButton.color = originalColor;
            lastHoveredButton = null;
        }
    }
}




