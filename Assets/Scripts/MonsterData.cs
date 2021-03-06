﻿using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class MonsterDataAnimations
{
    [SerializeField]
    public MDLAnimation[] stand;

    [SerializeField]
    public MDLAnimation[] patrol;

    [SerializeField]
    public MDLAnimation[] chase;

    [SerializeField]
    public MDLAnimation[] leap;

    [SerializeField]
    public MDLAnimation[] attack;

    [SerializeField]
    public MDLAnimation[] pain;

    [SerializeField]
    public MDLAnimation[] death;
}

[Serializable]
public class MonsterDataAudio
{
    [SerializeField]
    public AudioClip[] idle;

    [SerializeField]
    public AudioClip[] sight;

    [SerializeField]
    public AudioClip[] attack;

    [SerializeField]
    public AudioClip[] pain;

    [SerializeField]
    public AudioClip[] death;
}

[Serializable]
[CreateAssetMenu]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    public MonsterDataAnimations animations;

    [SerializeField]
    public MonsterDataAudio audio;

    [SerializeField]
    public float sightDistance = 20;

    [SerializeField]
    public float fov = 90;

    [SerializeField]
    public float closeCombatRange = 1.2f;

    [SerializeField]
    public float leapRange;

    [SerializeField]
    public int visionRate = 2;

    public float sightDistanceSqr
    {
        get { return sightDistance * sightDistance; }
    }

    public float closeCombatRangeSqr
    {
        get { return closeCombatRange * closeCombatRange; }
    }
}
