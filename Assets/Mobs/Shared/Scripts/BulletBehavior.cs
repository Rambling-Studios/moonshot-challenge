using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject OnCollisionParticle;

    void OnTriggerEnter(Collider collider)
    {

        if (this.OnCollisionParticle != null)
        {
            PubSub.Instance.Publish(Topics.BULLET_COLLISION, new MerikhManager.BulletCollisionArgs()
            {
                OnCollisionParticle = this.OnCollisionParticle,
                Pos = this.transform.parent.position
            });
        }
        Debug.Log($"Hit {collider.name}");
        Destroy(this.transform.parent.gameObject);
    }


}
