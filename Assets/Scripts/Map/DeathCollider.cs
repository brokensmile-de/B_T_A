using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Hitpoints hp = other.GetComponent<Hitpoints>();
        if (hp)
            hp.TakeDamage(1000, null);
    }
}
