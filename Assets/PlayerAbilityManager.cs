using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    public PortalSizeManager portalSizeManager;
    
    private MonoBehaviour currentAbility;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5.0f))
            {
                GameObject creature = hit.collider.gameObject;

                currentAbility = creature.GetComponent<MonoBehaviour>();
                if (currentAbility != null)
                {
                    Debug.Log($"Player get ability {currentAbility.GetType().Name}");
                }
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two) && currentAbility != null)
        {
            if (currentAbility is EnlargeAbility enlargeAbility)
            {
                enlargeAbility.Execute(gameObject, portalSizeManager.portalSize);
            }
            
        }
    }
}
