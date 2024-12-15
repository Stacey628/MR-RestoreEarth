using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public GameObject[] creaturePrefabs;
    public PortalSizeManager portalManager;
    public float spawnInterval = 10.0f;
    public float playerProximityRange = 2.0f;
    public string objectTag = "Interactable";

    private float lastSpawnTime;
    private Transform playerTransform;
    private int currentCreatureIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnCreature();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnCreature()
    {
        Vector3 portalCenter = portalManager.portalCenter.position;
        float portalSize = portalManager.portalSize;

        GameObject creaturePrefab = creaturePrefabs[currentCreatureIndex];
        currentCreatureIndex = (currentCreatureIndex + 1) % creaturePrefabs.Length;

        GameObject newCreature = Instantiate(creaturePrefab, portalCenter, Quaternion.identity);
        if (!string.IsNullOrEmpty(objectTag))
            {
                newCreature.tag = objectTag;
                Debug.Log($"Generated object tagged as: {objectTag}");
            }
        
        CreatureFlyOut flyOutScript = newCreature.GetComponent<CreatureFlyOut>();
        flyOutScript.Initialize(portalCenter, portalSize);
        
    }

    void CheckPlayerInteraction()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");

        foreach (GameObject creature in creatures)
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, creature.transform.position);
            if (distanceToPlayer <= playerProximityRange)
            {
                if (OVRInput.GetDown(OVRInput.Button.Two))
                {
                    Debug.Log("Player clicked creature");
                    HandlePlayerInteraction(creature);
                }
            }
        }
    }

    void HandlePlayerInteraction(GameObject creature)
    {
        MonoBehaviour ability = creature.GetComponent<MonoBehaviour>();
        if (ability != null)
        {
            Debug.Log($"get {ability.GetType().Name} ability");
            if (ability is EnlargeAbility)
            {
                (ability as EnlargeAbility).Execute(gameObject, portalManager.portalSize);
            }

            Destroy(creature);
        }
    }
}
