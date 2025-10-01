using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class LineTextureController : MonoBehaviour
{
    [Header("Texture Tiling (전체 길이 기반)")]
    [Tooltip("라인 전체 길이를 몇 단위로 나눠 텍스처 타일링할지")]
    public float textureUnitLength = 1f;  

    [Header("세그먼트 분할 옵션 (짧은 Path 보정용)")]
    [Tooltip("세그먼트를 일정 간격으로 분할하여 UV 왜곡 최소화")]
    public bool autoSubdivide = false;
    public float maxSegmentLength = 0.5f; // 세그먼트 최대 길이

    private LineRenderer lr;

    void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (lr == null || lr.positionCount < 2) return;

        // 1️⃣ 라인의 전체 길이 계산
        float totalLength = 0f;
        for (int i = 1; i < lr.positionCount; i++)
        {
            totalLength += Vector3.Distance(lr.GetPosition(i - 1), lr.GetPosition(i));
        }

        // 2️⃣ 전체 길이에 맞춰 Texture Scale 자동 조정
        if (lr.sharedMaterial != null)
        {
            float tileAmount = totalLength / Mathf.Max(0.001f, textureUnitLength);
            lr.sharedMaterial.mainTextureScale = new Vector2(tileAmount, 1f);
        }

        // 3️⃣ 짧은 Path 세그먼트 자동 분할
        if (autoSubdivide)
        {
            SubdivideLine(totalLength);
        }
    }

    void SubdivideLine(float totalLength)
    {
        // 원본 포인트 가져오기
        Vector3[] originalPoints = new Vector3[lr.positionCount];
        lr.GetPositions(originalPoints);

        // 새 포인트 리스트
        System.Collections.Generic.List<Vector3> subdividedPoints = new System.Collections.Generic.List<Vector3>();
        subdividedPoints.Add(originalPoints[0]);

        for (int i = 1; i < originalPoints.Length; i++)
        {
            Vector3 start = originalPoints[i - 1];
            Vector3 end = originalPoints[i];
            float segLength = Vector3.Distance(start, end);

            // 필요하다면 세그먼트 잘라서 추가
            if (segLength > maxSegmentLength)
            {
                int cuts = Mathf.CeilToInt(segLength / maxSegmentLength);
                for (int j = 1; j < cuts; j++)
                {
                    float t = j / (float)cuts;
                    subdividedPoints.Add(Vector3.Lerp(start, end, t));
                }
            }
            subdividedPoints.Add(end);
        }

        // 다시 적용
        lr.positionCount = subdividedPoints.Count;
        lr.SetPositions(subdividedPoints.ToArray());
    }
}
