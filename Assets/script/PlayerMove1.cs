using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove1 : MonoBehaviour 
{
    public float moveSpeed = 4.0f;
    public float turnSpeed = 360.0f;
    public float gravityRatio = 0.5f;

    CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Rotate(0, turnSpeed * h * Time.deltaTime, 0);
        cc.Move((transform.forward * v * moveSpeed + 
            Physics.gravity * gravityRatio) * Time.deltaTime);
    }
}
