using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:Adrian Zimmer
//Description: Movementscript für Player bei dem die Kamera immer in die richtung des Players guckt und W/A/S/D spieler relativ funktioniert
//Date Created: 28.03.2017
//Last edited:
//Edited by:

public class PlayerMovement2 : MonoBehaviour {

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
        RotatePlayer(); //Drehe Spieler wenn die maus auf der Y Achse bewegt wird
        DoMovement();
        AlignToGround(); //rotiere Player parallel zur oberfläche auf der er steht
    }

    private void RotatePlayer()
    {
        //addiere/Subtrahiere mousemovement vom yaw angle
        facing += Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed * 10;

        //setze Yaw Rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, facing, 0));
    }

    private void DoMovement()
    {
        if (controller.isGrounded)
        {
            //berechne W/S und A/D movement
            Vector3 forward = transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical");
            Vector3 right = transform.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal");

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

}
