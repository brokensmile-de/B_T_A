using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponPickup : NetworkBehaviour {
    public int weaponId;
    // Use this for initialization
    [SyncVar]
    private bool active = true;
    public float cooldown;
    public GameObject mesh;
    public AudioSource sound;


    void OnTriggerEnter(Collider other)
    {
        if (!this.isServer)
            return;
        if(other.tag == "Player" && active)
        {
            RpcPickup(other.gameObject);
            active = false;
            Invoke("Reactivate", cooldown);
        }
    }

    [ClientRpc]
    void RpcPickup(GameObject other)
    {
        mesh.SetActive(false);
        sound.Play();
        other.GetComponent<Combat.GunController>().PickGun(weaponId);

    }

    [ClientRpc]
    void RpcReEnable()
    {
        mesh.SetActive(true);

    }
    [Server]
    private void Reactivate()
    {
        active = true;
        RpcReEnable();
    }
}
