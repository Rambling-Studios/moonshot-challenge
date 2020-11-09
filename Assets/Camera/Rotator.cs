using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    Transform Center;

    // Update is called once per frame
    void Update()
    {
        var canRotate = Input.GetMouseButton(0) && Input.GetKey(KeyCode.Space);

        if (!canRotate)
            return;

        var mouseX = Input.GetAxisRaw("Mouse X");

        this.transform.RotateAround(this.Center.position, Vector3.up, 300 * mouseX * Time.deltaTime);
    }
}
