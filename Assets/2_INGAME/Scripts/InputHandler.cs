using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 조작키의 입력을 감지하는 코드. 플레이어는 여기서 입력을 감지해서 PlayerMovement.cs 에서 입력을 처리함.
/// </summary>
public class InputHandler : SingletonMonoBehaviour<InputHandler>
{
    public Action<InputActionType, object> OnInputAction;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                switch (context.phase)
                {
                    case InputActionPhase.Started:
                        //Debug.Log("Move 시작!");
                        break;
                    case InputActionPhase.Performed:
                        Vector2 move = context.ReadValue<Vector2>();
                        OnInputAction(InputActionType.Move, move);
                        //Debug.Log($"Move 중: {move}");
                        break;
                    case InputActionPhase.Canceled:
                        OnInputAction(InputActionType.Move, Vector2.zero);
                        //Debug.Log("Move 끝남!");
                        break;
                }
                break;
            
            case "Jump":
                if (context.performed)
                {
                    OnInputAction(InputActionType.Jump, true);
                    //Debug.Log("Jumped!");
                }
                else if(context.canceled)
                {
                    OnInputAction(InputActionType.Jump, false);
                    //Debug.Log("Jump end!");
                }
                break;
            
            case "Dash":
                if (context.performed)
                {
                    OnInputAction(InputActionType.Dash, null);
                    //Debug.Log("Jumped!");
                }
                break;
        }
    }

    public enum InputActionType
    {
        Move,
        Jump,
        Dash,
    }
}
