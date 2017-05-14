using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Author:Adrian Zimmer
//Description: Movementscript für Player bei dem die Kamera immer im gleichen winkel bleiben sollte und W/A/S/D kamera relativ funktionieren
//Date Created: 27.03.2017
//Last edited: 05.04.2017
//Edited by: Adrian Zimmer

public class PlayerMovement : NetworkBehaviour
{

    public float speed;
    public float rotateSpeed;
    public float jumpSpeed;
    public float gravity;
    public float dashSpeed;
    public float DashPowerUpAmount;
    

    public float maxDashes;             //Maximale anzahl an dash charges
    public float dashChargeCooldown;    //Cooldown für einen Charge restore
    private float dashes;               //Wieviele dashes kann man noch usen
    private bool isDashing;             //Gibt an ob der spieler gerade am Dashen ist
    private bool restoringDashes;       //Gibt an ob ein dash charge gerade am laden ist um doppelte aufladungen zu verhindern
 


    private CharacterController controller;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        dashes = maxDashes;
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if(dashes < maxDashes && !restoringDashes)
        {
            restoringDashes = true;
            Invoke("IncreaseDashCount", dashChargeCooldown);
        }

        CalcRotationToMouse();  //spieler in richtung Maus rotieren
        DoMovement();
        AlignToGround();        //rotiere Player parallel zur oberfläche auf der er steht 
    }

    private Vector3 moveDirection = Vector3.zero;
    private void DoMovement()
    {
        if (controller.isGrounded)
        {
            //berechne W/S und A/D movement
            Vector3 forward = Vector3.forward * Input.GetAxis("Vertical");
            Vector3 right = Vector3.right * Input.GetAxis("Horizontal");

            if(!isDashing)
            {
                //Kombiniere beide
                moveDirection = forward + right;
            }

            if (Input.GetButtonDown("Jump") && dashes >= 1 && !isDashing)
            {
                isDashing = true;
                dashes --;

                StartCoroutine(Dash());
            }

            if(!isDashing)
            {
                //verrechne mit movementspeed
                moveDirection *= speed;
            }    
        }
        //subtrahiere gravity
        moveDirection.y -= gravity * Time.deltaTime;
        //apply movement
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void IncreaseDashCount()
    {
        dashes++;
        restoringDashes = false;
    }

    public void IncreaseDashPowerUp()
    {
        
            StartCoroutine(DashPowerUp());
        
    }

    private IEnumerator Dash()
    {
        moveDirection.x = moveDirection.x * dashSpeed;
        moveDirection.z = moveDirection.z * dashSpeed;
        yield return new WaitForSeconds(0.2f);

        isDashing = false;
    }

    private IEnumerator DashPowerUp()
    {


            powerUpCountDown timer = GetComponent<powerUpCountDown>();

            do
            {
            
                if (dashes < DashPowerUpAmount)
                    dashes++;
                else if (dashes >= DashPowerUpAmount)
                    dashes = DashPowerUpAmount;

                
                yield return new WaitForSeconds(0.5f);

            } while (timer.hasPowerUp==true);
            
        
    }

    private Vector3 lookVector;
    private void CalcRotationToMouse()
    {
        //berechne Vektor der von der Playerposition aus auf die Mausposition zeigt (Screenspace)
        lookVector = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        //normalisieren des Vektors um Werte minimal zu halten
        lookVector.Normalize();
        //Winkel zwischen X-Achse und neuem Vektor (im Screenspace)
        float newRotation = Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
    }

    private Ray ray = new Ray(Vector3.zero, Vector3.down);
    private RaycastHit hit;
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

