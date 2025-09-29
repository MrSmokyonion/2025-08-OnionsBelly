using Cysharp.Threading.Tasks;
using UnityEngine;

public class CurtainTransitionView : MonoBehaviour, ITransitionableUI
{
    [Header("커튼 오브젝트 참조")]
    [SerializeField] private RectTransform curtainUI;
    
    [Header("트랜지션 속성값")]
    [SerializeField] private float topPositionY;
    [SerializeField] private float centerPositionY;
    [SerializeField] private float bottomPositionY;
    [SerializeField] private float transitionDuration;
    

    public async void TransitionEnter()
    {
        Debug.Log("[TRANSITION] 커튼 트랜지션 Enter 시작");
        
        curtainUI.anchoredPosition = new Vector2(0, bottomPositionY);
        await MoveUIAsync(curtainUI, new Vector2(0f, centerPositionY), transitionDuration);
        
        Debug.Log("[TRANSITION] 커튼 트랜지션 Enter 끝");
    }

    public async void TransitionExit()
    {
        Debug.Log("[TRANSITION] 커튼 트랜지션 Exit 시작");
        
        curtainUI.anchoredPosition = new Vector2(0, centerPositionY);
        await MoveUIAsync(curtainUI, new Vector2(0f, topPositionY), transitionDuration);
        
        Debug.Log("[TRANSITION] 커튼 트랜지션 Exit 끝");
    }
    
    
    private async UniTask MoveUIAsync(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 startPos = rect.anchoredPosition;
        float elapsed = 0f;

        // 프레임마다 위치 갱신
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration); // 0 ~ 1
            rect.anchoredPosition = Vector2.LerpUnclamped(startPos, target, t);

            await UniTask.Yield(); // 다음 프레임까지 대기
        }

        // 정확하게 위치 보정
        rect.anchoredPosition = target;
    }
}
