using System.Collections;
using System.Collections.Generic;
using Asteroids;
using Unity.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Parameters

        public enum WeaponStates
        {
            WeaponStart,
            WeaponDelayBeforeUse,
            WeaponUse,
            WeaponDelayBetweenUses,
            WeaponStop,
            WeaponReloadNeeded,
            WeaponReloadStart,
            WeaponReload,
            WeaponReloadStop,
            WeaponIdle
        }

        public enum TriggerModes
        {
            SemiAuto,
            Auto
        }

        public TriggerModes TriggerMode = TriggerModes.SemiAuto;


        public WeaponStates WeaponState;
        public float DelayBeforeUse = 0f;
        public float TimeBetweenUses = 1f;
        public bool MagazineBased = false;
        public bool IsGradualBased = false;
        public int MagazineSize = 5;
        public float ReloadTime = 2f;
        public int AmmoConsumedPerShot = 1;
        public bool AutoReload;
        public GameObject owner;


        [ReadOnly]
        /// the current amount of ammo loaded inside the weapon
        public int CurrentAmmoLoaded = 0;

        public WeaponStates CurrentState
        {
            get => WeaponState;
            protected set => WeaponState = value;
        }

        protected bool _reloading = false;
        protected float _delayBeforeUseCounter = 0f;
        protected float _delayBetweenUsesCounter = 0f;
        protected bool _triggerReleased = false;
        protected bool isAutoFilling = false;
        protected float _reloadingCounter = 0f;

        #endregion

        #region Events

        protected delegate void Reload();

        protected event Reload On_Reload_Needed;
        protected event Reload On_Reloading;
        protected event Reload On_Ammo_Ends;

        public delegate void OnAmmoCountChanged();

        public event OnAmmoCountChanged On_ammoCountChanged;

        #endregion

        /// <summary>
        /// On LateUpdate, processes the weapon state
        /// </summary>
        protected virtual void LateUpdate()
        {
            ProcessWeaponState();
        }

        protected void OnAmmoChanged()
        {
            On_ammoCountChanged?.Invoke();
        }

        /// <summary>
        /// Initialize this weapon.
        /// </summary>
        public virtual void Initialization()
        {
            WeaponState = (WeaponStates.WeaponIdle);
            CurrentAmmoLoaded = MagazineSize;
        }


        protected virtual void ProcessWeaponState()
        {
            switch (WeaponState)
            {
                case WeaponStates.WeaponIdle:
                    break;
                case WeaponStates.WeaponStart:
                    CaseWeaponStart();
                    break;

                case WeaponStates.WeaponDelayBeforeUse:
                    CaseWeaponDelayBeforeUse();
                    break;

                case WeaponStates.WeaponUse:
                    CaseWeaponUse();
                    break;

                case WeaponStates.WeaponDelayBetweenUses:
                    CaseWeaponDelayBetweenUses();
                    break;

                case WeaponStates.WeaponReloadNeeded:
                    CaseWeaponReloadNeeded();
                    break;

                case WeaponStates.WeaponReloadStart:
                    CaseWeaponReloadStart();
                    break;

                case WeaponStates.WeaponReload:
                    CaseWeaponReload();
                    break;

                case WeaponStates.WeaponReloadStop:
                    CaseWeaponReloadStop();
                    break;
            }
        }

        #region Case_Methods

        /// <summary>
        /// When the weapon starts we switch to a delay or shoot based on our weapon's settings
        /// </summary>
        public virtual void CaseWeaponStart()
        {
            if (DelayBeforeUse > 0)
            {
                _delayBeforeUseCounter = DelayBeforeUse;
                WeaponState = (WeaponStates.WeaponDelayBeforeUse);
            }
            else
            {
                ShootRequest();
            }
        }

        /// <summary>
        /// on reload, we reset our movement multiplier, and switch to reload stop once our reload delay has passed
        /// </summary>
        public virtual void CaseWeaponReload()
        {
            _reloadingCounter -= Time.deltaTime;
            if (_reloadingCounter <= 0)
            {
                WeaponState = (WeaponStates.WeaponReloadStop);
            }
        }

        /// <summary>
        /// If we're in delay before use, we wait until our delay is passed and then request a shoot
        /// </summary>
        public virtual void CaseWeaponDelayBeforeUse()
        {
            _delayBeforeUseCounter -= Time.deltaTime;
            if (_delayBeforeUseCounter <= 0)
            {
                ShootRequest();
            }
        }

        /// <summary>
        /// On weapon use we use our weapon then switch to delay between uses
        /// </summary>
        public virtual void CaseWeaponUse()
        {
            WeaponUse();
            _delayBetweenUsesCounter = TimeBetweenUses;
            WeaponState = (WeaponStates.WeaponDelayBetweenUses);
        }

        /// <summary>
        /// If a reload is needed, we mention it and switch to idle
        /// </summary>
        public virtual void CaseWeaponReloadNeeded()
        {
            ReloadNeeded();
            WeaponState = (WeaponStates.WeaponIdle);
        }

        /// <summary>
        /// on reload stop, we swtich to idle and load our ammo
        /// </summary>
        public virtual void CaseWeaponReloadStop()
        {
            _reloading = false;
            WeaponState = (WeaponStates.WeaponIdle);
            CurrentAmmoLoaded = MagazineSize;
        }

        /// <summary>
        /// When in delay between uses, we either turn our weapon off or make a shoot request
        /// </summary>
        public virtual void CaseWeaponDelayBetweenUses()
        {
            _delayBetweenUsesCounter -= Time.deltaTime;
            if (_delayBetweenUsesCounter <= 0)
            {
                if ((TriggerMode == TriggerModes.Auto) && !_triggerReleased)
                {
                    ShootRequest();
                }
                else
                {
                    TurnWeaponOff();
                }
            }
        }

        /// <summary>
        /// on reload start, we reload the weapon and switch to reload
        /// </summary>
        public virtual void CaseWeaponReloadStart()
        {
            ReloadWeapon();
            WeaponState = (WeaponStates.WeaponReload);
        }

        #endregion


        /// <summary>
        /// Reloads the weapon
        /// </summary>
        /// <param name="ammo">Ammo.</param>
        protected virtual void ReloadWeapon()
        {
            if (MagazineBased)
            {
                On_Reloading?.Invoke();
            }
        }

        /// <summary>
        /// When the weapon is used, plays the corresponding sound
        /// </summary>
        public virtual void WeaponUse()
        {
        }

        /// <summary>
        /// Called by input, turns the weapon on
        /// </summary>
        public virtual void WeaponInputStart()
        {
            if (_reloading)
            {
                return;
            }

            if (WeaponState == WeaponStates.WeaponIdle)
            {
                _triggerReleased = false;
                TurnWeaponOn();
            }
        }

        /// <summary>
        /// Seting the owner of a current weapon
        /// </summary>
        public void DefineOwner(GameObject owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Determines whether or not the weapon can fire
        /// </summary>
        public virtual void ShootRequest()
        {
            if (_reloading)
            {
                return;
            }

            if (MagazineBased)
            {
                if (CurrentAmmoLoaded != 0)
                {
                    if (EnoughAmmoToFire())
                    {
                        CurrentAmmoLoaded -= AmmoConsumedPerShot;
                        WeaponState = (WeaponStates.WeaponUse);
                        if (!EnoughAmmoToFire() && IsGradualBased)
                            StartCoroutine(AutoAmmoFilling());
                    }
                    else
                    {
                        if (AutoReload && MagazineBased)
                        {
                            InitiateReloadWeapon();
                        }
                        else
                        {
                            WeaponState = (WeaponStates.WeaponReloadNeeded);
                        }
                    }
                }
                else
                {
                    if (CurrentAmmoLoaded > 0)
                    {
                        WeaponState = (WeaponStates.WeaponUse);
                        CurrentAmmoLoaded -= AmmoConsumedPerShot;
                    }
                    else
                    {
                        if (AutoReload)
                        {
                            InitiateReloadWeapon();
                        }
                        else
                        {
                            WeaponState = (WeaponStates.WeaponReloadNeeded);
                        }
                    }
                }
            }
            else
            {
                if (CurrentAmmoLoaded != 0)
                {
                    WeaponState = EnoughAmmoToFire() ? WeaponStates.WeaponUse : WeaponStates.WeaponReloadNeeded;
                }
                else
                {
                    WeaponState = (WeaponStates.WeaponUse);
                }
            }
        }

        /// <summary>
        /// Describes what happens when the weapon needs a reload
        /// </summary>
        public virtual void ReloadNeeded()
        {
            On_Reload_Needed?.Invoke();
        }

        /// <summary>
        /// Turns the weapon off.
        /// </summary>
        public virtual void TurnWeaponOff()
        {
            if (WeaponState == WeaponStates.WeaponIdle)
            {
                return;
            }

            _triggerReleased = true;

            TriggerWeaponStopFeedback();
            WeaponState = (WeaponStates.WeaponIdle);
        }

        /// <summary>
        /// Describes what happens when the weapon starts
        /// </summary>
        protected virtual void TurnWeaponOn()
        {
            WeaponState = (WeaponStates.WeaponStart);
        }

        public virtual void InitiateReloadWeapon()
        {
            // if we're already reloading, we do nothing and exit
            if (_reloading)
            {
                return;
            }

            WeaponState = (WeaponStates.WeaponReloadStart);
            _reloading = true;
        }

        /// <summary>
        /// Plays the weapon's stop sound
        /// </summary>
        protected virtual void TriggerWeaponStopFeedback()
        {
        }

        /// <summary>
        /// Fills the weapon with ammo
        /// </summary>
        public virtual void FillWeaponWithAmmo()
        {
            if (MagazineBased)
            {
                int counter = 0;
                int stock = CurrentAmmoLoaded;
                for (int i = CurrentAmmoLoaded; i < MagazineSize; i++)
                {
                    if (stock > 0)
                    {
                        stock--;
                        counter++;
                    }
                }

                CurrentAmmoLoaded += counter;
            }
        }

        protected IEnumerator AutoAmmoFilling()
        {
            if (!IsGradualBased) yield break;
            if (isAutoFilling) yield break;
            if (!MagazineBased) yield break;
            isAutoFilling = true;

            while (CurrentAmmoLoaded< MagazineSize)
            {
                yield return new WaitForSeconds(ReloadTime);
                CurrentAmmoLoaded++;
                OnAmmoChanged();
            }

            isAutoFilling = false;
        }

        /// <summary>
        /// Returns true if this weapon has enough ammo to fire, false otherwise
        /// </summary>
        /// <returns></returns>
        public bool EnoughAmmoToFire()
        {
            if (MagazineBased)
            {
                return CurrentAmmoLoaded >= AmmoConsumedPerShot;
            }

            return CurrentAmmoLoaded >= AmmoConsumedPerShot;
        }
    }
}