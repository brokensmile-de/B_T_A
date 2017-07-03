using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Player
{
	public class LockRotation : MonoBehaviour
	{
		private Quaternion _fixedRotation;

		public Vector3 Rotation;
		
		private void Awake()
		{
			_fixedRotation = Quaternion.Euler(Rotation);
		}

		private void LateUpdate()
		{
			transform.rotation = _fixedRotation;
		}
	}
}
