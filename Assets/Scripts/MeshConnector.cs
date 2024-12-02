using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshConnector : MonoBehaviour
{
    public OVRInput.Controller controller;
    public LineRenderer linePrefab;
    public  Material lineMaterial;
    public float lineWidth = 0.02f;
    public GameObject uiDialog;

    private GameObject firstSelectedMesh;
    private bool isSelecting;
    private HashSet<String> connectedPairs = new HashSet<String>();
    private int totalMeshes;
    private int connectionCount;

    // Start is called before the first frame update
    void Start()
    {
        if (linePrefab == null)
        {
            Debug.LogError("Line Prefab is not set");
            return;
        }

        if (uiDialog != null)
        {
            uiDialog.SetActive(false);
        }
        firstSelectedMesh = null;
        isSelecting = false;
        totalMeshes = GameObject.FindGameObjectsWithTag("GeneratedMesh").Length;
        connectionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject != null && clickedObject.CompareTag("GeneratedMesh"))
                {
                    if (!isSelecting)
                    {
                        firstSelectedMesh = clickedObject;
                        isSelecting = true;
                        Debug.Log("First Mesh Selected: " + clickedObject.name);
                    }
                    else
                    {
                        GameObject secondSelectedMesh = clickedObject;
                        if (secondSelectedMesh != firstSelectedMesh)
                        {
                            string pairKey = GetPairKey(firstSelectedMesh.name, secondSelectedMesh.name);
                            if (!connectedPairs.Contains(pairKey))
                            {
                                CreateConnectionLine(firstSelectedMesh, secondSelectedMesh);
                                connectedPairs.Add(pairKey);
                                connectionCount++;
                                Debug.Log($"Connected {firstSelectedMesh.name} -> {secondSelectedMesh.name}");
                            }
                        }

                        firstSelectedMesh = null;
                        isSelecting = false;
                    }
                    if (connectionCount == (totalMeshes * (totalMeshes - 1)) / 2)
                    {
                        ShowCompletionDialog();
                    }
                }
            }
        }
    }

    void CreateConnectionLine(GameObject mesh1, GameObject mesh2)
    {
        LineRenderer line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        line.material = lineMaterial;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.SetPosition(0, mesh1.transform.position);
        line.SetPosition(1, mesh2.transform.position);
        line.startColor = Color.red;
        line.endColor = Color.red;
    }
    string GetPairKey(string name1, string name2)
    {
        return name1.CompareTo(name2) < 0 ? $"{name1}-{name2}" : $"{name2}-{name1}";
    }

    void ShowCompletionDialog()
    {
        if (uiDialog != null)
        {
            uiDialog.SetActive(true);
        }

        Debug.Log("All meshes are connected, Please go to the Portal to get more superpowers");
    }
}
