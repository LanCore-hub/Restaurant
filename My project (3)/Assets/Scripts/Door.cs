using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public CheckTriggerDoor DoDoor;

    [SerializeField]
    float closeDoor;
    [SerializeField]
    float speed = 1;

    void Start()
    {
        speed = 1;
    }

    void Update()
    {
        if (DoDoor.isClose == true)
            CloseDoor();
    }
    
    void CloseDoor()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(transform.rotation.x, closeDoor, transform.rotation.z), 2* speed * Time.deltaTime);
    }
}
