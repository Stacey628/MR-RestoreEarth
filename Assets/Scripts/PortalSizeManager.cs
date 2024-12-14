using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PortalSizeManager : MonoBehaviour
{
    public float portalSize = 1.0f;
    public float maxPortalSize = 200.0f;
    public float expansionRate = 5.0f;
    public float contractionRate = 2.0f;
    public float cooldownTime = 10.0f;

    private float lastAbilityUseTime;
    private int comboCount = 0;

    public GameObject portalObject;
    public Transform portalCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Portal shink
        if (Time.time - lastAbilityUseTime >=cooldownTime && portalSize > 1.0f)
        {
            portalSize = Mathf.Max(portalSize - contractionRate * Time.deltaTime, 1.0f);
            UpdatePortalVisuals();
        }
    }

    public void ExpandPortal()
    {
        comboCount++;
        portalSize += expansionRate + comboCount;
        lastAbilityUseTime = Time.time;

        UpdatePortalVisuals();
    }

    private void UpdatePortalVisuals()
    {
        portalObject.transform.localScale = UnityEngine.Vector3.one * portalSize;
    }

    public UnityEngine.Vector3 CalculateCreatureStartPosition()
    {
        return portalCenter.position + Random.insideUnitSphere * (portalSize / 2);
    }
}
