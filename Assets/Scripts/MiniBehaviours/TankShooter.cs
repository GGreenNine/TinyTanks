using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace MiniBehaviours
{
    public class TankShooter : MonoBehaviour
    {
        public Weapon[] WeaponInventory;
        public Weapon _currentWeapon;
        private int _currentWeaponId;

        void Awake()
        {
            InitializeWeaponInventory();
        }

        public void InitializeWeaponInventory()
        {
            WeaponInventory = GetComponentsInChildren<Weapon>();
            foreach (var weapon in WeaponInventory)
            {
                weapon.DefineOwner(gameObject.GetComponentInChildren<TowerBehaviour>().gameObject);
                weapon.Initialization();
            }

            SetWeapon(0);
        }

        public void SetWeapon(int weaponId)
        {
            if (weaponId > WeaponInventory.Length)
                return;
            if (_currentWeapon.CurrentState != Weapon.WeaponStates.WeaponIdle)
                return;
            _currentWeapon = WeaponInventory[weaponId];
            _currentWeaponId = weaponId;

            foreach (var item in WeaponInventory.Select((value, i) => new {i, value}))
            {
                if (item.i != _currentWeaponId)
                {
                    item.value.gameObject.SetActive(false);
                    continue;
                }

                item.value.gameObject.SetActive(true);
            }
        }

        private void SwapWeapon()
        {
            _currentWeaponId = _currentWeaponId == WeaponInventory.Length - 1 ? 0 : ++_currentWeaponId;
            SetWeapon(_currentWeaponId);
        }


        void Update()
        {
            if (TankInput.IsShooting()) _currentWeapon.WeaponInputStart();
            if (TankInput.ChangeWeapon()) SwapWeapon();
        }
    }
}