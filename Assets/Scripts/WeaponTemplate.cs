using System;

namespace AssemblyCSharp
{
	public abstract class WeaponTemplate
	{
		public float BulletSpeed{ get; protected set; }

		public float TimeBetweenShots{ get; protected set; }

		public float ShotCounter{ get; protected set; }

		public float MaxBulletDistance{ get; protected set; }



		public WeaponTemplate ()
		{
			

		}
	}
}

