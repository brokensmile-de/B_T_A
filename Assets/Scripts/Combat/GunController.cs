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
	    private Transform _bullet;
	    public Transform Bullet
	    {
	        get { return _bullet; }
	        set { _bullet = value; }
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

	        // Temporarily switch weapons using Number Keys
	        if (Input.GetKeyDown("1"))
	        {
	            ChangeGun(Weapons.Templates.Pistol.CreateGun(this));
	        }
	        else if (Input.GetKeyDown("2"))
	        {
	            ChangeGun(Weapons.Templates.Shotgun.CreateGun(this));
	        }
	        else if (Input.GetKeyDown("3"))
	        {
	            ChangeGun(Weapons.Templates.AssaultRifle.CreateGun(this));
	        }
	        else if (Input.GetKeyDown("4"))
	        {
	            ChangeGun(Weapons.Templates.Railgun.CreateGun(this));
	        }
	    }

	    void Start ()
	    {
	        ChangeGun(Weapons.Templates.Pistol.CreateGun(this));
	    }

	    public void ChangeGun(IGun gun)
	    {
	        CurrentGun = gun;
	        FirePoint = transform.Find("FirePoint");
	        GunHolder = transform.Find("GunHolder");
	    }
	}
}

