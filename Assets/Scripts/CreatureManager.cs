using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public GameObject[] creaturePrefabs;
    public PortalSizeManager portalManager;
    public float spawnInterval = 10.0f;
    public float playerProximityRange = 2.0f;

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
        GameObject creaturePrefab = creaturePrefabs[currentCreatureIndex];
        Vector3 spawnPosition = portalManager.CalculateCreatureStartPosition();
        GameObject newCreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        
        CreatureFlyOut flyOutScript = newCreature.GetComponent<CreatureFlyOut>();
        flyOutScript.Initialize(portalManager.portalCenter.position, portalManager.portalSize);
        currentCreatureIndex = (currentCreatureIndex + 1) % creaturePrefabs.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creature"))
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, other.transform.position);
            if (distanceToPlayer <= playerProximityRange)
            {
                Debug.Log($"Player gets {other.name} superpower!");
                Destroy(other.gameObject);
            }
        }
    }
}
