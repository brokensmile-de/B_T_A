using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
	public class HitscanGun : BaseGun
	{
	    public HitscanGun(WeaponController wp, GunController gc) : base(wp, gc)
	    {
            Start();
        }

	    protected override void Reset()
	    {
        }

        void Start()
        {
            _ammo = WeaponController.AmmoPerPickUp;
        }
        protected override void Shoot(Vector3 bulletSpawnPoint)
	    {
           
            
            GunController.CmdHitscan(WeaponController.MaxShotDistance, bulletSpawnPoint);
            _ammo--;
            GunController.ammoText.text = _ammo + "";
            
            if (_ammo < 1f)
            {
                GunController.EmptyAmmo();
            }
 



        }
	}
}

