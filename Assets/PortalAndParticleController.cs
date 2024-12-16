using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PortalAndParticleController : MonoBehaviour
{
    [Header("Portal Settings")]
    public float scaleFactor = 0.1f; // 每次放大的比例
    public Vector3 maxScale = new Vector3(5f, 0.1f, 5f); // 最大放大值
    public Vector3 minScale = new Vector3(0.5f, 0.1f, 0.5f); // 最小缩小值
    public Transform playerTransform; // 玩家对象的 Transform
    public float triggerDistance = 1.5f; // 切换场景的触发距离
    public string sceneName = "universe"; // 目标场景名称

    [Header("Particle Settings")]
    public ParticleSystem particle1; // A 按键的粒子效果
    public ParticleSystem particle2; // B 按键的粒子效果
    public ParticleSystem particle3; // X 按键的粒子效果

    [Header("Input Settings")]
    private InputAction fireParticle1Action;
    private InputAction fireParticle2Action;
    private InputAction fireParticle3Action;

    void OnEnable()
    {
        // 初始化输入动作
        InitializeInputActions();

        // 启用输入动作
        fireParticle1Action.Enable();
        fireParticle2Action.Enable();
        fireParticle3Action.Enable();
    }

    void OnDisable()
    {
        // 禁用输入动作
        fireParticle1Action.Disable();
        fireParticle2Action.Disable();
        fireParticle3Action.Disable();
    }

    void Update()
    {
        // 检测 Portal 与玩家的距离
        CheckPortalProximity();
    }

    private void InitializeInputActions()
    {
        if (Application.isEditor)
        {
            // 键盘绑定（用于编辑器测试）
            fireParticle1Action = new InputAction("FireParticle1", binding: "<Keyboard>/j"); // 模拟 A 按键
            fireParticle2Action = new InputAction("FireParticle2", binding: "<Keyboard>/k"); // 模拟 B 按键
            fireParticle3Action = new InputAction("FireParticle3", binding: "<Keyboard>/l"); // 模拟 X 按键
        }
        else
        {
            // XR 手柄绑定
            fireParticle1Action = new InputAction("FireParticle1", binding: "<XRController>{RightHand}/buttonSouth"); // A 按键
            fireParticle2Action = new InputAction("FireParticle2", binding: "<XRController>{RightHand}/buttonEast");  // B 按键
            fireParticle3Action = new InputAction("FireParticle3", binding: "<XRController>{LeftHand}/buttonSouth"); // X 按键
        }

        // 绑定按键事件
        fireParticle1Action.performed += ctx => OnButtonPressed(particle1);
        fireParticle2Action.performed += ctx => OnButtonPressed(particle2);
        fireParticle3Action.performed += ctx => OnButtonPressed(particle3);
    }

    private void OnButtonPressed(ParticleSystem particle)
    {
        // 播放粒子效果
        if (particle != null)
        {
            particle.Play();
        }
        else
        {
            Debug.LogWarning("Particle system not assigned!");
        }

        // 同时触发 Portal 放大和靠近
        ScalePortal(scaleFactor);
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
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.position += directionToPlayer * amount * 0.1f; // 控制靠近速度
        }
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
