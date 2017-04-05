using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Weist dem Lokalen Player Character während der runtime den HUD zu
public class AttachLocalHud : NetworkBehaviour
{
    private GameObject hudObject;
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

        hudObject = GameObject.FindGameObjectWithTag("LocalHUD");
        hitpointScript = GetComponent<Hitpoints>();

        //fetching references for Hitpoints script
        //playerHealthReference = hudObject.Find("HealthUI").Find("Hitpoints");
        //playerShieldReference =hudObject.Find("HealthUI").Find("Shield");

        playerHealthReference = GameObject.Find("Hitpoints");
        playerShieldReference = GameObject.Find("Shield");

        //assigning them
        hitpointScript.setHealthReference(playerHealthReference);
        hitpointScript.setShieldReference(playerShieldReference);
    }


}
