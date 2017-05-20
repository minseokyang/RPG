using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour 
{
    void Awake()
    {
        StartCoroutine("A");
    }
    IEnumerator A()
    {
        while(true)
        {
            Debug.Log("A");
            yield return new WaitForSeconds(0.5f);
        }

    }
}

