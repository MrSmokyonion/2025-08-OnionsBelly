using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StageInstance : MonoBehaviour
{
    [FormerlySerializedAs("Entrance")] public Transform EntranceTR;
    [FormerlySerializedAs("Exit")] public Transform ExitTR;
    public List<Transform> EnemySpawnPoints;
}
