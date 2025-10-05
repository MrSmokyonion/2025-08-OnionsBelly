using UnityEngine;

public class DEV_VineAnchorFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
