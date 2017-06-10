using System;
using UnityEngine;

namespace Combat
{
    public abstract class BaseGun: IGun
    {
        public readonly WeaponTemplate WeaponTemplate;
        public readonly GunController GunController;

        private float _shotCounter;
        private float _resetCooldown;

        private Boolean _needReset;

        public BaseGun (WeaponTemplate wp, GunController gc)
        {
            WeaponTemplate = wp;
            GunController = gc;
        }

        public void Shoot(Boolean autofire)
        {
            if (_shotCounter <= 0 && (WeaponTemplate.AutoFire || !autofire)) {
                _shotCounter = WeaponTemplate.Cooldown;
                _needReset = true;
                _resetCooldown = WeaponTemplate.ResetCoolown;

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

        public WeaponTemplate GetGunTemplate()
        {
            return WeaponTemplate;
        }
    }
}