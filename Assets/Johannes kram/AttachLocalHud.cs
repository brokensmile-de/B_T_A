using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Weist dem Lokalen Player Character während der runtime den HUD zu
public class AttachLocalHud : NetworkBehaviour
{
    private Hitpoints hitpointScript;
    private GameObject playerHealthReference;
    private GameObject playerShieldReference;

    void Start()
    {

        //damit tritt das Script nur beim local player in Kraft( Ansonsten würde es die anderen Player auch beeinflussen)
        if (!isLocalPlayer)
        {
            return;
        }

        hitpointScript = GetComponent<Hitpoints>();

        playerHealthReference = GameObject.Find("Hitpoints");
        playerShieldReference = GameObject.Find("Shield");

        //assigning them
        hitpointScript.setHealthReference(playerHealthReference);
        hitpointScript.setShieldReference(playerShieldReference);
    }


}
