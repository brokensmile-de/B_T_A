using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GridController : MonoBehaviour
{

    // Get a reference to the player
    public Transform player;

    void Update()
    {
        if (player != null)
        {
            GetComponent<Renderer>().sharedMaterial.SetVector("_PlayerPosition", player.position);
        }
        else
        {
            var playerObject = GameObject.Find("Player(Clone)");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }

        }
    }
}