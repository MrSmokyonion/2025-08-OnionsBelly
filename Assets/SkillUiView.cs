using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUiView : MonoBehaviour
{
    public Slider slider;
    public Image targetImage;
    public float duration;

    private Coroutine asd;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (asd != null)
                return;
            
            asd = StartCoroutine(OnSkillCooldown());
        }
    }

    IEnumerator OnSkillCooldown()
    {
        slider.value = 0;

        while (true)
        {
            yield return null;
            slider.value += Time.deltaTime;
            if (slider.value >= 1)
            {
                break;
            }
        }

        Color startColor = Color.white;
        Color endColor = Color.black;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            targetImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // 마지막에 확실히 블랙으로 설정
        targetImage.color = endColor;
        
        asd =  null;
    }
}
