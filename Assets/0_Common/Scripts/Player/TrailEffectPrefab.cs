using UnityEngine;

public class TrailEffectPrefab : MonoBehaviour
{
    public float invisibleSpeed = 1f; // 알파 감소 속도

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer 컴포넌트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        Color currentColor = spriteRenderer.color;

        // 알파 값을 invisibleSpeed 속도로 감소
        currentColor.a -= invisibleSpeed * Time.deltaTime;

        // 알파 값을 0 이상으로 제한
        currentColor.a = Mathf.Max(0f, currentColor.a);

        spriteRenderer.color = currentColor;

        if (currentColor.a <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
