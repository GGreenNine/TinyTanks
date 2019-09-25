using System.Collections;
using System.Collections.Generic;
using Asteroids;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : GameUnit
{
    [SerializeField]
    [Range(0, 3000)]
    protected int destructionScore = 100;

    protected override void Awake()
    {
        base.Awake();
        _health.On_Death += () => Score.Earn(destructionScore);
    }
    
}
