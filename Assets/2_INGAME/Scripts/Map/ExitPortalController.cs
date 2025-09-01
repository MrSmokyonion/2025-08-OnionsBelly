using System;
using UnityEngine;
using UnityEngine.Events;

public class ExitPortalController : MonoBehaviour
{
    public UnityEvent OnExitPortal;
    public GameObject Indicator;

    private void Start()
    {
        Indicator.gameObject.SetActive(false);
        OnExitPortal.AddListener(DEV_lkjasd);
    }

    private void InteractExitPortal(InputHandler.InputActionType actionType, object actionData)
    {
        if (actionType == InputHandler.InputActionType.Interact)
        {
            OnExitPortal.Invoke();
        }
    }

    private void DEV_lkjasd()
    {
        
        Debug.Log("Exit!");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InputHandler.Instance.OnInputAction += InteractExitPortal;
            Indicator.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InputHandler.Instance.OnInputAction -= InteractExitPortal;
            Indicator.gameObject.SetActive(false);
        }
    }
}
