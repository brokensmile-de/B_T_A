using UnityEngine;

namespace Combat
{
	public class BulletController : MonoBehaviour
	{
        
        public GameObject instantiator;
        [SerializeField]
        public bool hasVampirePowerUp;
        private int _damage;
	    public int Damage
	    {
	        get { return _damage; }
	        set { _damage = value; }
	    }

        public void setInstantiator(GameObject obj)
        {
            instantiator = obj;
        }

        void OnCollisionEnter(Collision collision)
        {
            GameObject hit = collision.gameObject;
            Hitpoints health = hit.GetComponent<Hitpoints>();


            if (health != null)
            {
                health.ApplyDamage(Damage, gameObject);
                //hasVampirePowerUp = true;
            }

            Debug.Log("Player " + instantiator + " hit Player " + hit);
           

            Destroy(gameObject);
        }

	}
}

