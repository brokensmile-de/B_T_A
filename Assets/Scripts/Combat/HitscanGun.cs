using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
	public class HitscanGun : BaseGun
	{
	    public HitscanGun(WeaponController wp, GunController gc) : base(wp, gc)
	    {
	    }

	    protected override void Reset()
	    {
	    }


		protected override void Shoot()
	    {
		    GunController.CmdHitscan(WeaponController.MaxShotDistance);
	    }
	}
}

