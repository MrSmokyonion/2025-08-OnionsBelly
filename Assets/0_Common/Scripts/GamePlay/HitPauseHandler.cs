using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitPauseHandler : MonoBehaviour
{
    [SerializeField] private float pauseDuration = 0.1f;
    [SerializeField] private float timeScaleDuringPause = 0f;

    private bool isPausing = false;

    public async UniTask TriggerHitPause()
    {
        if (isPausing) return;

        isPausing = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = timeScaleDuringPause;

        // Wait in real time (not affected by timeScale)
        await UniTask.Delay(System.TimeSpan.FromSeconds(pauseDuration), ignoreTimeScale: true);

        Time.timeScale = originalTimeScale;
        isPausing = false;
    }
}