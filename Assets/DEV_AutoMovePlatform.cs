using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DEV_AutoMovePlatform : MonoBehaviour
{
    [Header("Platform Points")]
    [SerializeField] private Transform OriginPoint;
    [SerializeField] private Transform DestPoint;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private bool isMovingToDest = true;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // 💡 필수! (물리 충돌 영향 안 받음)
    }

    private void Start()
    {
        rb.position = OriginPoint.position;
        _ = AutoMovePlatform();
    }

    public async UniTask AutoMovePlatform()
    {
        while (true)
        {
            Vector2 start = isMovingToDest ? OriginPoint.position : DestPoint.position;
            Vector2 end = isMovingToDest ? DestPoint.position : OriginPoint.position;

            float distance = Vector2.Distance(start, end);
            float duration = distance / moveSpeed;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float easedT = t * t; // Ease In Quad

                Vector2 target = Vector2.Lerp(start, end, easedT);
                rb.MovePosition(target); // ✅ Transform이 아니라 MovePosition으로 이동

                await UniTask.WaitForFixedUpdate(); // 💡 물리 프레임에 맞춰 대기
            }

            rb.MovePosition(end);
            isMovingToDest = !isMovingToDest;
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        }
    }
}