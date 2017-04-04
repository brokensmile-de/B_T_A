using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ProjectileGun : IGun
	{
		private WeaponTemplate myTemp;

		private float shotCounter;

		private GunController gunController;


		public ProjectileGun (WeaponTemplate wp, GunController gc)
		{
			this.myTemp = wp;

			this.gunController = gc;
			
		}

		public void Shoot(){
			

				shotCounter -= Time.deltaTime;
			if (shotCounter <= 0) {

				shotCounter = myTemp.TimeBetweenShots;
				BulletController newBullet = GameObject.Instantiate (gunController.bullet, gunController.firePoint.position, gunController.firePoint.rotation) as BulletController;
				newBullet.speed = myTemp.BulletSpeed;
			}

			
		}
		public WeaponTemplate GetGunTemplate(){
			return this.myTemp;
		}
	}
}

