using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeScript : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject Steak;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerController.FullInventory == false)
        {
            playerController.CanTakeFood = true;
        }
        else if (other.gameObject.CompareTag("Player") && playerController.FullInventory == true)
        {
            playerController.CanTakeFood = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerController.FullInventory == false)
        {
            playerController.CanTakeFood = false;
            playerController.isObject = null;
        }
        else if (other.gameObject.CompareTag("Player") && playerController.FullInventory == true)
        {
            playerController.CanTakeFood = false;
        }
    }
}
