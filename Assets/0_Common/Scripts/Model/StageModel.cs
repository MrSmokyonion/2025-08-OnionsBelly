using UnityEngine;

public class StageModel
{
    public string StageName;
    public GameObject StagePrefab;
    public E_CHAPTER_NUMBER ChapterNumber;


    public StageModel(string stageName, GameObject stagePrefab, E_CHAPTER_NUMBER chapterNumber)
    {
        StageName = stageName;
        StagePrefab = stagePrefab;
        ChapterNumber = chapterNumber;
    }
}
