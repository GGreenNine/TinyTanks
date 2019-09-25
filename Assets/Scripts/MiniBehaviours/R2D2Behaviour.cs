using System.Collections;
using System.Collections.Generic;
using HappyUnity.TransformUtils;
using UnityEngine;

public class R2D2Behaviour : EnemyUnit
{
    private SmoothMover _smoothMover;
    public Transform target;
    
    protected override void Awake()
    {
        base.Awake();
        _smoothMover = gameObject.GetComponent<SmoothMover>();
    }
    
    private void FixedUpdate()
    {
        if (target)
        {
            _smoothMover.Move();
        }
    }
    
    protected override void RequestDestruction()
    {
        gameObject.SetActive(false);
    }

    public override void Spawn()
    {
        base.Spawn();
        if (target == null)
            target = FindObjectOfType<TinyTank>().transform;
        ConfigureSmoothMover();
    }
    
    public void ConfigureSmoothMover()
    {
        _smoothMover.TargetPosition = target;
        _smoothMover.BeginMoving();
    }
}
