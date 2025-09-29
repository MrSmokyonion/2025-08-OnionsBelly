using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraEffectHandler : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;

    // 피격 시 호출
    public void Shake()
    {
        // 방향 벡터, 진동 강도
        impulseSource.GenerateImpulse(Vector3.one * 0.5f);
    }
}
