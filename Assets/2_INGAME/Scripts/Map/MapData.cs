using UnityEngine;

[CreateAssetMenu(fileName = "MapData_Chapter_0_0", menuName = "Custom Data/Map Data")]
public class MapData : ScriptableObject
{
    public string MapName;
    public GameObject MapPrefab;
    public E_CHAPTER_NUMBER ChapterNumber;
}
