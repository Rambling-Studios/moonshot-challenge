using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootTargeter))]
public class Aimer : MonoBehaviour
{
    [SerializeField]
    ShootTargeter ShootTargeter;

    void Start()
    {
        this.ShootTargeter = this.GetComponent<ShootTargeter>();
    }

    void LateUpdate()
    {
        if (this.ShootTargeter.GetTarget() != null)
        {
            this.gameObject.transform.LookAt(this.ShootTargeter.GetTarget());
            if (this.gameObject.name == "Cube")
                Debug.DrawRay(this.transform.position, this.transform.forward * 10, Color.red);
        }
    }
}
