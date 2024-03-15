using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public PlayerStats playerStat;
    [Header("Base Stats")]

    public float baseHealth;
    public float baseSpeed;
    public float ammo;
    private void Awake()
    {
        //baseHealth = playerStat.GetStatValue(Stat.MaxHealth);
        //baseSpeed = playerStat.GetStatValue(Stat.WalkSpeed);
    }
    public struct buffInfo
    {

    }

    [Header("Buffs")]
    public List<BuffType> activeBuffs = new List<BuffType>();

    public void AddBuff(BuffType buff)
    {
        if (buff == null)
        {
            Debug.LogWarning("Trying to apply a null buff.");
            return;
        }
        switch (buff.StackMethod)
        {
            case BuffType.MutiBuffConfig.notStackable:
                foreach (BuffType buffType in activeBuffs)
                {
                    if (buffType.BuffName == buff.BuffName)
                    {
                        break;
                    }
                }
                activeBuffs.Add(buff);
                break;
            case BuffType.MutiBuffConfig.StackDuration:
                foreach (BuffType buffType in activeBuffs)
                {
                    if (buffType.BuffName == buff.BuffName)
                    {
                        buffType.curDuration = buff.curDuration > buffType.duration ? buff.curDuration : buffType.duration;
                    }
                }
                activeBuffs.Add(buff);
                break;
        }
    }
}
