using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform target;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void LateUpdate()
    {
        transform.position = target.position;
    }

}
