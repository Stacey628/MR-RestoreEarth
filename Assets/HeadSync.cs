using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSync : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject head;
    void Start()
    {
        head = GameObject.Find("CenterEyeAnchor");
        // Console.WriteLine(head);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = head.transform.position + new Vector3(100,0,0);
        // transform.rotation = head.transform.rotation;
        
        // transform.position = head.GetComponent<OVRManager>().headPoseRelativeOffsetTranslation + new Vector3(100,0,0);
        // transform.rotation = Quaternion.Euler(head.GetComponent<OVRManager>().headPoseRelativeOffsetRotation);

        transform.position = head.transform.position + new Vector3(10000,0,0);
        transform.rotation = head.transform.rotation;
    }
}
