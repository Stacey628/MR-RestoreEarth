using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Correct namespace
using UnityEngine.InputSystem; // For Input Actions

/// <summary>
/// Manages duplication and behavior of Paramecium objects around a portal,
/// and handles shooting of SuperPower_1 particles using the Oculus Quest 3 controller.
/// </summary>
public class DuplicateParameciumSingleScene : MonoBehaviour
{
    [Header("Prefab and Portal Settings")]
    public GameObject parameciumPrefab;          // Assign the Paramecium prefab in the Inspector
    public Transform portalTransform;            // Assign the portal Transform in the Inspector

    [Header("Duplication Settings")]
    public float spawnRange = 5f;                // Range around the portal to spawn objects
    public int initialDuplicateCount = 20;       // Number of duplicates at the start
    public int maxDuplicates = 100;              // Maximum duplicates allowed in the scene
    public float duplicationInterval = 2f;       // Time interval for adding duplicates initially

    [Header("Movement Settings")]
    public float movementSpeed = 2f;             // Speed for random movement of duplicates

    [Header("Reduction Settings")]
    [Tooltip("Percentage of duplicates to remove when portal size increases (e.g., 0.8 for 80%)")]
    [Range(0f, 1f)]
    public float reductionPercentage = 0.8f;     // 80% reduction

    [Header("Super Power Shooting Settings")]
    public GameObject superPowerPrefab;          // Assign your SuperPower_1 prefab here
    public Transform shootSpawnPoint;            // The point from where the super power is spawned
    public float shootForce = 1000f;             // Force applied to the super power projectile
    public float shootCooldown = 0.5f;           // Cooldown time between shots

    private float lastPortalSize;                                 // Store the last known portal size
    private List<GameObject> parameciumObjects = new List<GameObject>(); // Track all duplicated objects
    private bool portalEnlarged = false;                         // Flag to check if the portal size has increased
    private Coroutine decreaseCoroutine;                         // Store the ongoing decrease coroutine
    private Coroutine gradualAddCoroutine;                       // Coroutine to gradually add duplicates

    // Input Action for Shooting
    [Header("Input Settings")]
    public InputActionProperty shootAction; // Assign the "A" button action in the Inspector

    private float lastShootTime = -Mathf.Infinity;               // Time when the last shot was fired

    void Start()
    {
        Debug.Log("DuplicateParameciumSingleScene started.");

        // Ensure references are assigned in the scene
        if (parameciumPrefab == null || portalTransform == null)
        {
            Debug.LogError("Missing references! Please assign the Paramecium prefab and Portal Transform in the Inspector.");
            return;
        }

        if (superPowerPrefab == null || shootSpawnPoint == null)
        {
            Debug.LogError("Missing Super Power references! Please assign the SuperPower Prefab and Shoot Spawn Point in the Inspector.");
            return;
        }

        // Initialize the portal size
        lastPortalSize = portalTransform.localScale.x;

        // Start with a larger initial set of duplicates
        DuplicateParameciumObjects(initialDuplicateCount);

        // Start gradual addition of duplicates
        gradualAddCoroutine = StartCoroutine(GraduallyAddParameciumObjects());

        // Enable the shoot action
        if (shootAction != null)
        {
            shootAction.action.Enable();
            shootAction.action.performed += ctx => OnShoot();
        }
        else
        {
            Debug.LogWarning("ShootAction is not assigned. Please assign it in the Inspector.");
        }
    }

    void OnDestroy()
    {
        // Disable the shoot action to prevent memory leaks
        if (shootAction != null)
        {
            shootAction.action.performed -= ctx => OnShoot();
            shootAction.action.Disable();
        }
    }

    void Update()
    {
        HandlePortalSizeChange();
    }

    /// <summary>
    /// Handles changes in the portal size and triggers reduction of duplicates if enlarged.
    /// </summary>
    private void HandlePortalSizeChange()
    {
        float currentPortalSize = portalTransform.localScale.x;

        if (currentPortalSize > lastPortalSize)
        {
            float portalSpeed = currentPortalSize - lastPortalSize;
            Debug.Log($"Portal size increased. Speed: {portalSpeed}");

            if (!portalEnlarged)
            {
                portalEnlarged = true;

                // Stop the gradual addition coroutine to prevent adding while reducing
                if (gradualAddCoroutine != null)
                {
                    StopCoroutine(gradualAddCoroutine);
                    gradualAddCoroutine = null;
                    Debug.Log("Stopped gradual addition due to portal enlargement.");
                }

                // Start massive reduction
                if (decreaseCoroutine != null)
                {
                    StopCoroutine(decreaseCoroutine);
                }
                decreaseCoroutine = StartCoroutine(MassiveReduceParameciumObjects(reductionPercentage));

                // Optionally, you can reset the portalEnlarged flag after reduction
                // StartCoroutine(ResetPortalEnlargedFlagAfterDelay(1f)); // e.g., after 1 second
            }
        }

        lastPortalSize = currentPortalSize;
    }

    /// <summary>
    /// Called when the shoot action is performed (A button pressed).
    /// </summary>
    private void OnShoot()
    {
        // Check for cooldown
        if (Time.time - lastShootTime >= shootCooldown)
        {
            ShootSuperPower();
            lastShootTime = Time.time;
        }
    }

    /// <summary>
    /// Shoots the SuperPower_1 particle from the shoot spawn point.
    /// </summary>
    private void ShootSuperPower()
    {
        if (superPowerPrefab != null && shootSpawnPoint != null)
        {
            // Instantiate the super power at the spawn point's position and rotation
            GameObject superPower = Instantiate(superPowerPrefab, shootSpawnPoint.position, shootSpawnPoint.rotation);

            // Add force to the super power's Rigidbody to propel it forward
            Rigidbody rb = superPower.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(shootSpawnPoint.forward * shootForce);
                Debug.Log($"Shot {superPower.name} from {shootSpawnPoint.position} with force {shootForce}.");
            }
            else
            {
                Debug.LogWarning("SuperPower prefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogWarning("SuperPowerPrefab or ShootSpawnPoint is not assigned.");
        }
    }

    /// <summary>
    /// Duplicates a specified number of Paramecium objects around the portal.
    /// </summary>
    /// <param name="count">Number of duplicates to create.</param>
    public void DuplicateParameciumObjects(int count)
    {
        Debug.Log($"Duplicating {count} objects...");

        for (int i = 0; i < count; i++)
        {
            // Generate a random position around the portal within the spawn range
            Vector3 randomPosition = portalTransform.position + new Vector3(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange)
            );

            // Instantiate a duplicate of the Paramecium prefab
            GameObject newParamecium = Instantiate(parameciumPrefab, randomPosition, Quaternion.identity);
            parameciumObjects.Add(newParamecium);
            newParamecium.name = $"Paramecium_{parameciumObjects.Count}";

            // Ensure the RandomMovement component is added
            AddRandomMovement(newParamecium);

            Debug.Log($"Spawned {newParamecium.name} at {randomPosition}");
        }
    }

    /// <summary>
    /// Adds the RandomMovement component to a Paramecium object if not already present.
    /// </summary>
    /// <param name="paramecium">The Paramecium GameObject.</param>
    private void AddRandomMovement(GameObject paramecium)
    {
        // Check if RandomMovement is already attached
        RandomMovement movement = paramecium.GetComponent<RandomMovement>();
        if (movement == null)
        {
            movement = paramecium.AddComponent<RandomMovement>();
            movement.movementSpeed = movementSpeed;
            Debug.Log($"Added RandomMovement to {paramecium.name}");
        }
    }

    /// <summary>
    /// Coroutine to gradually add Paramecium duplicates over time with tapering.
    /// </summary>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator GraduallyAddParameciumObjects()
    {
        int addedCount = 0;
        float currentInterval = duplicationInterval;

        while (parameciumObjects.Count < maxDuplicates && !portalEnlarged)
        {
            // Calculate the proportion of duplicates added
            float proportion = (float)parameciumObjects.Count / maxDuplicates;

            // Dynamically adjust the interval to increase delay as more duplicates are added
            currentInterval = Mathf.Lerp(duplicationInterval, duplicationInterval * 5f, proportion);

            // Calculate the number of duplicates to add, reducing as it approaches maxDuplicates
            int additionalDuplicates = Mathf.Clamp((maxDuplicates - parameciumObjects.Count) / 10, 1, 5);

            DuplicateParameciumObjects(additionalDuplicates);
            addedCount += additionalDuplicates;

            Debug.Log($"Gradually added {additionalDuplicates} objects. Total added: {addedCount}");

            // Check if maximum duplicates are reached
            if (parameciumObjects.Count >= maxDuplicates)
            {
                Debug.Log("Reached maximum duplicates.");
                break;
            }

            // Wait for the adjusted interval before adding more duplicates
            yield return new WaitForSeconds(currentInterval);
        }

        Debug.Log("Gradual addition coroutine ended.");
    }

    /// <summary>
    /// Coroutine to massively reduce Paramecium duplicates by a specified percentage.
    /// </summary>
    /// <param name="percentage">Percentage of duplicates to remove (e.g., 0.8 for 80%).</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator MassiveReduceParameciumObjects(float percentage)
    {
        if (percentage <= 0f || percentage >= 1f)
        {
            Debug.LogError("Reduction percentage must be between 0 and 1 (exclusive).");
            yield break;
        }

        int duplicatesToRemove = Mathf.FloorToInt(parameciumObjects.Count * percentage);
        duplicatesToRemove = Mathf.Clamp(duplicatesToRemove, 1, parameciumObjects.Count - 1); // Ensure at least one remains

        Debug.Log($"Massively reducing {duplicatesToRemove} Paramecium objects ({percentage * 100}%).");

        for (int i = 0; i < duplicatesToRemove; i++)
        {
            // Remove from the end of the list for efficiency
            int lastIndex = parameciumObjects.Count - 1;
            GameObject parameciumToRemove = parameciumObjects[lastIndex];
            parameciumObjects.RemoveAt(lastIndex);
            Destroy(parameciumToRemove);

            Debug.Log($"Massively removed {parameciumToRemove.name}. Remaining objects: {parameciumObjects.Count}");

            // Optional: Introduce a very short delay for visual effect
            yield return new WaitForSeconds(0.05f); // 50 milliseconds
        }

        Debug.Log("Massive reduction complete.");

        // Optionally, restart gradual addition if desired
        // gradualAddCoroutine = StartCoroutine(GraduallyAddParameciumObjects());

        // Reset the portalEnlarged flag to allow future reductions
        portalEnlarged = false;
    }

    /// <summary>
    /// Optional coroutine to reset the portalEnlarged flag after a delay.
    /// This allows future portal enlargements to trigger reductions again.
    /// </summary>
    /// <param name="delay">Delay in seconds before resetting the flag.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator ResetPortalEnlargedFlagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        portalEnlarged = false;
        Debug.Log("Portal enlargement flag reset.");
    }

    /// <summary>
    /// Public method to trigger duplication when a Paramecium is hit by SuperPower.
    /// </summary>
    public void OnParameciumHit()
    {
        if (!portalEnlarged)
        {
            Debug.Log("Paramecium hit by SuperPower. Triggering duplication.");
            DuplicateParameciumObjects(initialDuplicateCount);
        }
        else
        {
            Debug.Log("Portal is enlarged. Duplication is currently stopped.");
        }
    }
}

/// <summary>
/// Handles collision detection for the SuperPower_1 projectile.
/// When it collides with a Paramecium, it notifies the DuplicateParameciumSingleScene to duplicate Paramecia.
/// </summary>
public class SuperPowerProjectile : MonoBehaviour
{
    [Header("Collision Settings")]
    public string parameciumTag = "Paramecium"; // Tag assigned to Paramecium objects

    private DuplicateParameciumSingleScene duplicationManager; // Reference to the duplication manager

    void Start()
    {
        // Find the duplication manager in the scene
        duplicationManager = FindObjectOfType<DuplicateParameciumSingleScene>();
        if (duplicationManager == null)
        {
            Debug.LogError("DuplicateParameciumSingleScene not found in the scene.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(parameciumTag))
        {
            Debug.Log($"SuperPower collided with {collision.gameObject.name}. Triggering duplication.");

            // Notify the duplication manager to duplicate Paramecia
            if (duplicationManager != null)
            {
                duplicationManager.OnParameciumHit();
            }

            // Destroy the SuperPower projectile after collision
            Destroy(gameObject);
        }
    }
}

/// <summary>
/// Adds random movement to a GameObject.
/// </summary>
public class RandomMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 2f; // Speed of random movement

    private Vector3 direction;         // Current movement direction

    void Start()
    {
        // Assign a random initial direction
        direction = GetRandomDirection();
    }

    void Update()
    {
        // Move in the current direction
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        // Randomly change direction occasionally
        if (Random.Range(0, 1000) < 5) // Approximately 0.5% chance each frame
        {
            direction = GetRandomDirection();
        }

        // Optional: Keep the object within spawn range (e.g., to prevent drifting too far)
        // Vector3 clampedPosition = new Vector3(
        //     Mathf.Clamp(transform.position.x, portalTransform.position.x - spawnRange, portalTransform.position.x + spawnRange),
        //     Mathf.Clamp(transform.position.y, portalTransform.position.y - spawnRange, portalTransform.position.y + spawnRange),
        //     Mathf.Clamp(transform.position.z, portalTransform.position.z - spawnRange, portalTransform.position.z + spawnRange)
        // );
        // transform.position = clampedPosition;
    }

    /// <summary>
    /// Generates a random normalized direction vector.
    /// </summary>
    /// <returns>A normalized Vector3 representing direction.</returns>
    private Vector3 GetRandomDirection()
    {
        return new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }
}
