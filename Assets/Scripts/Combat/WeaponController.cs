using System;
using AssemblyCSharp;
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
        public Transform Bullet = null;
        public WeaponType WeaponType = WeaponType.Projectile;
    }
}
