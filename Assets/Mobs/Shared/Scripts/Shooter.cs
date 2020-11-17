using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootTargeter))]
public class Shooter : MonoBehaviour
{
    [SerializeField]
    ShootTargeter ShootTargeter;

    [SerializeField]
    GameObject Bullet;

    void Start()
    {
        this.ShootTargeter = this.GetComponent<ShootTargeter>();
    }

    void Update()
    {

    }
}
