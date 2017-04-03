using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;


    void Start()
    {
        offset = new Vector3(0, 20, -15);
    }

    // LateUpdate um jittering zu verhindern (jittering mit Update taucht nur bei Hinzufügen einer NetworkIdentity auf, 2 Stunden dran verzweifelt)
    void LateUpdate()
    {

        if (player == null)
        {
            return;
        }

        transform.position = player.transform.position + offset;

    }

    public void setTarget(GameObject target)
    {
        player = target;
    }
}
