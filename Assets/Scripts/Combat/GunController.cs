using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Combat
{
	public class GunController : NetworkBehaviour
	{
		public Transform GunHolder;
        public PlayerMovement movement;
        public AudioSource audioSource;
        public Text ammoText;
        public Image image;

		public Transform FirePoint
		{
			get { return transform.root.gameObject.GetComponent<GunController>().gunModel.Find("FirePoint"); }
		}

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

        public void Start()
        {
            if(isLocalPlayer)
            {
                ammoText = GameObject.Find("HudCanvas").transform.Find("AmmoHud/Ammo").GetComponent<Text>();
                image = GameObject.Find("HudCanvas").transform.Find("AmmoHud/Image").GetComponent<Image>();
                
            }

        }

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

	            if (Input.GetMouseButton(0) && !Timer.singleton.isGameOver && !movement.movementBlocked)
	            {
	                CurrentGun.Shoot(!Input.GetMouseButtonDown(0), FirePoint.position);    //_xyz
	            }
	        }

            if (!first)
                Invoke("StartGun", 0.3f);
            first = true;
	    }

        private void StartGun()
        {
            PickGun(0);
        }

        [Command]
        public void CmdProjectile(Quaternion rotation, float speed, float maxDistance, Vector3 bulletSpawnPoint)   //_xyz
        {
	        var fwd = GetForwardDirection();
	        
            WeaponController currentGun = transform.root.gameObject.GetComponent<GunController>().CurrentGun.GetWeaponController();

            var bullet = currentGun.Bullet.gameObject;

            var newBullet = Instantiate(bullet, bulletSpawnPoint , fwd * rotation);   //_xyz

            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * speed;
	        var bc = newBullet.GetComponent<BulletController>();
            bc.spawnedBy = transform.root.gameObject.GetComponent<NetworkIdentity>().netId;
	        bc.Damage = currentGun.Damage;
            NetworkServer.Spawn(newBullet);
            RpcFire(currentGun.Id);
            Destroy(newBullet, 1.5f);
        }


		private Quaternion GetForwardDirection()
		{
			var baseRotation = transform.rotation;
			baseRotation.x = 0;
			baseRotation.z = 0;
			var len = Mathf.Sqrt (baseRotation.y * baseRotation.y + baseRotation.w * baseRotation.w);
			baseRotation.y /= len;
			baseRotation.w /= len;

			return baseRotation;
		}


		[Command]
		public void CmdHitscan(float maxDistance, Vector3 bulletSpawnPoint)   //_xyz
        {
			var fwd = GetForwardDirection();
	        
			WeaponController currentGun = transform.root.gameObject.GetComponent<GunController>().CurrentGun.GetWeaponController();

			var ray = currentGun.Ray.gameObject;

			var direction = fwd * Vector3.forward;

			//Debug.DrawRay(GunController.FirePoint.position, fwd * WeaponController.MaxShotDistance, Color.green, 5.0f);
		    
			RaycastHit hit;
			var distance = maxDistance;
			if (Physics.Raycast(bulletSpawnPoint, direction, out hit, maxDistance))    //_xyz
            {
				Hitpoints health = hit.transform.GetComponent<Hitpoints>();

				if (health != null)
				{
					health.TakeDamage(currentGun.Damage, gameObject);
				}
				
				distance = hit.distance;
			}

			var start = bulletSpawnPoint;            //_xyz
            var end = start + direction * distance;
			var width = 1f;
			
			var offset = end - start;
			var position = start + (offset / 2.0f);


			var cylinder = Instantiate(ray, position, Quaternion.identity);
			//cylinder.transform.up = offset;
			/*cylinder.transform.localScale = new Vector3(
				width * cylinder.transform.localScale.x, 
				offset.magnitude / 2.0f * cylinder.transform.localScale.y, 
				width * cylinder.transform.localScale.z
			);*/
            NetworkRay networkRay = cylinder.GetComponent<NetworkRay>();
            networkRay.up = offset;
            networkRay.scale = new Vector3(
                width * cylinder.transform.localScale.x,
                offset.magnitude / 2.0f * cylinder.transform.localScale.y,
                width * cylinder.transform.localScale.z
            );
            NetworkServer.Spawn(cylinder);
			RpcFire(currentGun.Id);
			
			Destroy(cylinder, 1.5f);
		}

		[ClientRpc]
        void RpcFire(int i)
        {
            audioSource.PlayOneShot(Weapons[i].Sound,PersistenceManager.instance.effectVolume);
          
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

            
            if(isLocalPlayer)
            {
                ammoText.text = Weapons[i].AmmoPerPickUp + "";
                image.sprite = Weapons[i].Image;
                image.color = Weapons[i].Color;
                ammoText.color = Weapons[i].Color;
            }

            if (i == 0)
                ammoText.text = "";



            CmdChangeGun(i);
	    }

		public void PickGun(int i){
			if (i >= 0 && i < Weapons.Length)
			{
                if(isLocalPlayer)
				ChangeGun(i);
			}
		}
	
	//Esteban ---- diese Methode wird von ProjectileGun Angerufen.
	public void EmptyAmmo(){
			//wechsel auf pistole wenn ammo leer ist
		ChangeGun(0);
		}
	}
}

