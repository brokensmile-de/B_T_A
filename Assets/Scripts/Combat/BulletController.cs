using UnityEngine;

namespace Combat
{
	public class BulletController : MonoBehaviour
	{
	    [SerializeField]
	    private int _damage;
	    public int Damage
	    {
	        get { return _damage; }
	        set { _damage = value; }
	    }

	    void OnCollisionEnter(Collision collision)
        {
            GameObject hit = collision.gameObject;
            Hitpoints health = hit.GetComponent<Hitpoints>();


            if (health != null)
            {
                health.ApplyDamage(Damage, gameObject);
            }

            Destroy(gameObject);
        }

	}
}

