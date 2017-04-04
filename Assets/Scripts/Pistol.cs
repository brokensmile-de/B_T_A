using System;

namespace AssemblyCSharp
{
	public class Pistol : WeaponTemplate
	{
		public Pistol ()
		{
			BulletSpeed = 20f;
			TimeBetweenShots = 0.5f;
			ShotCounter = TimeBetweenShots;
			MaxBulletDistance = 50f;

		}
	}
}

