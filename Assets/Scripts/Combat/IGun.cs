using System;

namespace Combat
{
	public interface IGun
	{
		void Shoot(Boolean autofire);
	    void Update();
		WeaponController GetWeaponController();
	}
}

