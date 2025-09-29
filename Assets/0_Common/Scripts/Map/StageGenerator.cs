using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class StageGenerator : MonoBehaviour, IStageGeneratable
{
    [FormerlySerializedAs("MapDatas")] public List<StageData> StageDataList;  //맵 패턴들
    public GameObject ExitPortalPrefab; //포탈 오브젝트

    public Transform DEV_PlayerTR;  //플레이어 Transform
    
    private StageModel currentStageModel;
    private GameObject currentStageObject;
    private StageInstance currentStageInstance;
    private GameObject currentExitPortal;


    public void RefreshStage()
    {
        if (currentStageObject != null)
        {
            Destroy(currentStageObject);
        }
        
        GenerateStage();
    }
    
    public void GenerateStage()
    {
        currentStageModel = GetRandomStageModel(StageDataList);
        if (currentStageModel == null)
        {
            Debug.LogError("맵 랜덤 생성 하려고 했더니만, 맵이 없는디요?");
            return;
        }
        
        currentStageObject = Instantiate(currentStageModel.StagePrefab, Vector3.zero, Quaternion.identity);
        currentStageInstance = currentStageObject.GetComponent<StageInstance>();
        currentExitPortal = Instantiate(ExitPortalPrefab, currentStageInstance.ExitTR.position, Quaternion.identity, currentStageObject.transform);
        currentExitPortal.GetComponent<ExitPortalController>().OnExitPortal.AddListener(RefreshStage);
        
        Debug.Log($"맵 랜덤 생성 완료! 맵 이름 : {currentStageModel.StageName}");
    }

    private StageModel GetRandomStageModel(List<StageData> mapDatas)
    {
        int mapCount = mapDatas.Count;
        
        if (mapCount == 0) return null;
        
        int mapIndex = Random.Range(0, mapCount);
        ICastModel<StageModel> castModel = mapDatas[mapIndex];
        return castModel.ToModel();
    }
}
