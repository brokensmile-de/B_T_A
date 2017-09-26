using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShield : Pickup{

    public static float durationStatic;
    [SerializeField]
    private float duration = 10;

    public override void Start()
    {
        base.Start();
        durationStatic = duration;
    }

    protected override void Use(Collider other)
    {
        Hitpoints hp = other.GetComponent<Hitpoints>();
        hp.HasDoubleShield = true;
    }
}
