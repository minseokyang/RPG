using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RayMove1 : MonoBehaviour 
{
    public GameObject movePoint;
    public GameObject attackPoint;

    public float moveSpeed = 4.0f;
    public float turnSpeed = 360.0f;

    public CharacterState state = CharacterState.Idle;

    NavMeshAgent agent;

    Animator a;

    public LayerMask layerMask;

    public float attackRange = 1.5f;

    void Awake()
    {
        movePoint.SetActive(false);
        attackPoint.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;
        agent.acceleration = 2000.0f;

        a = GetComponent<Animator>();

        layerMask = LayerMask.GetMask("Click","Block","Monster");
    }

    void Update()
    {
        switch (state)
        {
            case CharacterState.Idle:
                break;

            case CharacterState.Run:
                if (agent.remainingDistance == 0)
                {
                    state = CharacterState.Idle;

                    movePoint.SetActive(false);
                }

                break;

            case CharacterState.AttackRun:
                if (agent.remainingDistance <= attackRange)
                {
                    state = CharacterState.Idle;
                    attackPoint.SetActive(false);
                }
                else
                {
                    agent.SetDestination(attackPoint.transform.position);
                }
                break;

            case CharacterState.Attack:
                break;

        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100.0f,layerMask))
            {
                int layer = hitInfo.collider.gameObject.layer;

                if (layer == LayerMask.NameToLayer("Click"))
                {
                    Debug.Log(hitInfo.point);
                    movePoint.transform.position = hitInfo.point;
                    movePoint.SetActive(true);

                    agent.SetDestination(movePoint.transform.position);
                    state = CharacterState.Run;
                    agent.stoppingDistance = 0;

                }
                else if (layer == LayerMask.NameToLayer("Monster"))
                {
                   
                    attackPoint.transform.SetParent(hitInfo.transform);
                    attackPoint.transform.localPosition = Vector3.zero;
                    attackPoint.SetActive(true);
                    movePoint.SetActive(false);    
                    agent.SetDestination(attackPoint.transform.position);
                    state = CharacterState.AttackRun;
                    agent.stoppingDistance = attackRange;
                }
            }
        }

        a.SetInteger("state", (int)state);
    }
}
