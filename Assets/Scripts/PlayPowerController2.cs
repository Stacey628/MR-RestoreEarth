using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPowerController2 : MonoBehaviour
{
    [Header("Ability Settings")]
    public GameObject superPowerParticle; // 粒子效果：superPower_2
    public float scaleMultiplier = 2f; // 放大倍数
    public string interactableTag = "Interactable"; // 场景中可点击的物体标签
    public float particleLifetime = 2f; // 粒子效果的存在时间
    public GameObject creatureName;

    [Header("Input Settings")]
    public OVRInput.Button abilityButton = OVRInput.Button.Two; // 默认是右手 B 按键

    [Header("Ray Settings")]
    public float rayLength = 100f; // 射线长度
    public LayerMask interactableLayer; // 可交互物体的层级

    private bool hasAbility = false; // 是否获得能力

    private void OnTriggerEnter(Collider other)
    {
        // 检查接触的对象是否是 JellyfishPrefab2
        if (other.gameObject.name == "creatureName")
        {
            // 获得能力
            hasAbility = true;
            Debug.Log("Ability unlocked: Can use right hand B button to enlarge objects!");

            // 销毁 JellyfishPrefab2
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        // 如果玩家未获得能力，不响应按键
        if (!hasAbility) return;

        // 检测右手 B 按键按下
        if (OVRInput.GetDown(abilityButton))
        {
            TryActivateAbility();
        }
    }

    private void TryActivateAbility()
    {
        // 发射射线检测点击的物体
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, interactableLayer))
        {
            GameObject targetObject = hit.collider.gameObject;

            // 检查物体是否有特定标签
            if (targetObject.CompareTag(interactableTag))
            {
                // 放大物体
                targetObject.transform.localScale *= scaleMultiplier;
                Debug.Log("Object enlarged: " + targetObject.name);

                // 在手柄位置生成粒子效果
                Vector3 handPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                Quaternion handRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

                if (superPowerParticle != null)
                {
                    GameObject particle = Instantiate(superPowerParticle, handPosition, handRotation);
                    Destroy(particle, particleLifetime); // 在指定时间后销毁粒子效果
                    Debug.Log("Particle effect spawned.");
                }
            }
        }
        else
        {
            Debug.Log("No interactable object detected within ray range.");
        }
    }
}
