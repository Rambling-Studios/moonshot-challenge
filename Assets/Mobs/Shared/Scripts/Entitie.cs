using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entitie : MonoBehaviour
{

    [SerializeField]
    int CurrentHealth;

    [SerializeField]
    int MaxHealth;

    [SerializeField]
    public int ViewRange = 150;

    void Start()
    {
        this.MaxHealth = 100;
        this.CurrentHealth = this.MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        this.CurrentHealth -= damage;

        if (this.CurrentHealth <= 0)
        {
            // die
        }
    }

}
