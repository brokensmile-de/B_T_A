namespace Combat.Weapons
{
	public class Pistol : ProjectileWeaponTemplate
	{
		public Pistol ()
		{
		    Cooldown = 0.5f;
		    BulletSpeed = 100f;
		    MaxShotDistance = 50f;
		}

	    public override IGun CreateGun(GunController controller)
	    {
	        return new ProjectileGun(this, controller);
	    }
	}
}

