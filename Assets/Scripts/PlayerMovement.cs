using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:Adrian Zimmer
//Description: Movementscript für Player bei dem die Kamera immer im gleichen winkel bleiben sollte und W/A/S/D kamera relativ funktionieren
//Date Created: 27.03.2017
//Last edited:
//Edited by:

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float rotateSpeed;
    public float jumpSpeed;
    public float gravity;

    private CharacterController controller;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private Ray ray = new Ray(Vector3.zero, Vector3.down);
    private RaycastHit hit;
    private float facing = 0;
    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        CalcRotationToMouse();  //spieler in richtung Maus rotieren
        DoMovement();
        AlignToGround();        //rotiere Player parallel zur oberfläche auf der er steht 
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

    private void AlignToGround()
    {
        ray.origin = transform.position;
        if (Physics.Raycast(ray, out hit))
        {
            Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = grndTilt * transform.rotation;
        }
    }

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
}
