using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSizeManagerSimpleVersion : MonoBehaviour
{
   
    
    [Header("Scaling Parameters")]
    public float scaleFactor = 0.1f; // 每次放大的比例
    public Vector3 maxScale = new Vector3(5f, 0.1f, 5f); // 最大放大值
    public Vector3 minScale = new Vector3(0.5f, 0.1f, 0.5f); // 最小缩小值

    [Header("OVR Input")]
    public OVRInput.Button scaleButton = OVRInput.Button.One; // 默认是A键

    [Header("Player Settings")]
    public Transform playerTransform; // 玩家对象的 Transform
    public float triggerDistance = 1.5f; // 切换场景的触发距离

    [Header("Scene Settings")]
    public string sceneName = "universe"; // 目标场景名称

    void Update()
    {
        // 检测手柄按键
        if (OVRInput.GetDown(scaleButton))
        {
            ScalePortal(scaleFactor); // 放大 Portal
        }

        // 检测 Portal 与玩家的距离
        CheckPortalProximity();
    }

    private void ScalePortal(float amount)
    {
        // 获取当前缩放
        Vector3 currentScale = transform.localScale;

        // 计算新的缩放值
        Vector3 newScale = currentScale + new Vector3(amount, 0, amount);

        // 限制缩放值
        newScale = Vector3.Max(minScale, Vector3.Min(newScale, maxScale));

        // 应用缩放
        transform.localScale = newScale;

        // 移动 Portal 靠近玩家
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        transform.position += directionToPlayer * amount * 0.1f; // 控制靠近速度
    }

    private void CheckPortalProximity()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
            return;
        }

        // 计算 Portal 与玩家的距离
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 如果距离小于触发距离，切换场景
        if (distanceToPlayer <= triggerDistance)
        {
            Debug.Log("Portal is close to the player. Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName); // 切换到目标场景
        }
    }
}

    

    
    
