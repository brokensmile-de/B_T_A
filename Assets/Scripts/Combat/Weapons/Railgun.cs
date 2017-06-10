
namespace Combat.Weapons
{
	public class Railgun : WeaponTemplate
	{
		public Railgun ()
		{
		    Cooldown = 2f;
		    MaxShotDistance = 100f;
		}

	    public override IGun CreateGun(GunController controller)
	    {
	        return new HitscanGun(this, controller);
	    }
	}
}

