using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BulletController : MonoBehaviour
	{
		public float Speed { get; set; }
	    public float MaxDistance { get; set; }

	    private float _distanceTravelled = 0f;

	    public BulletController()
	    {

	    }

        void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }


        void Update ()
	    {
	        var vec = Vector3.forward * Speed * Time.deltaTime;
			transform.Translate (vec);
	        _distanceTravelled += vec.magnitude;

	        if (_distanceTravelled > MaxDistance)
	        {
	            Destroy(gameObject);
	        }
	    }

	}
}

