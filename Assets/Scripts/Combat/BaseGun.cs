using System;
using UnityEngine;

namespace Combat
{
    public abstract class BaseGun: IGun
    {
        public readonly WeaponController WeaponController;
        public readonly GunController GunController;

        private float _shotCounter;
        private float _resetCooldown;

        private Boolean _needReset;

        public BaseGun (WeaponController wc, GunController gc)
        {
            WeaponController = wc;
            GunController = gc;
        }

        public void Shoot(Boolean autofire)
        {
            if (_shotCounter <= 0 && (WeaponController.AutoFire || !autofire)) {
                _shotCounter = WeaponController.Cooldown;
                _needReset = true;
                _resetCooldown = WeaponController.ResetCoolown;

                Shoot();
            }
        }

        public void Update()
        {
            _shotCounter -= Time.deltaTime;

            if (_shotCounter <= 0f)
            {
                _resetCooldown -= Time.deltaTime;

                if (_needReset && _resetCooldown <= 0f)
                {
                    _needReset = false;
                    Reset();
                }
            }
        }

        protected abstract void Shoot();

        protected abstract void Reset();

        public WeaponController GetWeaponController()
        {
            return WeaponController;
        }
    }
}