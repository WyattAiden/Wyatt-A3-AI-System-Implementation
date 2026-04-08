using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDetection : MonoBehaviour
{

    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        ZombieAI otherzom = other.GetComponent<ZombieAI>();
        if (otherzom != null)
        {
            otherzom.ZominRange = true;
            otherzom.Leader = transform.parent.GetComponent<ZombieAI>();
            Debug.Log(otherzom.name + otherzom.ZominRange);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ZombieAI otherzom = other.GetComponent<ZombieAI>();
        if (otherzom != null)
        {
            otherzom.ZominRange = false;
            Debug.Log(otherzom.name + otherzom.ZominRange);
        }
    }
}
