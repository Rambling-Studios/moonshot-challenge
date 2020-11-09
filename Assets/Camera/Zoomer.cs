using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Zoomer : MonoBehaviour
{
    [SerializeField]
    Camera Camera;

    void Start()
    {
        this.Camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        var currentSize = this.Camera.orthographicSize;
        var newSize = Mathf.Min(40, Mathf.Max(15, currentSize - (-1 * Input.mouseScrollDelta.normalized.y * 100 * Time.deltaTime)));
        this.Camera.orthographicSize = newSize;
    }
}
