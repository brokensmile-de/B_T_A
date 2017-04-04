using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class PlayerGunController : MonoBehaviour
	{
		public GunController gun;

		public PlayerGunController ()
		{
		}

		void Update() {
			if (Input.GetMouseButtonDown (0)) {
				gun.isFiring = true;
			}
			if (Input.GetMouseButtonUp(0)){
				gun.isFiring = false;
			}
		}
			
		void OnTriggerEnter(Collider other){

		}
	}
}

