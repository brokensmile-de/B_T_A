using System;
using UnityEngine;

namespace Combat
{
    public class WeaponController : MonoBehaviour {
        public float Cooldown = 0f;
        public float MaxShotDistance = 50f;
        public Boolean AutoFire = false;
        public float ResetCoolown = 0f;
        public float BulletSpeed = 50f;
        public int BulletsPerShot = 1;
        public float BulletBaseSpread = 0f;
        public float BulletMaxSpread = 0f;
        public float BulletSpreadIncrease = 0f;
        public BulletController Bullet = null;
        public Transform Model = null;
        public WeaponType WeaponType = WeaponType.Projectile;
		//Esteban-- ammo
		public float AmmoPerPickUp = 10f;

    }
}
