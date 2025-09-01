using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapMNG : MonoBehaviour
{
    public List<MapData> MapDatas;
    public GameObject ExitPortalPrefab;

    public Transform DEV_PlayerTR;
    
    private MapData currentMapData;
    private GameObject currentMap;
    private MapInstance currentMapInstance;
    private GameObject currentExitPortal;

    private void Start()
    {
        RefreshStage();
    }

    public void RefreshStage()
    {
        DEV_PlayerTR.gameObject.SetActive(false);
        
        GenerateMap();
        
        DEV_PlayerTR.position = currentMapInstance.Entrance.position;
        DEV_PlayerTR.gameObject.SetActive(true);
    }
    
    public void GenerateMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
        }
        
        currentMapData = GetRandomMap(MapDatas);
        if (currentMapData == null)
        {
            Debug.LogError("맵 랜덤 생성 하려고 했더니만, 맵이 없는디요?");
            return;
        }
        
        currentMap = Instantiate(currentMapData.MapPrefab, Vector3.zero, Quaternion.identity);
        currentMapInstance = currentMap.GetComponent<MapInstance>();
        currentExitPortal = Instantiate(ExitPortalPrefab, currentMapInstance.Exit.position, Quaternion.identity, currentMap.transform);
        currentExitPortal.GetComponent<ExitPortalController>().OnExitPortal.AddListener(RefreshStage);
        
        Debug.Log($"맵 랜덤 생성 완료! 맵 이름 : {currentMapData.MapName}");
    }

    private MapData GetRandomMap(List<MapData> mapDatas)
    {
        int mapCount = mapDatas.Count;
        
        if (mapCount == 0) return null;
        
        int mapIndex = Random.Range(0, mapCount);
        return mapDatas[mapIndex];
    }
}
