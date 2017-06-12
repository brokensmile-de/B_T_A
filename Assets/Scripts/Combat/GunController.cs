using UnityEngine;
using UnityEngine.Networking;

namespace Combat
{
	public class GunController : NetworkBehaviour
	{
		public Transform FirePoint;
		public Transform GunHolder;

	    public IGun CurrentGun { get; private set; }

	    [SerializeField]
	    private WeaponController[] _weapons;
	    public WeaponController[] Weapons
	    {
	        get { return _weapons; }
	        set { _weapons = value; }
	    }
	    
        //Esteban --- Model GameObjects

        //Player movement für animation
        public PlayerMovement player;

        //Current Gun model
		private Transform gunModel;

		void Start(){
		
			if (!isLocalPlayer) {
				return;
			}
			ChangeGun (Weapons [0]);
		
		}

	    void Update ()
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

			//Numpad gun change
	       /* for (var i = 1; i <= 9; i++)
	        {
	            if (Input.GetKeyDown(i.ToString()) && Weapons.Length >= i)
	            {
	                ChangeGun(Weapons[i - 1]);
	            }
	        }*/
	    }


        [Command]
        public void CmdFire(Quaternion rotation, float speed, float maxDistance)
        {
	        var bullet = CurrentGun.GetWeaponController().Bullet.gameObject;
            var newBullet = Instantiate(bullet, FirePoint.position, rotation);

            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * speed;
            newBullet.GetComponent<BulletController>().spawnedBy = transform.root.gameObject.GetComponent<NetworkIdentity>().netId;
            NetworkServer.Spawn(newBullet);
            //Destroy(newBullet, maxDistance / speed);
        }

	    public void ChangeGun(WeaponController weapon)
	    {
	        IGun gun = null;

	        switch (weapon.WeaponType)
	        {
	            case WeaponType.Projectile: gun = new ProjectileGun(weapon, this); break;
	            case WeaponType.HitScan: gun = new HitscanGun(weapon, this); break;
	        }
		    
		    if(gunModel != null){
				DestroyObject (gunModel.gameObject);
			    gunModel = null;
		    }

		    if (weapon.gameObject.name == "Pistol")
		    {
			    player.HasPistolAnim();
		    }
		    else
		    {
			    player.HasNoPistolAnim();
		    }
		    
		    
		    gunModel = Instantiate (weapon.Model, GunHolder.position, GunHolder.rotation);
		    gunModel.transform.parent = GunHolder;

	        CurrentGun = gun;
	    }


		//Esteban ---- diese Methode wird von CollisionDetector angerufen.
		public void PickGun(int i){
			if (i >= 0 && i < Weapons.Length)
			{
				ChangeGun(Weapons[i]);
			}
		}

		//Esteban ---Ammo -- wird von WeaponController angerufen
		public void emptyAmmo(){
			//Change back to Pistol
			ChangeGun (Weapons [0]);
		}

	}
}

