using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobleFSM : MonsterFSM 
{
    int currentIndex = 0;
   
    public override IEnumerator Walk()
    {
        //enter
        Transform target = waypoints[currentIndex++];
        agent.SetDestination(target.position);
        agent.speed = walkSpeed;
        agent.stoppingDistance = 0;

        if (currentIndex >= waypoints.Length)
        {
            currentIndex = 0;
           // gameObject.SetActive(false);
           // Invoke("Respawn", 7200.0f);
        }

        while (state == CharacterState.Walk)  
        {
            yield return null;
            //stay
            if (agent.remainingDistance == 0)
            {
                SetState(CharacterState.Idle);
                break;
            }

            if (IsDectectPlayer())
            {
                SetState(CharacterState.Run);
                break;
            }
            //exit
        }
    }
}
