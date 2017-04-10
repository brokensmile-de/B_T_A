using UnityEngine;

namespace Combat
{
	public class HitscanGun : BaseGun
	{
	    public HitscanGun(WeaponTemplate wp, GunController gc) : base(wp, gc)
	    {
	    }

	    protected override void Reset()
	    {
	    }

	    protected override void Shoot()
	    {
	        var fwd = GunController.FirePoint.TransformDirection(Vector3.forward);

	        Debug.DrawRay(GunController.FirePoint.position, fwd * WeaponTemplate.MaxShotDistance, Color.green, 5.0f);

	        RaycastHit hit;
	        if (Physics.Raycast(GunController.FirePoint.position, fwd, out hit, WeaponTemplate.MaxShotDistance))
	        {
	            Debug.Log("Raycast hit: " + hit.collider);
	        }
	    }
	}
}

