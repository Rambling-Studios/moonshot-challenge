using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBlockRotator : MonoBehaviour
{
    public float Speed = 0.3f;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", this.Speed * Time.time);
    }
}
