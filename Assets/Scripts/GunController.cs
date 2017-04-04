using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp{
	public class GunController : MonoBehaviour {

		public bool isFiring;

		public BulletController bullet;

		private float shotCounter;

		public Transform firePoint;
		//-----
		public Transform gunHolder;

		public IGun currentGun;

		private bool hasGun;


		// Use this for initialization
		void Start () {
			hasGun = true;

			currentGun = new ProjectileGun(new Pistol(), this);
		}

		// Update is called once per frame
		void Update () {
			if (isFiring && hasGun) {

				currentGun.Shoot();

			}
		}

		public void ChangeGun(int i){
			
	

		}
	}
}

