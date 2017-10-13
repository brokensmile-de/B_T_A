using UnityEngine;
using UnityEngine.Networking;

namespace Combat
{
    public class BulletController : NetworkBehaviour
    {
        [SyncVar]
        public NetworkInstanceId spawnedBy;
        public GameObject obj;
        private bool damaged;
        public override void OnStartClient()
        {
            obj = ClientScene.FindLocalObject(spawnedBy);
            Collider[] playerColliders = obj.GetComponents<Collider>();
            Collider bulletCollider = gameObject.GetComponent<Collider>();
            foreach (Collider c in playerColliders)
            {
                Physics.IgnoreCollision(c, bulletCollider);
            }
        }

        private int _damage;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        void OnTriggerEnter(Collider collision)
        {
            if (damaged)
                return;

            GameObject hit = collision.gameObject;
            Hitpoints health = hit.GetComponent<Hitpoints>();

            if (health != null)
            {
                health.TakeDamage(Damage, obj);
                damaged = true;
            }

            Destroy(gameObject);
        }

    }
}

