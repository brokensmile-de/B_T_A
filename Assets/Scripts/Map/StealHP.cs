

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;



//public class StealHP : Combat.BulletController
//{
    
//    public bool hasDoubleDamagePowerUp;
//    public int doubleDamageAmount;

//    void OnCollisionEnter(Collision collision)
//    {
//        GameObject hit = collision.gameObject;
//        Hitpoints enemyHealth = hit.GetComponent<Hitpoints>();


//        if (enemyHealth != null)
//        {
            
//            enemyHealth.ApplyDamage(Damage, gameObject);
//            hasVampirePowerUp=true;
//        }

        

//        Destroy(gameObject);
//        hasVampirePowerUp = false;
//    }

    

//}