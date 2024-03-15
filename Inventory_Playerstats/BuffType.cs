using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bufftype", menuName = "ScriptableObjects/BuffTypes", order = 2)]
public class BuffType : ScriptableObject
{
    public enum BuffTypes
    {
        None,
        Speed,
        AttackForce,
        HealthChange,
        HealthChangeOverTime,
        Defence,
    }
    public enum MutiBuffConfig
    {
        notStackable,
        StackDuration,
        HighestTime,
        addEffect,
        mutiplyEffect
    }
    public BuffTypes buffTypes;
    public MutiBuffConfig StackMethod;
    public string BuffName;
    public float value;
    public float duration;
    public float curDuration;
    //Icon
    //special effects
}

