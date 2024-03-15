using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObjects/PlayerStats", order = 3)]

public class PlayerStats : ScriptableObject
{
    [SerializeField]
    private List<StatInfo> statInfoList = new List<StatInfo>();

    private Dictionary<Stat, float> stats;

    public Dictionary<Stat, float> Stats
    {
        get
        {
            if (stats == null)
            {
                InitializeStats();
            }
            return stats;
        }
    }

    [ContextMenu("Initialize Stats")]
    private void InitializeStats()
    {
        stats = new Dictionary<Stat, float>();
        foreach (StatInfo statInfo in statInfoList)
        {
            stats[statInfo.statType] = statInfo.value;
        }
    }

    public float GetStatValue(Stat stat)
    {
        return stats[stat];
    }
}

[Serializable]
public class StatInfo
{
    public Stat statType;
    public float value;
}
public enum Stat
{
    None,
    WalkSpeed,
    RunSpeed,
    SpeedMutiplyer,
    Jumpforce,
    Attack,
    MaxHealth,
    CurHealth
}

/* How to Use
$$ Access the stats dictionary using the property

Dictionary<Stat, float> stats = playerStats.Stats;

$$ Access individual stats

float walkSpeed = stats[Stat.WalkSpeed];
float maxHealth = stats[Stat.MaxHealth];
 ...
*/