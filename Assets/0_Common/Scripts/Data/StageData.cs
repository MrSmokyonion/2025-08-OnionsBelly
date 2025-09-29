using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "StageData_0_0", menuName = "Custom Data/Stage Data")]
public class StageData : ScriptableObject, ICastModel<StageModel>
{
    [FormerlySerializedAs("MapName")] public readonly string StageName;
    [FormerlySerializedAs("MapPrefab")] public readonly GameObject StagePrefab;
    public readonly E_CHAPTER_NUMBER ChapterNumber;

    public StageModel ToModel()
    {
        return new StageModel(StageName, StagePrefab, ChapterNumber);
    }
}
