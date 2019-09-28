using System.Collections;
using System.Collections.Generic;
using HappyUnity.TransformUtils;
using UnityEngine;

public class RoboxBehaviour : EnemyUnit
{
    private FollowTarget _smoothMover;
    public Transform target;
    
    protected override void Awake()
    {
        base.Awake();
        _smoothMover = gameObject.GetComponent<FollowTarget>();
    }
    
    private void FixedUpdate()
    {
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
        _smoothMover.Target = target;
        _smoothMover.StartFollowing();
    }
}
