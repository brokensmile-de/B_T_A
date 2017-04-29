using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Combat
{
	public class GunController : NetworkBehaviour
	{
	    public IGun CurrentGun { get; private set; }

	    [SerializeField]
	    private Transform _firePoint;
	    public Transform FirePoint
	    {
	        get { return _firePoint; }
	        set { _firePoint = value; }
	    }

	    [SerializeField]
	    private Transform _gunHolder;
	    public Transform GunHolder
	    {
	        get { return _gunHolder; }
	        set { _gunHolder = value; }
	    }

	    [SerializeField]
	    private GameObject _bullet;
	    public GameObject Bullet
	    {
	        get { return _bullet; }
	        set { _bullet = value; }
	    }

	    [SerializeField]
	    private WeaponController[] _weapons;
	    public WeaponController[] Weapons
	    {
	        get { return _weapons; }
	        set { _weapons = value; }
	    }

	    public void Update()
	    {
            if (!isLocalPlayer)
            {
                return;
            }

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


	    [Command]
        public void CmdFire(Quaternion rotation, float speed, float maxDistance)
	    {
            var newBullet = Instantiate(_bullet, FirePoint.position, rotation);

            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * speed;

            NetworkServer.Spawn(newBullet);

            Destroy(newBullet, maxDistance / speed);
        }

	    private void ChangeGun(WeaponController weaponController)
	    {
	        IGun gun = null;

	        switch (weaponController.WeaponType)
	        {
	            case WeaponType.Projectile: gun = new ProjectileGun(weaponController, this); break;
	            case WeaponType.HitScan: gun = new HitscanGun(weaponController, this); break;
	        }
	        CurrentGun = gun;
	    }
	}
}

