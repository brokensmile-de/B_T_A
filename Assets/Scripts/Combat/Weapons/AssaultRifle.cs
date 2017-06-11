namespace Combat.Weapons
{
	public class AssaultRifle : ProjectileWeaponTemplate
	{
		public AssaultRifle ()
		{
		    Cooldown = 0.1f;
		    BulletSpeed = 20f;
		    MaxShotDistance = 50f;
		    BulletMaxSpread = 10f;
		    BulletSpreadIncrease = 0.5f;
		    AutoFire = true;
		    ResetCoolown = 2f;
			//Esteban
			AmmoPerPickUp = 30.0f;
		}

	    public override IGun CreateGun(GunController controller)
	    {
	        return new ProjectileGun(this, controller);
	    }
	}
}

