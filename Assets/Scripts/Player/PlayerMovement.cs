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
    [SyncVar]
    public Color color;
    public float speed;
    public float rotateSpeed;
    public float jumpSpeed;
    public float gravity;
    public float dashSpeed;

    public float maxDashes;             //Maximale anzahl an dash charges
    public float dashChargeCooldown;    //Cooldown für einen Charge restore
    private float dashes;               //Wieviele dashes kann man noch usen
    private bool isDashing;             //Gibt an ob der spieler gerade am Dashen ist
    private bool restoringDashes;       //Gibt an ob ein dash charge gerade am laden ist um doppelte aufladungen zu verhindern

    private CharacterController controller;

	//animation ----Esteban
	static Animator anim;

    public Text dashText;

    //has pistol
    private bool hasPistol;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        dashes = maxDashes;

		hasPistol = false;

        if(isLocalPlayer)
        {
            anim = GetComponent<Animator>();
            GameObject hudCanvas = GameObject.Find("HudCanvas");
            dashText = hudCanvas.transform.Find("HealthUI/Dash").GetComponent<Text>();
        }

        transform.FindChild("Mesh").gameObject.GetComponent<SkinnedMeshRenderer>().material.color = color;
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
        if(!Timer.singleton.isGameOver)
        {
            CalcRotationToMouse();  //spieler in richtung Maus rotieren
            DoMovement();
            AlignToGround();        //rotiere Player parallel zur oberfläche auf der er steht 
        }else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isRunningBack", false);
            anim.SetBool("isRunningLeft", false);
            anim.SetBool("isRunningRight", false);
            anim.SetBool("isIdle", true);
        }


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
                dashText.text = dashes+"";
				//Esteban--- schneller anim
				anim.speed = 1.5f;
                StartCoroutine(Dash());
            }

            if(!isDashing)
            {
                //verrechne mit movementspeed
                moveDirection *= speed;
				anim.speed = 1;
            }    
        }
        //subtrahiere gravity
        moveDirection.y -= gravity * Time.deltaTime;
        //apply movement
        controller.Move(moveDirection * Time.deltaTime);

		//animation---Esteban
		//bei moveDirection != 0 -> animmieren
		moveAnim(moveDirection);
		
    }

    private void IncreaseDashCount()
    {
        dashes++;
        dashText.text = dashes + "";
        restoringDashes = false;
    }

    private IEnumerator Dash()
    {
        moveDirection.x = moveDirection.x * dashSpeed;
        moveDirection.z = moveDirection.z * dashSpeed;
        yield return new WaitForSeconds(0.2f);

        isDashing = false;
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

	//animation -----Esteban
	//hiermit wird die Melee-Animation angerufen
	public void Punch(){
		anim.SetBool ("isPunching", true);
	}
	//hiermit wird die Melle-Animation gestoppt
	public void StopPunch(){
		anim.SetBool ("isPunching", false);
	}

	//pistol Körperhaltung
	public void HasPistolAnim(){
		this.hasPistol = true;
	}

	//rifle Körperhaltung
	public void HasNoPistolAnim(){
		this.hasPistol = false;
	}

	//bewegungs anim check
	private void moveAnim(Vector3 moveDirection){

		//prüft ob pistol hat oder nicht
		if (hasPistol) {
			
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isRunningBack", false);
			anim.SetBool ("isRunningLeft", false);
			anim.SetBool ("isRunningRight", false);
			anim.SetBool ("isIdle", false);

			if (moveDirection.x != 0f || moveDirection.z != 0f) {
				anim.SetBool ("isIdleP", false);

				//schaut nach hinten
				// y <= height / 2 && Differenz von x und width / 2 ist 
				// kleiner oder gleich die Differenz von y und height / 2
				if (Input.mousePosition.y <= Screen.height / 2 && (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width) <= (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height)) {

					//bewegt sich nach hinten
					if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {

						anim.SetBool ("isRunningP", true);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich nach vorne
					} else if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {

						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", true);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {

						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", true);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {

						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", true);

					}

				}
				//schaut nach vorne
				// y > height / 2 && Differenz von x und width / 2 ist 
				// kleiner oder gleich die Differenz von y und height / 2
				else if (Input.mousePosition.y > Screen.height / 2 && (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width) <= (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height)) {
					//bewegt sich narch vorne
					if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", true);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);

					}
					//bewegt sich nach hinten
					else if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", true);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", true);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {

						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", true);
						anim.SetBool ("isRunningRightP", false);

					}
				}
				//schaut nach links
				else if (Input.mousePosition.x <= Screen.width / 2 && (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height) <= (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width)) {
					//bewegt sich narch vorne
					if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", true);

					}
					//bewegt sich nach hinten
					else if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", true);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", true);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", true);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);

					}
				}
				//schaut nach rechts
				else if (Input.mousePosition.x > Screen.width / 2 && (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height) <= (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width)) {
					//bewegt sich narch vorne
					if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", true);
						anim.SetBool ("isRunningRightP", false);

					}
					//bewegt sich nach hinten
					else if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", true);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", true);
						anim.SetBool ("isRunningBackP", false);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunningP", false);
						anim.SetBool ("isRunningBackP", true);
						anim.SetBool ("isRunningLeftP", false);
						anim.SetBool ("isRunningRightP", false);

					}
				}




			} else {
				anim.SetBool ("isRunningP", false);
				anim.SetBool ("isRunningBackP", false);
				anim.SetBool ("isRunningLeftP", false);
				anim.SetBool ("isRunningRightP", false);
				anim.SetBool ("isIdleP", true);
			}
		
			//has no pistol
		} else {
			anim.SetBool ("isRunningP", false);
			anim.SetBool ("isRunningBackP", false);
			anim.SetBool ("isRunningLeftP", false);
			anim.SetBool ("isRunningRightP", false);
			anim.SetBool ("isIdleP", false);

			if (moveDirection.x != 0f || moveDirection.z != 0f) {
				anim.SetBool ("isIdle", false);

				//schaut nach hinten
				if (Input.mousePosition.y <= Screen.height / 2 && (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width) <= (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height)) {

					//bewegt sich nach hinten
					if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
					
						anim.SetBool ("isRunning", true);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich nach vorne
					} else if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
					
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", true);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
					
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", true);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
					
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", true);

					}

				}
			//schaut nach vorne
				else if (Input.mousePosition.y > Screen.height / 2 && (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width) <= (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height)) {
					//bewegt sich narch vorne
					if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", true);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);

					}
				//bewegt sich nach hinten
					else if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", true);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", true);
				

						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
					
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", true);
						anim.SetBool ("isRunningRight", false);
				
					}
				}
			//schaut nach links
				else if (Input.mousePosition.x <= Screen.width / 2 && (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height) <= (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width)) {
					//bewegt sich narch vorne
					if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", true);

					}
				//bewegt sich nach hinten
					else if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", true);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", true);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", true);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);

					}
				}
			//schaut nach rechts
				else if (Input.mousePosition.x > Screen.width / 2 && (Mathf.Abs((Screen.height / 2) - Input.mousePosition.y) / Screen.height) <= (Mathf.Abs((Screen.width / 2) - Input.mousePosition.x) / Screen.width)) {
					//bewegt sich narch vorne
					if (moveDirection.z > 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", true);
						anim.SetBool ("isRunningRight", false);

					}
				//bewegt sich nach hinten
					else if (moveDirection.z < 0f && (moveDirection.z * moveDirection.z) > (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", true);


						//bewegt sich rechts
					} else if (moveDirection.x > 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", true);
						anim.SetBool ("isRunningBack", false);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);


						//bewegt sich links
					} else if (moveDirection.x < 0f && (moveDirection.z * moveDirection.z) < (moveDirection.x * moveDirection.x)) {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isRunningBack", true);
						anim.SetBool ("isRunningLeft", false);
						anim.SetBool ("isRunningRight", false);

					}
				}




			} else {
				anim.SetBool ("isRunning", false);
				anim.SetBool ("isRunningBack", false);
				anim.SetBool ("isRunningLeft", false);
				anim.SetBool ("isRunningRight", false);
				anim.SetBool ("isIdle", true);
			}
		}
	
	}

}

