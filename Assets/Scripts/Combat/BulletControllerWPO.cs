//using UnityEngine;

//namespace Combat
//{
//	public class BulletController : MonoBehaviour
//	{
        
//        public GameObject instantiator;
//        [SerializeField]
//        private int _damage;
//	    public int Damage
//	    {
//	        get { return _damage; }
//	        set { _damage = value; }
//	    }

//        public void setInstantiator(GameObject obj)
//        {
//            instantiator = obj;
//        }

//        void OnCollisionEnter(Collision collision)
//        {
            
//            GameObject hit = collision.gameObject;
//            HitpointsWPO enemyHealth = hit.GetComponent<HitpointsWPO>();
//            PowerUpCountDown countdown = instantiator.GetComponent<PowerUpCountDown>();
//            HitpointsWPO instantiatorHealth = instantiator.GetComponent<HitpointsWPO>();
            

//            if (enemyHealth != null)
//            {
//                enemyHealth.ApplyDamage(Damage, gameObject);
                
//                if (countdown.hasPowerUp&& instantiatorHealth.hasVampire)
//                {
//                    Debug.Log("VampirePowerUpinstantiatorHealth: " + instantiatorHealth + "hit: " +hit);
//                    instantiatorHealth.ApplyHeal(5,gameObject);

//                }

//                if(countdown.hasPowerUp&& instantiatorHealth.hasDoubleDam)
//                {
//                    Debug.Log("hit doubleDamage");
//                    enemyHealth.ApplyDamage(Damage * 2, gameObject);
//                }
                
//            }

//            //countdown.hasPowerUp

//            Debug.Log("Player " + instantiator + " hit Player " + hit);
           

//            Destroy(gameObject);
//        }

//	}
//}

