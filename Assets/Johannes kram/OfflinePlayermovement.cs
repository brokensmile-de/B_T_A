using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Author:Adrian Zimmer
//Description: Movementscript für Player bei dem die Kamera immer im gleichen winkel bleiben sollte und W/A/S/D kamera relativ funktionieren
//Date Created: 27.03.2017
//Last edited: 02.04.2017
//Edited by: Johannes R

public class OfflinePlayermovement : NetworkBehaviour
{

    public float speed;
    public float rotateSpeed;
    public float jumpSpeed;
    public float gravity;


    private CharacterController controller;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    //private Ray ray = new Ray(Vector3.zero, Vector3.down);
    private RaycastHit hit;
    //private float facing = 0;
    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        

        CalcRotationToMouse();  //spieler in richtung Maus rotieren
        DoMovement();
        //AlignToGround();        //rotiere Player parallel zur oberfläche auf der er steht 
    }

    private void DoMovement()
    {
        if (controller.isGrounded)
        {
            //berechne W/S und A/D movement
            Vector3 forward = Vector3.forward * Input.GetAxis("Vertical");
            Vector3 right = Vector3.right * Input.GetAxis("Horizontal");

            //Kombiniere beide
            moveDirection = forward + right;

            //verrechne mit movementspeed
            moveDirection *= speed * 100 * Time.smoothDeltaTime;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        //subtrahiere gravity
        moveDirection.y -= gravity * Time.deltaTime;
        //apply movement
        controller.Move(moveDirection * Time.deltaTime);
    }




    //edit: überarbeitete Version ohne Generierung von neuem Plane pro Update cycle
    private void CalcRotationToMouse()
    {

        //Debug.Log("dont forget to minimize when testing multiplayer on one machine!  Time: "+Time.time+" Input mousepos: "+ Input.mousePosition);
        //berechne Vektor der von der Playerposition aus auf die Mausposition zeigt (Screenspace)
        Vector3 lookVector = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        //normalisieren des Vektors um Werte minimal zu halten
        lookVector.Normalize();
        //Winkel zwischen X-Achse und neuem Vektor (im Screenspace)
        float newRotation = Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg;


        transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
    }





    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }


    //Version mit Raycast und Plane
    /*
    private void CalcRotationToMouse()
    {
        //berechne mouse to screen ray
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ebene die parallel zur spieler position verläuft
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        float hitdist = 0.0f;
        if (playerPlane.Raycast(mouseRay, out hitdist))
        {
            Vector3 targetPoint = mouseRay.GetPoint(hitdist);
            //rotation zur maus berechnen
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            //x und z rotations werte auf 0 setzen(just in case)
            Quaternion fixedTargetRotation = Quaternion.Euler(new Vector3(0, targetRotation.eulerAngles.y, 0));
            transform.rotation = fixedTargetRotation;
        }
    }



    private void AlignToGround()
    {
        ray.origin = transform.position;
        if (Physics.Raycast(ray, out hit))
        {
            Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = grndTilt * transform.rotation;
        }
    }
    */
}
