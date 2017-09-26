using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Pickup
{
    public int healAmount;

    protected override void Use(Collider other)
    {
        Hitpoints hpScript = other.GetComponent<Hitpoints>();
        hpScript.Heal(healAmount);//Apply heal
    }
}
