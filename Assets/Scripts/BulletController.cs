using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp{
	public class BulletController : MonoBehaviour {

		public float speed;

		//atributes for max distance of bullets

		public float maxDistance;

		private float timePassed;




		// Use this for initialization
		void Start () {
			timePassed = 0f;

		}

		// Update is called once per frame
		void Update () {
			if (!((timePassed * speed) >= maxDistance)) {
				transform.Translate (Vector3.forward * speed * Time.deltaTime);
				timePassed += Time.deltaTime;
			} else {
				Destroy (gameObject);

			}

		}



	}
}

