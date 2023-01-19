using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTriggerDoor : MonoBehaviour
{
    public bool isClose;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isClose = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isClose = true;
    }
}
