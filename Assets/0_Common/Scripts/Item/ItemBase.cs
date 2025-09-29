using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public string ItemName;
    
    public string ItemDescription;
    
    public abstract void Use();
}
