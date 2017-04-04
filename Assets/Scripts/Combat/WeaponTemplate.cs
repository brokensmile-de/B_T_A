using System;

namespace Combat
{
	public abstract class WeaponTemplate
	{
		public float Cooldown { get; protected set; }
		public float MaxShotDistance { get; protected set; }
	    public Boolean AutoFire { get; protected set; }
	    public float ResetCoolown { get; protected set; }

	    public abstract IGun CreateGun(GunController controller);

	    protected WeaponTemplate()
	    {
	        AutoFire = false;
	        Cooldown = 0.0f;
	        MaxShotDistance = 50.0f;
	        ResetCoolown = 0f;
	    }
	}
}

