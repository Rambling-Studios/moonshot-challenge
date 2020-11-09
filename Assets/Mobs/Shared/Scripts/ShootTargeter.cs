using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootTargeter : MonoBehaviour
{
    [SerializeField]
    Transform Target;

    [SerializeField]
    public GameObject TargetsGroup;

    [SerializeField]
    Entitie Entitie;

    public Transform GetTarget() => this.Target;

    void Start()
    {
        this.Entitie = this.GetComponent<Entitie>();
    }

    void Update()
    {
        if (this.Target == null)
        {
            this.RequestNewTarget();
        }
        else if (this.Target != null && this.Target.TryGetComponent<Mover>(out var mover) && !mover.enabled)
        {
            this.RequestNewTarget();
        }
    }

    void RequestNewTarget()
    {
        var closest = (
                Dist: float.MaxValue,
                this.Target
            );

        for (var i = 0; i < this.TargetsGroup.transform.childCount; i += 1)
        {
            var curr = this.TargetsGroup.transform.GetChild(i);

            if (curr == null)
                continue;

            if (!curr.gameObject.activeSelf)
                continue;

            if (curr.TryGetComponent<Mover>(out var mover) && !mover.enabled)
                continue;

            var dist = Vector3.Distance(this.transform.position, curr.position);
            if (dist > this.Entitie.ViewRange)
                continue;

            if (dist > closest.Dist)
                continue;

            closest = (dist, curr);
        }

        this.Target = closest.Target;
    }
}
