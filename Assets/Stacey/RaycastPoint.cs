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
    public Color hoverColor = Color.blue;
    public Color originalColor = Color.white;

    private InputDevice leftDevice;
    private InputDevice rightDevice;

    private Image lastHoveredButton = null;

    private bool isGameplayMode = false; // Flag to determine if in gameplay mode

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
        if (!isGameplayMode)
        {
            HandleRaycastAndInteraction(leftDevice, leftLineRenderer);
            HandleRaycastAndInteraction(rightDevice, rightLineRenderer);
        }
        else
        {
            // Disable or adjust line renderers for gameplay mode
            DisableLineRenderers();
        }
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
                    ResetButtonColor();

                    if (buttonImage != null)
                    {
                        buttonImage.color = hoverColor;
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

    private void DisableLineRenderers()
    {
        if (leftLineRenderer != null)
        {
            leftLineRenderer.enabled = false;
        }

        if (rightLineRenderer != null)
        {
            rightLineRenderer.enabled = false;
        }
    }

    public void EnterGameplayMode()
    {
        isGameplayMode = true;
        Debug.Log("Entering Gameplay Mode: Disabling line renderers.");
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




