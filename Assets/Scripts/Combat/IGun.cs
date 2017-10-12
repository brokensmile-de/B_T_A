using System;
using UnityEngine;

namespace Combat
{
	public interface IGun
	{
		void Shoot(Boolean autofire, Vector3 bulletSpawnPoint);
	    void Update();
		WeaponController GetWeaponController();
	}
}

