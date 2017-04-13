using System;
using UnityEngine;

namespace Combat
{
	public class GunController : MonoBehaviour
	{
	    public Transform FirePoint { get; private set; }
	    public Transform GunHolder { get; private set; }

	    public IGun CurrentGun { get; private set; }

	    [SerializeField]
	    private WeaponController[] _weaponst;
	    public WeaponController[] Weapons
	    {
	        get { return _weaponst; }
	        set { _weaponst = value; }
	    }

	    void Update ()
	    {
	        if (CurrentGun != null)
	        {
	            CurrentGun.Update();

	            if (Input.GetMouseButton(0))
	            {
	                CurrentGun.Shoot(!Input.GetMouseButtonDown(0));
	            }
	        }

	        for (var i = 1; i <= 9; i++)
	        {
	            if (Input.GetKeyDown(i.ToString()) && Weapons.Length >= i)
	            {
	                ChangeGun(Weapons[i - 1]);
	            }
	        }
	    }

	    public void ChangeGun(WeaponController weapon)
	    {
	        IGun gun = null;

	        switch (weapon.WeaponType)
	        {
	            case WeaponType.Projectile: gun = new ProjectileGun(weapon, this); break;
	            case WeaponType.HitScan: gun = new HitscanGun(weapon, this); break;
	        }

	        CurrentGun = gun;
	        FirePoint = transform.Find("FirePoint");
	        GunHolder = transform.Find("GunHolder");
	    }
	}
}

