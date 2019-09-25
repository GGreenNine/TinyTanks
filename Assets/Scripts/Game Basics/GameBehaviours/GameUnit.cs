using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids;
using HappyUnity.Spawners.ObjectPools;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameUnit : GameBehaviour
{
    public enum CharacterType
    {
        Player,
        AI
    }

    public CharacterType characterType;

    public enum CharacterState
    {
        Normal,
        Paused,
        Dead
    }

    public CharacterState currentCharacterState;

    public Health _health;

    public bool isRandomScale;
    [Range(0, 5)] [SerializeField] float minScale = 0f;

    [Range(0, 5)] [SerializeField] float maxScale = 0.3f;

    public GameObject CharacterBody;
    public Animator CharacterAnimator;
    public bool isAbleToKnockBack;

    public Vector3 Randomize()
    {
        float r = Random.Range(minScale, maxScale);
        return new Vector3(r, r, r);
    }

    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
    }

    public virtual void Spawn()
    {
        _health.Revive();
        if (isRandomScale)
            transform.localScale = Randomize();
        transform.position = GameField.GetRandomSpawnPosition();
    }


    public virtual void SpawnAt(Vector3 position)
    {
        transform.localScale = Randomize();
        transform.position = position;
    }
}