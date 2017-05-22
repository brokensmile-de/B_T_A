using AssemblyCSharp;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Combat
{
	public class GunController : NetworkBehaviour
	{
        public Transform FirePoint;
        public Transform GunHolder;

        public IGun CurrentGun { get; private set; }

		//Esteban --- Model GameObjects

		//Player movement für animation
		public PlayerMovement player;

		//Current Gun GameObject

		private GameObject currentGun;

		//Assault Rifle 
		public GameObject AssaultRifleOb;
		//Rail Gun
		public GameObject RailGunOb;
		//Pistol
		public GameObject PistolOb;
		//Shotgun
		public GameObject ShotgunOb;


        [SerializeField]
        private GameObject _bullet;
        public GameObject Bullet
        {
            get { return _bullet; }
            set { _bullet = value; }
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
                    //CmdFire();
	            }
	        }

	        // Temporarily switch weapons using Number Keys
	        if (Input.GetKeyDown("1"))
	        {
	            ChangeGun(Weapons.Templates.Pistol.CreateGun(this));
				player.HasPistolAnim ();
	        }
	        else if (Input.GetKeyDown("2"))
	        {
	            ChangeGun(Weapons.Templates.Shotgun.CreateGun(this));
				player.HasNoPistolAnim ();
	        }
	        else if (Input.GetKeyDown("3"))
	        {
	            ChangeGun(Weapons.Templates.AssaultRifle.CreateGun(this));
				player.HasNoPistolAnim ();
	        }
	        else if (Input.GetKeyDown("4"))
	        {
	            ChangeGun(Weapons.Templates.Railgun.CreateGun(this));
				player.HasNoPistolAnim ();
	        }
	    }


        [Command]
        public void CmdFire(Quaternion rotation, float speed, float maxDistance)
        {
            


            var newBullet = GameObject.Instantiate(Bullet, FirePoint.position, rotation);

            Debug.Log(newBullet + " newBullet");

            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * speed;


            //var bulletController = newBullet.GetComponent<BulletController>();
            //bulletController.MaxDistance =maxDistance;


            NetworkServer.Spawn(newBullet);

            Destroy(newBullet, 2f);
        }

	    void Start ()
	    {

            if (!isLocalPlayer)
            {
                return;
            }

			//Esteban --- Player beginnt ohne Waffe
            //ChangeGun(Weapons.Templates.Pistol.CreateGun(this));
	    }

	    public void ChangeGun(IGun gun)
	    {
	        CurrentGun = gun;

            //direct assignment saves headaches
	        //FirePoint = transform.Find("FirePoint");
	        //GunHolder = transform.Find("GunHolder");
	    }


		//Esteban ---- diese Methode wird von CollisionDetector angerufen.
		public void PickGun(int i){
			if (i == 1) {
				player.HasNoPistolAnim ();
				ChangeGun(Weapons.Templates.AssaultRifle.CreateGun(this));
				//Model in gunHolder erzeugen
				if(currentGun != null){
					DestroyObject (currentGun);
				}
				currentGun = Instantiate (AssaultRifleOb, GunHolder.position, GunHolder.rotation);
				currentGun.transform.parent = GunHolder;
			}
			else if (i == 2) {
				player.HasPistolAnim ();
				ChangeGun(Weapons.Templates.Pistol.CreateGun(this));
				//Model in gunHolder erzeugen
				if(currentGun != null){
					DestroyObject (currentGun);
				}
				currentGun = Instantiate (PistolOb, GunHolder.position, GunHolder.rotation);
				currentGun.transform.parent = GunHolder;
			}else if (i == 3) {
				player.HasNoPistolAnim ();
				ChangeGun(Weapons.Templates.Shotgun.CreateGun(this));
				//Model in gunHolder erzeugen
				if(currentGun != null){
					DestroyObject (currentGun);
				}
				currentGun = Instantiate (ShotgunOb, GunHolder.position, GunHolder.rotation);
				currentGun.transform.parent = GunHolder;
			}
			else if (i == 4) {
				player.HasNoPistolAnim ();
				ChangeGun(Weapons.Templates.Railgun.CreateGun(this));
				//Model in gunHolder erzeugen
				if(currentGun != null){
					DestroyObject (currentGun);
				}
				currentGun = Instantiate (RailGunOb, GunHolder.position, GunHolder.rotation);
				currentGun.transform.parent = GunHolder;
			}
		}
	}
}

