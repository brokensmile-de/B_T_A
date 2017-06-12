using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
	public class ProjectileGun : BaseGun
	{
	    private float _curSpread;

	    public ProjectileGun(WeaponController wp, GunController gc) : base(wp, gc)
	    {
	        Reset();
	    }

	    protected override void Reset()
	    {
	        _curSpread = WeaponController.BulletBaseSpread;
	    }

	    protected override void Shoot()
	    {
	        if (WeaponController.BulletsPerShot > 1)
	        {
	            // Multiple bullets per shot e.g. Shotgun
	            var bulletDistance = _curSpread / WeaponController.BulletsPerShot;
	            var rotation = GunController.FirePoint.rotation * Quaternion.Euler(0f, -_curSpread/2, 0f);
	            for (int i = 0; i < WeaponController.BulletsPerShot; i++)
	            {
                    GunController.CmdFire(rotation, WeaponController.BulletSpeed, WeaponController.MaxShotDistance);

	                rotation *= Quaternion.Euler(0f, bulletDistance, 0f);
                    
	            }
	        }
	        else
	        {
	            // Single bullet per shot
	            var spread = Random.Range(-_curSpread/2, _curSpread/2);
	            var rotation = GunController.FirePoint.rotation * Quaternion.Euler(0f, spread, 0f);
	            
	            GunController.CmdFire(rotation, WeaponController.BulletSpeed, WeaponController.MaxShotDistance);
	       }
	        _curSpread = Math.Min(WeaponController.BulletSpreadIncrease + _curSpread, WeaponController.BulletMaxSpread);

	    }
	}
}

