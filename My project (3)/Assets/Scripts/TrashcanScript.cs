using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashcanScript : MonoBehaviour
{
    public PlayerController controller;
    public bool CanPutButton;//œÓÎÓÊËÚ¸

    // Start is called before the first frame update
    void Start()
    {
        CanPutButton = false;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.E) && CanPutButton == true)
            {
                Destroy(controller.SubjectsAtHandsPlayer[0]);
                controller.SubjectsAtHandsPlayer.Clear();

            }
        }
        catch { }
    }

    private void OnTriggerStay(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Player") && controller.SubjectsAtHandsPlayer[0].CompareTag("Rubbish") && controller.FullInventory == true)
            {
                CanPutButton = true;
            }
            else
                CanPutButton = false; //Œÿ»¡ ¿
        }
        catch
        {
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanPutButton = false;
            controller.isObject = null;
        }
    }
}
