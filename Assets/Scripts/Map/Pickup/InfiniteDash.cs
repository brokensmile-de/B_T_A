using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteDash : Pickup {

    public static float durationStatic;
    [SerializeField]
    private float duration = 3;

    public override void Start()
    {
        base.Start();
        durationStatic = duration;
    }

    protected override void Use(Collider other)
    {
        PlayerMovement mov = other.GetComponent<PlayerMovement>();
        mov.HasInfiniteDash = true;
    }

}
