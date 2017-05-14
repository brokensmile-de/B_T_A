//using UnityEngine;
//using System.Collections;

//public class IronHide : PowerUp
//{
//    public int damageAmount;
//    public string displayName = "Hide of Iron";
//    public float cooldown;
//    public GameObject packMesh;

//    private bool onCooldown;

//    // Use this for initialization
//    void Start()
//    {
//        if (!GetComponent<Hitpoints>())
//        {
//            SphereCollider col = GetComponent<SphereCollider>();
//            if (!GetComponent<Collider>())
//            {
//                col = gameObject.AddComponent<SphereCollider>();
//            }
//            col.radius = 0.5f;
//            col.isTrigger = true;
//            isActive = false;
//        }
//    }

//    public override void Init()
//    {
//        toolTip = displayName;
//        isPassive = true; // is a passive power up
//        iconPosition = Vector2.right * 128;

//        if (GetComponent<Hitpoints>())
//        {
//            Destroy(this, passiveLifetime);
//            dieAt = Time.time + passiveLifetime;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!GetComponent<Hitpoints>())
//        {
//            // do animation effects here.
//            return;
//        }

//        if (isPassive)
//        {
//            DoPowerUp();
//        }
//        else if (isActive)
//        {
//            if (Input.GetButtonDown("Jump"))
//            {
//                DoPowerUp();
//            }
//        }
//    }

//    // handle the trigger enter stuff and update the player
//    void OnTriggerEnter(Collider other)
//    {
//        GameObject go = other.gameObject;
//        if (go.GetComponent<Hitpoints>())
//        {
//            if (go.GetComponent<IronHide>())
//            {
//                Destroy(go.GetComponent<IronHide>());
//            }
//            // create the new PowerUp
//            PowerUp po = go.AddComponent<IronHide>();
//            po.icon = icon;
//            Debug.Log(po);

//            po.Init();

//            Destroy(gameObject);
//        }
//    }

//    void DoPowerUp()
//    {
//        // do the actual powerup stuff here.
//        //PlayerMovement plMove = GetComponent<PlayerMovement>();
//        //plMove.maxDashes = 30f;
//        Hitpoints doubleDamage = GetComponent<Hitpoints>();
//        doubleDamage.ApplyDamage(damageAmount, this.gameObject);
//        Invoke("ReActivate", cooldown);

//    }

//    private void ReActivate()
//    {
//        onCooldown = false;
//        packMesh.SetActive(true);
//    }
//}