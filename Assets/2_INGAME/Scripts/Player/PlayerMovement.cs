using System;
using UnityEngine;

/// <summary>
/// 플레이어의 이동을 처리하는 클래스.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>최대 이동속도</summary>
    [Header("Run Variables")]
    public float runMaxSpeed;
    /// <summary>이동하기 시작할 때 가속정도</summary>
    public float runAccelAmount;
    /// <summary>멈출 때 감속정도</summary>
    public float runDeccelAmount;

    /// <summary>점프할 때 가속하는 힘</summary>
    [Header("Jump Variables")]
    public float JumpPower;
    /// <summary>낭떨어지 점프할 때 코요테 타임</summary>
    public float CoyoteTime;
    /// <summary>점프시, 최소체공가능시간</summary>
    public float minimumJumpingTime;
    /// <summary>점프시, 최대체공가능시간</summary>
    public float maximumJumpingTime;
    /// <summary>최대 떨어지는 속도</summary>
    public float maximumFallingSpeed;
    /// <summary>점프 최대 가능 횟수</summary>
    public int maxJumps;
    
    /// <summary>바닥 확인하는 충돌체</summary>
    [Header("Ground Check Variables")]
    public Collider2D legCollider;
    /// <summary>바닥 레이어</summary>
    public LayerMask groundLayer;
    
    /// <summary>벽 확인하는 충돌체</summary>
    [Header("Wall Check Variables")]
    public Collider2D wallColliderLeft;
    public Collider2D wallColliderRight;
    /// <summary>벽 레이어</summary>
    public LayerMask wallLayer;

    /// <summary>이동 방향</summary>
    private float direction;
    /// <summary>점프 입력 확인용 변수</summary>
    private bool jumpInput;
    
    /// <summary>코요테 점프 카운터</summary>
    private float coyoteTimeCounter;
    /// <summary>롱 점프 관리용 변수</summary>
    private bool longJump;
    //private float jumpedtime; //점프키 누른 타이밍
    /// <summary>점프키를 땠는지 확인하기 위한 변수</summary>
    private bool isJumpKeyEnd = false;
    /// <summary>공중에 떠있는 시간 확인용</summary>
    private float floatedtime;
    /// <summary>현재 점프한 횟수</summary>
    private int jumpCount;
    /// <summary>기본 중력값</summary>
    private float defaultgrav;
    /// <summary>떨어질 때 중력 가속 곱연산 배수</summary>
    [SerializeField] float gravityMult;
    
    private Rigidbody2D rigidbody;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        direction = 0;
        coyoteTimeCounter = 0;
        defaultgrav = rigidbody.gravityScale;
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnInputAction += ReceiveInput;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnInputAction -= ReceiveInput;
    }

    // Update is called once per frame
    void Update()
    {
        //점프~착지 처리
        if (isOnGround())
        {
            coyoteTimeCounter = CoyoteTime;
            jumpCount = 0;
            Debug.Log("Player is On Ground");
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            if (coyoteTimeCounter < CoyoteTime && jumpCount == 0)
            {
                jumpCount++;
            }
        }
        
        //롱 점프 확인 겸 점프 코드
        if (longJump)
        {
            floatedtime += Time.deltaTime;
            float t = floatedtime / maximumJumpingTime;
            float _velocity = Mathf.Lerp(JumpPower, 1f, t);
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, _velocity);
            //Debug.Log("t = " + t.ToString() + ", velocity = " + _velocity.ToString() + ", floatedTime = " + floatedtime.ToString());

            if (floatedtime > minimumJumpingTime)
            {
                if (isJumpKeyEnd || floatedtime > maximumJumpingTime) //최대 점프 체공가능 시간이 다되면 컷
                {
                    longJump = false;
                    isJumpKeyEnd = false;
                    floatedtime = 0f;
                    rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0f);
                    legCollider.enabled = true;
                }
            }
        }
        
        //떨어질 때 중력 가속에 배수를 곱연산
        if (rigidbody.linearVelocity.y < 0)
        {
            rigidbody.gravityScale = defaultgrav * gravityMult;
        }
        else
        {
            rigidbody.gravityScale = defaultgrav;
        }
        
        //떨어질 때 최대 속도값
        if (rigidbody.linearVelocityY <= maximumFallingSpeed)
            rigidbody.linearVelocityY = maximumFallingSpeed;
        
        Run();  
    }

    /// <summary>
    /// 플레이어의 입력값에 따른 콜백함수
    /// </summary>
    /// <param name="actionType">인풋값 타입</param>
    /// <param name="actionData">함께 넘어오는 데이터</param>
    private void ReceiveInput(InputHandler.InputActionType actionType, object actionData)
    {
        switch (actionType)
        {
            case InputHandler.InputActionType.Move:
                Vector2 moveInput = (Vector2)actionData;
                if (moveInput.x > 0)
                {
                    direction = 1;
                }
                else if (moveInput.x < 0)
                {
                    direction = -1;
                }
                else if (moveInput.x == 0)
                {
                    direction = 0;
                }
                break;
            
            case InputHandler.InputActionType.Jump:
                jumpInput = (bool)actionData;
                
                //점프 눌렀을 때
                if (jumpInput)
                {
                    if (!longJump && coyoteTimeCounter >= 0f)
                    {
                        //jumpedtime = Time.time;
                        coyoteTimeCounter = -1f;
                        longJump = true;
                        floatedtime = 0f;
                        isJumpKeyEnd = false;
                        legCollider.enabled = false;
                        jumpCount++;
                    } else if (maxJumps > jumpCount)
                    {
                        coyoteTimeCounter = -1f;
                        longJump = true;
                        floatedtime = 0f;
                        isJumpKeyEnd = false;
                        legCollider.enabled = false;
                        jumpCount++;
                    }
                }

                //점프키 땠을 때
                if (!jumpInput)
                {
                    isJumpKeyEnd = true;
                }
                
                break;
            
            default:
                Debug.Log("WTF? What is this Input Action Type?");
                break;
        }
    }
    
    /// <summary>
    /// 좌우 이동 코드
    /// </summary>
    private void Run()
    {
        #region wall sticky bug solve

        if (direction > 0 && isTouchingWallRight())
        {
            return;
        }
        if (direction < 0 &&  isTouchingWallLeft())
        {
            return;
        }

        #endregion
        
        float targetSpeed = direction * runMaxSpeed;

        #region Calculate AccelRate

        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;

        #endregion

        #region Conserve Momentum

        if (Mathf.Abs(rigidbody.linearVelocity.x) > Mathf.Abs(targetSpeed) &&
            Mathf.Sign(rigidbody.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = 0;
        }

        #endregion

        float speedDif = targetSpeed - rigidbody.linearVelocity.x;

        float movement = speedDif * accelRate;

        rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    
    /// <summary>
    /// 바닥에 착지했는지 확인용 코드
    /// </summary>
    /// <returns>바닥에 발 닿았음?</returns>
    public bool isOnGround()
    {
        bool isTouchingGround = legCollider.IsTouchingLayers(groundLayer);
        return isTouchingGround;
    }
    
    /// <summary>
    /// 옆구리가 벽에 닿았는지 확인용 코드 (왼쪽)
    /// </summary>
    /// <returns>옆구리가 벽에 닿았음?</returns>
    public bool isTouchingWallLeft()
    {
        bool isTouchingWall = wallColliderLeft.IsTouchingLayers(wallLayer);
        
        return isTouchingWall;
    }
    
    /// <summary>
    /// 옆구리가 벽에 닿았는지 확인용 코드 (오른쪽)
    /// </summary>
    /// <returns>옆구리가 벽에 닿았음?</returns>
    public bool isTouchingWallRight()
    {
        bool isTouchingWall = wallColliderRight.IsTouchingLayers(wallLayer);
        
        return isTouchingWall;
    }
}
