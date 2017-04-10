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

		public GameObject myPistol;


		// Use this for initialization
		void Start () {
			hasGun = false;
			// if you want to start with a weapon, instantiate here and set has gun to true
		}

		// Update is called once per frame
		void Update () {
			if (isFiring && hasGun) {

				currentGun.Shoot();

			}
		}

		public void ChangeGun(int i){
			
			if (i == 0) {
				// destroy current gun if you have other
				if (currentGun != null) {
					Destroy (myPistol.gameObject);
				}
				currentGun = new ProjectileGun(new Pistol(), this);
				hasGun = true;
				Instantiate (myPistol, gunHolder.position, gunHolder.rotation).transform.parent = gunHolder;
			}	
			// add new guns here

		}
	}
}

