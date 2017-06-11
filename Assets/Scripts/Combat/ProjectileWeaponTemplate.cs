namespace Combat
{
	public abstract class ProjectileWeaponTemplate: WeaponTemplate
	{
		public float BulletSpeed { get; protected set; }
	    public int BulletsPerShot { get; protected set; }
	    public float BulletBaseSpread { get; protected set; }
	    public float BulletMaxSpread { get; protected set; }
	    public float BulletSpreadIncrease { get; protected set; }


	    public ProjectileWeaponTemplate()
	    {
	        BulletsPerShot = 1;
	        BulletBaseSpread = 0f;
	        BulletMaxSpread = 0f;
	        BulletSpreadIncrease = 0f;
	        BulletSpeed = 20.0f;
	    }
	}
}

