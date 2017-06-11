namespace Combat.Weapons
{
	public class Pistol : ProjectileWeaponTemplate
	{
		public Pistol ()
		{
		    Cooldown = 0.5f;
		    BulletSpeed = 100f;
		    MaxShotDistance = 50f;
			//Esteban
			AmmoPerPickUp = 15.0f;
		}

	    public override IGun CreateGun(GunController controller)
	    {
	        return new ProjectileGun(this, controller);
	    }
	}
}

