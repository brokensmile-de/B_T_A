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
			
            player = GameObject.Find("Player(Clone)").transform;

			//Esteban---- verändert für Level Anim
			//player = GameObject.Find("BTA_Player 1(Clone)").transform;
        }
    }
}