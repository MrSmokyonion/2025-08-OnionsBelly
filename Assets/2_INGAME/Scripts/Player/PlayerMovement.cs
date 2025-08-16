using System;
using UnityEngine;

/// <summary>
/// 플레이어의 이동을 처리하는 클래스.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float runMaxSpeed;
    public float runAccelAmount;
    public float runDeccelAmount;

    public float JumpPower;
    public float CoyoteTime;
    public float minimumJumpingTime; //점프시, 최소체공가능시간
    public float maximumJumpingTime; //점프시, 최대체공가능시간
    
    public Collider2D legCollider;
    public LayerMask groundLayer;

    private float direction;
    private bool jumpInput;
    
    private float coyoteTimeCounter;
    private bool longJump;
    float jumpedtime; //점프키 누른 타이밍
    private bool isJumpKeyEnd = false; //점프키를 땠는가?
    private float floatedtime; //공중에 떠있는 시간
    private float defaultgrav;
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
            Debug.Log("Player is On Ground");
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
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
        
        if (rigidbody.linearVelocity.y < 0)
        {
            rigidbody.gravityScale = defaultgrav * gravityMult;
        }
        else
        {
            rigidbody.gravityScale = defaultgrav;
        }
        
        Run();  
    }

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
                
                if (jumpInput && !longJump && coyoteTimeCounter >= 0f)
                {
                    jumpedtime = Time.time;
                    coyoteTimeCounter = -1f;
                    longJump = true;
                    floatedtime = 0f;
                    isJumpKeyEnd = false;
                    legCollider.enabled = false;
                }

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
    
    private void Run()
    {
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
    
    public bool isOnGround()
    {
        bool isTouchingGround = legCollider.IsTouchingLayers(groundLayer);
        return isTouchingGround;
    }
}
