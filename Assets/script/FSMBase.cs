using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMBase : MonoBehaviour
{

    public CharacterState state = CharacterState.Idle;
    public NavMeshAgent agent;
    public Animator a;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        a = GetComponent<Animator>();
    }

    public virtual void OnEnable()
    {
        StartCoroutine("FSMMain");
    }

    protected IEnumerator FSMMain()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }
        
    public virtual IEnumerator Idle()
    {
        //enter


        while (state == CharacterState.Idle)
        {
            yield return null;
            //stay
        }
        //exit
    }
    public void SetState(CharacterState newState)
    {
        state = newState;
        a.SetInteger("state", (int)state);

    }

}
