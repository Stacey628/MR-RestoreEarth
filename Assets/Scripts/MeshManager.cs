using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    public GameObject roomRoot;
    public Vector3 offset = new Vector3(0, 2, 0);
    public float scaleFactor = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        if (roomRoot == null)
        {
            Debug.LogError("Please drag Father Mesh in Room Root");
            return;
        }

        foreach (Transform child in roomRoot.transform)
        {
            if (child.GetComponent<MeshFilter>() != null)
            {
                DuplicateAndTransformMesh(child.gameObject);
            }
        }
    }

    void DuplicateAndTransformMesh(GameObject Original)
    {
        GameObject duplicate = Instantiate(Original, Original.transform.position, Original.transform.rotation);
        duplicate.name = Original.name + "_Copy";

        duplicate.transform.localScale = Original.transform.localScale * scaleFactor;
        duplicate.transform.position = Original.transform.position + offset;

        duplicate.tag = "GeneratedMesh";
        
        duplicate.GetComponent<Renderer>().material.color = Color.red;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
