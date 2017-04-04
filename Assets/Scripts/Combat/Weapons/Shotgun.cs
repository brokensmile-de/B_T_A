namespace Combat.Weapons
{
	public class Shotgun : ProjectileWeaponTemplate
	{
		public Shotgun ()
		{
		    BulletsPerShot = 10;
		    BulletBaseSpread = 60.0f;
		    BulletMaxSpread = BulletBaseSpread;
		}

	    public override IGun CreateGun(GunController controller)
	    {
	        return new ProjectileGun(this, controller);
	    }
	}
}

