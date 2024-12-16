using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFlyOut : MonoBehaviour
{
    
    private Vector3 startPosition; // 动画起点
    private Vector3 targetPosition; // 动画目标点
    private float animationDuration = 3.0f; // 动画时长
    private float elapsedTime = 0.0f; // 已经过的时间
    private float maxHeight = 5.0f; // 抛物线的最大高度
    private bool initialized = false; // 检查是否初始化
    private Rigidbody rb; // Creature 的 Rigidbody

    public Transform playerTransform; // 玩家 Transform
    public float minDistanceToPlayer = 0.0f; // Creature 到玩家的最小距离
    public float maxDistanceToPlayer = 5.0f; // Creature 到玩家的最大距离

    public void Initialize(Vector3 portalCenter, float portalSize)
    {
        // 动态计算起点：Portal 表面附近的随机点
        startPosition = portalCenter + Random.insideUnitSphere * (portalSize / 2);
        startPosition.y = Mathf.Max(startPosition.y, portalCenter.y); // 确保起点不低于 Portal 中心
        transform.position = startPosition;

        // 动态计算目标点：玩家周围随机点
        Vector3 randomOffset = Random.onUnitSphere.normalized; // 随机方向
        randomOffset *= Random.Range(minDistanceToPlayer, maxDistanceToPlayer); // 随机距离
        targetPosition = playerTransform.position + randomOffset; // 目标点基于玩家位置

        // 确保目标点不低于地面（假设地面在 y = 0）
        targetPosition.y = Mathf.Max(targetPosition.y, 0.0f); // 最低高度为 1.0f，避免落入地面以下

        // 获取 Rigidbody，禁用重力以防止动画被破坏
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // 动画过程中禁用重力
        }

        elapsedTime = 0.0f;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        // 计算归一化时间 t（从 0 到 1）
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / animationDuration);

        // 水平方向 (x, z) 线性插值
        Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

        // 垂直方向 (y) 抛物线计算
        float heightFactor = 1 - Mathf.Pow((t - 0.5f) / 0.5f, 2); // 抛物线公式
        float verticalPositionY = Mathf.Lerp(startPosition.y, targetPosition.y, t) + maxHeight * heightFactor;

        // 更新 Creature 的位置
        transform.position = new Vector3(horizontalPosition.x, verticalPositionY, horizontalPosition.z);

        // 动画完成后启用重力
        if (t >= 1.0f)
        {
            if (rb != null)
            {
                rb.useGravity = true;
            }
            enabled = false; // 禁用脚本
        }
    }



}
