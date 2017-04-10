using System;

namespace AssemblyCSharp
{
	public class Pistol : WeaponTemplate
	{
		public Pistol ()
		{
			BulletSpeed = 20f;
			TimeBetweenShots = 0.05f;
			ShotCounter = TimeBetweenShots;
			MaxBulletDistance = 20f;

		}
	}
}

