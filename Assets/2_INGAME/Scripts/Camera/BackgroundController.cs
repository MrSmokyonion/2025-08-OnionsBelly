using UnityEngine;

/// <summary>
/// 패럴랙스 카메라 연출을 위한 배경 이미지들 컨트롤용 클래스
/// </summary>
public class BackgroundController : MonoBehaviour
{
    /// <summary>해당 이미지가 카메라에 대응해서 움직이는 척도 X값</summary>
    public float parallaxEffectX;
    /// <summary>해당 이미지가 카메라에 대응해서 움직이는 척도 Y값</summary>
    public float parallaxEffectY;
    
    /// <summary>메인 카메라 레퍼런스</summary>
    private Transform cameraTR;
    /// <summary>게임 첫 시작때 배경 초기 위치</summary>
    private Vector2 startPos;
    /// <summary>이미지 가로 길이값</summary>
    private float spriteLength;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Camera.main != null) cameraTR = Camera.main.transform;
        startPos = transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceX = cameraTR.position.x * parallaxEffectX;
        float distanceY = cameraTR.position.y * parallaxEffectY;
        
        transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, transform.position.z);
        
        float movement = cameraTR.position.x * (1 - parallaxEffectX);
        
        if(movement > startPos.x + spriteLength) startPos.x += spriteLength;
        else if (movement < startPos.x - spriteLength) startPos.x -= spriteLength;
    }
}
