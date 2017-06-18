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
		    var baseRotation = GunController.FirePoint.rotation;
		    baseRotation.x = 0;
		    baseRotation.z = 0;
		    var len = Mathf.Sqrt(baseRotation.y * baseRotation.y + baseRotation.w * baseRotation.w);
		    baseRotation.y /= len;
		    baseRotation.w /= len;
	        if (WeaponController.BulletsPerShot > 1)
	        {
	            // Multiple bullets per shot e.g. Shotgun
	            var bulletDistance = _curSpread / WeaponController.BulletsPerShot;
	            var rotation = baseRotation * Quaternion.Euler(0f, -_curSpread/2, 0f);
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
	            var rotation = baseRotation * Quaternion.Euler(0f, spread, 0f);
	            
	            GunController.CmdFire(rotation, WeaponController.BulletSpeed, WeaponController.MaxShotDistance);
	       }
	        _curSpread = Math.Min(WeaponController.BulletSpreadIncrease + _curSpread, WeaponController.BulletMaxSpread);

	    }
	}
}

