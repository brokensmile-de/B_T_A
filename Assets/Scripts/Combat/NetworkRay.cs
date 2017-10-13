using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkRay : NetworkBehaviour
{
    [SyncVar]
    public Vector3 scale;
    //[SyncVar]
    //public Quaternion rotation;
    [SyncVar]
    public Vector3 up;

    void Start()
    {
        transform.up = up;
        transform.localScale = scale;
    }
}