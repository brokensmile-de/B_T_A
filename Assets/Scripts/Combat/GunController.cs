using UnityEngine;
using UnityEngine.Networking;

namespace Combat
{
	public class GunController : NetworkBehaviour
	{
		public Transform FirePoint;
		public Transform GunHolder;
        public AudioSource audioSource;

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
        [SyncVar]
		private Transform gunModel;
        bool first = false;
	    void Update ()
	    {
            if (!isLocalPlayer)
            {
                return;
            }

            if (CurrentGun != null)
	        {
	            CurrentGun.Update();

	            if (Input.GetMouseButton(0) && !Timer.singleton.isGameOver)
	            {
	                CurrentGun.Shoot(!Input.GetMouseButtonDown(0));
	            }
	        }

            if (!first)
                Invoke("StartGun", 0.1f);
            first = true;

            for (var i = 1; i <= 9; i++)
	        {
	            if (Input.GetKeyDown(i.ToString()) && Weapons.Length >= i)
	            {
	                ChangeGun(i-1);
	            }
	        }
	    }

        private void StartGun()
        {
            PickGun(0);
        }

        [Command]
        public void CmdFire(Quaternion rotation, float speed, float maxDistance)
        {
            WeaponController currentGun = transform.root.gameObject.GetComponent<GunController>().CurrentGun.GetWeaponController();

            var bullet = currentGun.Bullet.gameObject;
            var newBullet = Instantiate(bullet, FirePoint.position, rotation);

            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * speed;
            newBullet.GetComponent<BulletController>().spawnedBy = transform.root.gameObject.GetComponent<NetworkIdentity>().netId;
            NetworkServer.Spawn(newBullet);
            RpcFire(currentGun.Id);
        }

        [ClientRpc]
        void RpcFire(int i)
        {
            audioSource.PlayOneShot(Weapons[i].Sound);
        }


        [Command]
        void CmdChangeGun(int i)
        {
            IGun gun = null;

            switch (Weapons[i].WeaponType)
            {
                case WeaponType.Projectile: gun = new ProjectileGun(Weapons[i], this); break;
                case WeaponType.HitScan: gun = new HitscanGun(Weapons[i], this); break;
            }

            CurrentGun = gun;

            if (gunModel != null)
            {
                DestroyObject(gunModel.gameObject);
                gunModel = null;
            }

            gunModel = Instantiate(Weapons[i].Model, GunHolder.position, GunHolder.rotation);
            //gunModel.transform.parent = GunHolder;
            NetworkServer.Spawn(gunModel.gameObject);
            RpcChangeWeapon(gunModel.gameObject);
        }

        [ClientRpc]
        void RpcChangeWeapon(GameObject obj)
        { 
            obj.transform.parent = GunHolder;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        public void ChangeGun(int i)
	    {
	        IGun gun = null;

	        switch (Weapons[i].WeaponType)
	        {
	            case WeaponType.Projectile: gun = new ProjectileGun(Weapons[i], this); break;
	            case WeaponType.HitScan: gun = new HitscanGun(Weapons[i], this); break;
	        }
            CurrentGun = gun;

            if (Weapons[i].gameObject.name == "Pistol")
            {
                player.HasPistolAnim();
            }
            else
            {
                player.HasNoPistolAnim();
            }

            CmdChangeGun(i);
	    }

		//Esteban ---- diese Methode wird von CollisionDetector angerufen.
		public void PickGun(int i){
			if (i >= 0 && i < Weapons.Length)
			{
				ChangeGun(i);
			}
		}
	}
}

