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

    void Update()
    {

    }
}
