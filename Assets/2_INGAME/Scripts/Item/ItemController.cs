using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private ItemBase itemBase;

    private void Start()
    {
        itemBase = GetComponent<ItemBase>();
        Canvas canvas = uiCanvas.GetComponent<Canvas>();
        
        InputHandler.Instance.OnInputAction += InteractItem;
    }

    private void InteractItem(InputHandler.InputActionType actionType, object actionData)
    {
        if (actionType == InputHandler.InputActionType.Interact)
        {
            itemBase.Use();
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiCanvas.SetActive(true);   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiCanvas.SetActive(false);   
        }
    }
}
