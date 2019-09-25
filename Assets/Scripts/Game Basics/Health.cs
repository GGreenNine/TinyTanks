using System;
using UnityEngine;
using System.Collections;
using Asteroids;
using Game_Basics;
using MiniBehaviours;
using Unity.Collections;

public class Health : MonoBehaviour
{
    public GameObject Model;

    [ReadOnly] public int CurrentHP;
    [ReadOnly] public bool Invulnerable;

    [Header("Health")] public int InitialHealth = 10;
    public int MaximumHealth = 10;

    public delegate void HealthEvent();

    public event HealthEvent On_GettingDamage;
    public event HealthEvent On_Death;
    public event HealthEvent On_Revive;

    [Header("Death")] public bool Destoy = false;
    public GameObject InstantiatedOnDeath;
    public int PointsWhenDestroyed;
    public HealthBar HealthBar;


    public delegate void Respawn();

    public event Respawn On_Respawn;

    protected Renderer Renderer;
    protected Vector3 InitialPosition;
    protected GameUnit Character;
    protected Rigidbody2D Rigidbody2D;
    protected bool Initialized = false;
    protected Animator Animator;
    protected Collider2D _collider2D;
    protected AutoRespawn AutoRespawn;
    protected Armor Armor;

    protected virtual void Initialization()
    {
        Character = GetComponent<GameUnit>();
        if (Model != null)
        {
            Model.SetActive(true);
        }

        Renderer = GetComponent<Renderer>();
        if (Character != null)
        {
            if (Character.CharacterBody != null)
            {
                if (Character.CharacterBody.GetComponent<Renderer>() != null)
                {
                    Renderer = Character.CharacterBody.GetComponent<Renderer>();
                }
            }
        }

        if (Character != null)
        {
            if (Character.CharacterAnimator != null)
            {
                Animator = Character.CharacterAnimator;
            }
            else
            {
                Animator = GetComponent<Animator>();
            }
        }
        else
        {
            Animator = GetComponent<Animator>();
        }

        if (Animator != null)
        {
            Animator.logWarnings = false;
        }

        HealthBar = GetComponent<HealthBar>();
        _collider2D = GetComponent<Collider2D>();
        AutoRespawn = GetComponent<AutoRespawn>();
        Armor = GetComponent<Armor>();

        Initialized = true;
        CurrentHP = InitialHealth;
        DamageEnabled();
        UpdateHealthBar(false);
    }

    /// <summary>
    /// Called when the object takes damage
    /// </summary>
    /// <param name="damage">The amount of health points that will get lost.</param>
    /// <param name="instigator">The object that caused the damage.</param>
    /// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage.</param>
    /// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
    public virtual void Damage(int damage, float invincibilityDuration)
    {
        if (Invulnerable)
        {
            return;
        }

        if ((CurrentHP <= 0) && (InitialHealth != 0))
        {
            return;
        }

        if (Armor != null)
        {
            CurrentHP -= (int) (damage * Armor.InitialArmor);
        }
        else
        {
            CurrentHP -= damage;
        }

        On_GettingDamage?.Invoke();

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        if (invincibilityDuration > 0)
        {
            DamageDisabled();
            StartCoroutine(DamageEnabled(invincibilityDuration));
        }


        UpdateHealthBar(true);

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;

            Kill();
        }
    }

    public void Kill()
    {
        if (Character != null)
        {
            CurrentHP = 0;
            DamageDisabled();

            Character.GetScored(PointsWhenDestroyed);
            Character.currentCharacterState = GameUnit.CharacterState.Dead;

            On_Death?.Invoke();

            if (InstantiatedOnDeath != null)
            {
                Instantiate(InstantiatedOnDeath, transform.position, Quaternion.identity);
            }

            if (AutoRespawn != null)
            {
                AutoRespawn.Kill();
            }

            Character.RemoveFromGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Revive this object.
    /// </summary>
    public virtual void Revive()
    {
        if (!Initialized)
        {
            return;
        }

        if (_collider2D != null)
        {
            _collider2D.enabled = true;
        }

        if (Character != null)
        {
            Character.currentCharacterState = GameUnit.CharacterState.Normal;
        }

        Initialization();
        if(HealthBar!= null)
        HealthBar.FinalShowBar();
        UpdateHealthBar(false);
        On_Revive?.Invoke();
    }

    /// <summary>
    /// Updates the character's health bar progress.
    /// </summary>
    protected virtual void UpdateHealthBar(bool show)
    {
        if (HealthBar != null)
        {
            HealthBar.UpdateBar(CurrentHP, 0f, MaximumHealth, show);
        }

        if (Character != null)
        {
            if (Character.characterType == GameUnit.CharacterType.Player)
            {
                // We update the health bar
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdatePlayerBar(CurrentHP, 0f, MaximumHealth);
                }
            }
        }
    }

    /// <summary>
    /// Called when the character gets health (from a stimpack for example)
    /// </summary>
    /// <param name="health">The health the character gets.</param>
    /// <param name="instigator">The thing that gives the character health.</param>
    public virtual void GetHealth(int health, GameObject instigator)
    {
        // this function adds health to the character's Health and prevents it to go above MaxHealth.
        CurrentHP = Mathf.Min(CurrentHP + health, MaximumHealth);
        UpdateHealthBar(true);
    }

    /// <summary>
    /// Resets the character's health to its max value
    /// </summary>
    public virtual void ResetHealthToMaxHealth()
    {
        CurrentHP = MaximumHealth;
        UpdateHealthBar(false);
    }

    /// <summary>
    /// Prevents the character from taking any damage
    /// </summary>
    public virtual void DamageDisabled()
    {
        Invulnerable = true;
    }

    /// <summary>
    /// makes the character able to take damage again after the specified delay
    /// </summary>
    /// <returns>The layer collision.</returns>
    public virtual IEnumerator DamageEnabled(float delay)
    {
        yield return new WaitForSeconds(delay);
        Invulnerable = false;
    }

    /// <summary>
    /// Allows the character to take damage
    /// </summary>
    public virtual void DamageEnabled()
    {
        Invulnerable = false;
    }
}