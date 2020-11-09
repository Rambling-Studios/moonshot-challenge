using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    public Transform Target;

    [SerializeField]
    float Speed;

    [SerializeField]
    bool ShouldMove;

    void Start()
    {
        this.ShouldMove = true;
    }

    void FixedUpdate()
    {
        if (!this.ShouldMove) return;

        this.transform.position = Vector3.MoveTowards(this.transform.position, this.Target.position, this.Speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.name != "Moon")
            return;

        this.ShouldMove = false;
        Destroy(this.GetComponent<Rigidbody>());
        PubSub.Instance.Publish(
            Topics.MOVER_REACHED_THE_END,
            new MerikhManager.OnMoverReachedTheEndArgs
            {
                DamageToInflict = 20,
                GameObject = this.gameObject,
                Target = this.Target
            }
        );

    }
}
