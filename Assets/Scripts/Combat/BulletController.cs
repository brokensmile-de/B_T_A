using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BulletController : MonoBehaviour
	{

        public int Damage;
		public float Speed { get; set; }
	    public float MaxDistance { get; set; }

	    private float _distanceTravelled = 0f;

	    public BulletController()
	    {

	    }

        void OnCollisionEnter(Collision collision)
        {

            

            GameObject hit = collision.gameObject;
            Hitpoints health = hit.GetComponent<Hitpoints>();

           

            if (health != null)
            {
                health.ApplyDamage(Damage, this.gameObject);
              
            }


            Destroy(gameObject);
        }


        //rigidbody and time to live make own update method currently obsolete

      //     void Update ()
	  //  {
	  //      var vec = Vector3.forward * Speed * Time.deltaTime;
			//transform.Translate (vec);
	  //      _distanceTravelled += vec.magnitude;

	  //      if (_distanceTravelled > MaxDistance)
	  //      {
	  //          Destroy(gameObject);
	  //      }
	  //  }

	}
}

