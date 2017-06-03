using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFSM: FSMBase 
{

    public GameObject movePoint;
    public GameObject attackPoint;

    public float moveSpeed = 4.0f;
    public float turnSpeed = 360.0f;


    public LayerMask layerMask;

    public float attackRange = 1.5f;
    public float attack = 100.0f;
    public int maxHP = 100;
    public int currentHP = 10;
    public int exp = 0; 
    public int gold = 0;
    public int level = 1;


    public Bounds bounds;

    public Renderer renderer;

    public MonsterFSM monsterFSM;

    EffectManager effectManager;

    public override void Awake()
    {
        base.Awake();

        movePoint = GameObject.Find("MovePoint");

        movePoint.SetActive(false);

        attackPoint = GameObject.Find("AttackPoint");
        attackPoint.SetActive(false);
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;
        agent.acceleration = 2000.0f;
       
        renderer = GetComponentInChildren<Renderer>();
        effectManager = GetComponentInChildren<EffectManager>();

        bounds = GetComponentInChildren<Renderer>().bounds;

        layerMask = LayerMask.GetMask("Click","Block","Monster");
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    void Update()
    {

        if (state == CharacterState.Skill1 || IsDead())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(CharacterState.Skill1);
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
                    SetState(CharacterState.Run);
                    agent.stoppingDistance = 0;

                }
                else if (layer == LayerMask.NameToLayer("Monster"))
                {

                    attackPoint.transform.SetParent(hitInfo.transform);
                    attackPoint.transform.localPosition = Vector3.zero;
                    attackPoint.SetActive(true);
                    movePoint.SetActive(false);    
                    agent.SetDestination(attackPoint.transform.position);
                    SetState(CharacterState.AttackRun);
                    agent.stoppingDistance = attackRange;
                    monsterFSM = hitInfo.transform.GetComponent<MonsterFSM>();
                }
            }
        }

        a.SetInteger("state", (int)state);
    }

    public override IEnumerator Idle()
    {
        //enter


        while (state == CharacterState.Idle)
        {
            yield return null;
            //stay
        }
        //exit
    }
    IEnumerator Run()
    {
        //enter


        while (state == CharacterState.Run)
        {
            yield return null;
            //stay
            if (agent.remainingDistance == 0)
            {
                SetState(CharacterState.Idle);

                movePoint.SetActive(false);
            }
        }
        //exit
    }
    IEnumerator AttackRun()
    {
        //enter


        while (state == CharacterState.AttackRun)  
        {
            yield return null;
            //stay
            if (agent.remainingDistance <= attackRange)
            {
                SetState(CharacterState.Attack);
                attackPoint.SetActive(false);
            }
        //exit
        }
    }
    IEnumerator Attack()
    {
        //enter


        while (state == CharacterState.Attack)
        {
            yield return null;
            //stay

            MoveUtil.RotateBurst(transform, attackPoint.transform);

            if (Vector3.Distance(transform.position,
                attackPoint.transform.position) > attackRange && RemainTime(0.7f))
            {
                SetState(CharacterState.AttackRun);
                break;
            }

            if (monsterFSM.state == CharacterState.Dead &&
                (a.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f) > 0.7)
            {
                attackPoint.SetActive(false);
                SetState(CharacterState.Idle);
                break;
            }
        }
        //exit
    }
    public void OnPlayerAttack()
    {
        monsterFSM.ProcessDamage(attack);
    }

    public bool IsDead()
    {
        return state == CharacterState.Dead;
    }

    public bool RemainTime(float ratio)
    {
        return (a.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > ratio);
    }

    public void ProcessDamage(float damage)
    {
        currentHP -= (int)damage;

        if (currentHP <= 0)
        {
            SetState(CharacterState.Dead);
            currentHP = 0;
        }
       
    }
    IEnumerator Dead()
    {
        while (state == CharacterState.Dead)
        {
            yield return null;
        }

    }

    public void StartEffect(string effectName)
    {
        effectManager.StartEffect(effectName);
    }


    public IEnumerator Skill1()
    {
        //enter
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        movePoint.SetActive(false);
        attackPoint.SetActive(false);

        while (state == CharacterState.Skill1)
        {
            yield return null;
            //stay
//            if (RemainTime(0.9f) &&
//                state == CharacterState.Skill1)
//           {
//                agent.isStopped = false;
//                SetState(CharacterState.Idle);
//               break;
//            }
            foreach (Collider monster in Physics.OverlapSphere(transform.position, 5.0f))
            {
                if (monster.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    monster.SendMessage("ProcessDamage", 100.0f,SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        //exit

    }
    public void OnSkill1Attack()
    {
        foreach (GameObject monster in GameObject.FindGameObjectWithTag("Monster"))
        {
            if (Vector3.Distance(transform.position, monster.transform.position) <= 5.0f)
            {
                monster.GetComponent<MonsterFSM>().ProcessDamage(100.0f);
            }
        }
    }

    public void Skill1End()
    {
        SetState(CharacterState.Idle);
        agent.isStopped = false;
    }
    public void GainExp(int gainExp)
    {
        exp += gainExp;

        CheckLevel();
    }
    public void GainGold(int gainGold)
    {
        gold += gainGold;
    }
    public void CheckLevel1()
    {
        if (exp % 30 == 0)
        {
            StartEffect("LevelUp");
        }
    }
}
