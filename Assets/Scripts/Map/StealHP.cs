

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StealHP : Combat.BulletController
{


    public int doubleDamageAmount;
    public int healAmount;
    public bool hasDoubleDamagePowerUp;


    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Hitpoints enemyHealth = hit.GetComponent<Hitpoints>();


        if (enemyHealth != null)
        {
            
            enemyHealth.ApplyDamage(Damage, gameObject);
            hasVampirePowerUp=true;
        }
        
        

        Destroy(gameObject);
        hasVampirePowerUp = false;
    }

    

}