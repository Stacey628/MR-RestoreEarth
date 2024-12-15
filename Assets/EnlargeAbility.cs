using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeAbility : MonoBehaviour
{
    public GameObject particleEffect;
    public float scaleFactor = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Execute(GameObject target, float portalSize)
    {
        Instantiate(particleEffect, target.transform.position, Quaternion.identity);
        
        float finalScaleFactor = Mathf.Max(0.1f,scaleFactor-0.05f*portalSize);
        target.transform.localScale *= finalScaleFactor;
        Debug.Log($"Enlarged {target.name} by {finalScaleFactor}");
    }
}
