using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Combat
{
	public class ProjectileGun : BaseGun
	{
	    public ProjectileWeaponTemplate ProjectileWeaponTemplate { get; private set; }

	    private float _curSpread;

		//Esteban --- Ammo
		private float _ammo;

	    public ProjectileGun(WeaponTemplate wp, GunController gc) : base(wp, gc)
	    {
	        if (!(wp is ProjectileWeaponTemplate))
	        {
	            throw new ArgumentException("Trying to intialize a ProjectileWeapon with a non ProjectileWeapon Template");
	        }
	        ProjectileWeaponTemplate = (ProjectileWeaponTemplate) wp;
	        _curSpread = ProjectileWeaponTemplate.BulletBaseSpread;

			//Esteban
			_ammo = ProjectileWeaponTemplate.AmmoPerPickUp;

	    }

	    protected override void Reset()
	    {
	        _curSpread = ProjectileWeaponTemplate.BulletBaseSpread;


	    }

	    protected override void Shoot()
	    {
			//Esteban--- Ammo check
			if (_ammo <= 0.0f) {
				Debug.Log ("Out of ammo");
				GunController.emptyAmmo ();

			} else {
			
				if (ProjectileWeaponTemplate.BulletsPerShot > 1) {
				
					// Multiple bullets per shot e.g. Shotgun
					var bulletDistance = _curSpread / ProjectileWeaponTemplate.BulletsPerShot;
					var rotation = GunController.FirePoint.rotation * Quaternion.Euler (0f, -_curSpread / 2, 0f);
					for (int i = 0; i < ProjectileWeaponTemplate.BulletsPerShot; i++) {

						GunController.CmdFire (rotation, ProjectileWeaponTemplate.BulletSpeed, ProjectileWeaponTemplate.MaxShotDistance);


                    
						//var newBullet = Object.Instantiate(GunController.Bullet, GunController.FirePoint.position, rotation);
						//var bulletController = newBullet.GetComponent<BulletController>();
						//bulletController.Speed = ProjectileWeaponTemplate.BulletSpeed;
						//bulletController.MaxDistance = ProjectileWeaponTemplate.MaxShotDistance;

						rotation *= Quaternion.Euler (0f, bulletDistance, 0f);
					}
					//Esteban
					_ammo -= 1.0f;

				} else {
				
					// Single bullet per shot
					var spread = Random.Range (-_curSpread / 2, _curSpread / 2);
					var rotation = GunController.FirePoint.rotation * Quaternion.Euler (0f, spread, 0f);

					GunController.CmdFire (rotation, ProjectileWeaponTemplate.BulletSpeed, ProjectileWeaponTemplate.MaxShotDistance);

					//Esteban
					_ammo -= 1.0f;

					//var newBullet = Object.Instantiate(GunController.Bullet, GunController.FirePoint.position, rotation);
					//var bulletController = newBullet.GetComponent<BulletController>();
					//bulletController.Speed = ProjectileWeaponTemplate.BulletSpeed;
					//bulletController.MaxDistance = ProjectileWeaponTemplate.MaxShotDistance;
				}
				_curSpread = Math.Min (ProjectileWeaponTemplate.BulletSpreadIncrease + _curSpread, ProjectileWeaponTemplate.BulletMaxSpread);
			}
            //GunController.CmdFire();
	    }
	}
}

