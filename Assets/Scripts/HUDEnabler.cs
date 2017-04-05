using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Weist dem Lokalen Player Character während der runtime die Kamera zu
public class HUDEnabler : NetworkBehaviour
{
    public GameObject hudCanvas;
    

    void Start()
    {
        
        //damit tritt das Script nur beim local player in Kraft( Ansonsten würde es die anderen Player auch beeinflussen)
        if (!isLocalPlayer)
        {
            hudCanvas.active = false;
            return;
        }

        
    }


}