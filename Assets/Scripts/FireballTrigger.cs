using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class FireballTrigger : MonoBehaviour
{
    public OVRInput.Controller controller;
    public GameObject fireballPrefab;
    public GameObject whiteGiantBall;
    public float fireballSpeed = 5f;

    private int fireballCount = 0;
    private int totalMeshes = 0;
    private List<GameObject> generatedMeshes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] meshes = GameObject.FindGameObjectsWithTag("GeneratedMesh");
        totalMeshes = meshes.Length;
        generatedMeshes.AddRange(meshes);

        if (whiteGiantBall != null)
        {
            SetWhiteGiantBallTransparency(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject != null && hitObject.CompareTag("GeneratedMesh"))
                {
                    GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                    fireball.GetComponent<Fireball>().SetTarget(hitObject.transform.position, fireballSpeed);

                    fireballCount++;

                    if (fireballCount > totalMeshes)
                    {
                        SetWhiteGiantBallTransparency(false);
                    }
                }
            }
        }
    }

    void SetWhiteGiantBallTransparency(bool isTransparent)
    {
        if (whiteGiantBall != null)
        {
            Renderer giantBallRenderer = whiteGiantBall.GetComponent<Renderer>();
            if (giantBallRenderer != null)
            {
                Color color = giantBallRenderer.material.color;
                if (isTransparent)
                {
                    color.a = 0f;
                }
                else
                {
                    color.a = 1f;
                }
                giantBallRenderer.material.color = color;
            }
        }
    }

    public class Fireball : MonoBehaviour
    {
        private Vector3 targetPosition;
        private float speed;

        public void SetTarget(Vector3 target, float FireballSpeed)
        {
            targetPosition = target;
            speed = FireballSpeed;
        }
        
        void Update()
        {
            if (targetPosition != Vector3.zero)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                if (transform.position == targetPosition)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
