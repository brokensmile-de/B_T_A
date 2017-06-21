using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
	public class ProjectileGun : BaseGun
	{
	    private float _curSpread;

		private float _ammo;

	    public ProjectileGun(WeaponController wp, GunController gc) : base(wp, gc)
	    {
			Start ();
	        Reset();
	    }

	    protected override void Reset()
	    {
	        _curSpread = WeaponController.BulletBaseSpread;
	    }
		void Start(){
			_ammo = WeaponController.AmmoPerPickUp;
		}

	    protected override void Shoot()
	    {
			//ammo check
			if (_ammo < 1f && this.WeaponController.Id != 0) {
				GunController.EmptyAmmo ();
			} else {
				var baseRotation = Quaternion.identity;
				if (WeaponController.BulletsPerShot > 1) {
					// Multiple bullets per shot e.g. Shotgun
					var bulletDistance = _curSpread / WeaponController.BulletsPerShot;
					var rotation = baseRotation * Quaternion.Euler (0f, -_curSpread / 2, 0f);
					for (int i = 0; i < WeaponController.BulletsPerShot; i++) {
						GunController.CmdFire (rotation, WeaponController.BulletSpeed, WeaponController.MaxShotDistance);

						rotation *= Quaternion.Euler (0f, bulletDistance, 0f);
                    
					}
					_ammo -= 1;
				} else {
					// Single bullet per shot
					var spread = Random.Range (-_curSpread / 2, _curSpread / 2);
					var rotation = baseRotation * Quaternion.Euler (0f, spread, 0f);
	            
					GunController.CmdFire (rotation, WeaponController.BulletSpeed, WeaponController.MaxShotDistance);
					_ammo -= 1;
				}
				_curSpread = Math.Min (WeaponController.BulletSpreadIncrease + _curSpread, WeaponController.BulletMaxSpread);
			}
	    }
	}
}

