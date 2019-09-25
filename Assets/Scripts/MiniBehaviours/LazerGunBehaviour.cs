using System.Collections;
using Asteroids;
using DefaultNamespace;
using UnityEngine;

namespace MiniBehaviours
{
    public class LazerGunBehaviour : Weapon
    {
        public Vector2 LaserWidth;
        public float LaserMaxDistance = 50;
        
        private LineRenderer _line;
        private BoxCollider2D _fireArea;
        private AudioSource source;
        public  float disolveProgress;
        private Material disolveMat;
        
        protected Vector2 _destination;

        public Transform muzzle;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            _fireArea = GetComponentInChildren<BoxCollider2D>();
            _line = gameObject.GetComponentInChildren<LineRenderer>();
            _line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            disolveMat = _line.material;
            disolveProgress = disolveMat.GetFloat("Vector1_5CF7729E");
            _line.receiveShadows = true;
            _line.startWidth = LaserWidth.x;
            _line.endWidth = LaserWidth.y;
        }

        /// </summary>
        /// Initialize this weapon
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
        }


        /// <summary>
        /// Called everytime the weapon is used
        /// </summary>
        public override void WeaponUse()
        {
            base.WeaponUse();
            ShootLaser();
            source.Play();
        }

        /// <summary>
        /// Draws the actual laser
        /// </summary>
        protected virtual void ShootLaser()
        {
            // our laser will be shot from the weapon's laser origin
            RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.transform.up);
            

                if (hit.collider != null)
            {
                var enemy = hit.collider.GetComponent<EnemyUnit>();
            }

            // if we've hit something, our destination is the raycast hit
            if (hit.transform != null)
            {
                _destination = (hit.point);
            }
            // otherwise we just draw our laser in front of our weapon 
            else
            {
                _destination = (muzzle.transform.up * LaserMaxDistance);
            }
            var inversed = transform.InverseTransformPoint(_destination);
            
            // we set our fire area 
            _fireArea.size =new Vector2(_fireArea.size.x, inversed.y);
            _fireArea.offset = new Vector2(_fireArea.offset.x, _fireArea.size.y/2);

            // we set our laser's line's 
            _line.SetPosition(0, muzzle.transform.position);
            _line.SetPosition(1, new Vector3(_destination.x, _destination.y, muzzle.transform.position.z));
        }

        public override void CaseWeaponUse()
        {
            base.CaseWeaponUse();
            _line.enabled = true;
            OnAmmoChanged();
            StartCoroutine(LaserDisableTimer());
        }


        IEnumerator LaserDisableTimer()
        {
            _fireArea.enabled = true;
            var fireAreaOldPos = _fireArea.transform.localPosition;
            var fireAreaOldRot = _fireArea.transform.localRotation;
            _fireArea.transform.parent = null;
            
            while (disolveProgress < 1)
            {
                disolveProgress+=0.05f;
                disolveMat.SetFloat("Vector1_5CF7729E", disolveProgress);
                yield return null;
            }
            _fireArea.enabled = false;
            _fireArea.transform.parent = transform;
            _fireArea.transform.localPosition = fireAreaOldPos;
            _fireArea.transform.localRotation = fireAreaOldRot;
            _line.enabled = false;
            disolveProgress = 0;
        }

    }
}