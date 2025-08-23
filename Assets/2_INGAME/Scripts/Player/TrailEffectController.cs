using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffectController : MonoBehaviour
{
    public GameObject TrailEffectPrefab;
    public float trailSpawnInterval;
    
    private PlayerMovement playerMovement;
    private SpriteRenderer childSpriteRenderer;
    private bool trailEffectActive;
    private float trailSpawnTimer;
    
    private void Start()
    {
        if(playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        
        childSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (childSpriteRenderer != null)
        {
            Debug.Log("자식의 SpriteRenderer를 찾았습니다.");
        }
        else
        {
            Debug.LogWarning("자식 오브젝트에 SpriteRenderer가 없습니다.");
        }
    }

    private void OnEnable()
    {
        if(playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        
        playerMovement.OnDashStart += TrailEffectStart;
        playerMovement.OnDashEnd += TrailEffectOff;
    }

    private void OnDisable()
    {
        playerMovement.OnDashStart -= TrailEffectStart;
        playerMovement.OnDashEnd -= TrailEffectOff;
    }

    private void Update()
    {
        if (trailEffectActive)
        {
            trailSpawnTimer += Time.deltaTime;
            if (trailSpawnTimer >= trailSpawnInterval)
            {
                trailSpawnTimer = 0f;
                GameObject trail = Instantiate(TrailEffectPrefab, playerMovement.transform.position, playerMovement.transform.rotation);
                trail.GetComponent<SpriteRenderer>().sprite = childSpriteRenderer.sprite;
                trail.transform.SetParent(null);
            }
        }
    }

    void TrailEffectStart()
    {
        trailEffectActive = true;
        trailSpawnTimer = 0f;
    }
    void TrailEffectOff()
    {
        trailEffectActive = false;
    }
}
