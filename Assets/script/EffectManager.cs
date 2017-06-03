using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject[] effects;

    public void StartEffect(string effectName)
    {
        foreach (GameObject effect in effects)
        {
            if (effect.name.CompareTo(effectName) == 0)
            {
                effect.SetActive(false);
                effect.SetActive(true);
                return;
            }
        }    
    }
}
