using System;

namespace AssemblyCSharp
{
	public interface IGun
	{
		void Shoot();

		WeaponTemplate GetGunTemplate();

	}
}

