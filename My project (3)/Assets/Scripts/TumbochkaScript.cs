using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbochkaScript : MonoBehaviour
{
    public GameObject playerController;
    public List<GameObject> SubjectsOnTumbochka;
    public bool CanPut;
    void Start()
    {
        CanPut = false;
        playerController = null;
    }

    void Update()
    {

    }

    private void CheckTagFood1()
    {
        int FALSE = 0;
        foreach(GameObject Food in playerController.GetComponent<PlateScript>().dish)
        {
            if (Food.tag == SubjectsOnTumbochka[0].tag)
                FALSE++;

            if (FALSE == 1)
            {
                CanPut = false;
                break;
            }
        }

        if (FALSE == 0)
            CanPut = true;
    }
    private void CheckTagFood2()
    {
        int FALSE = 0;
        foreach (GameObject Food in SubjectsOnTumbochka[0].GetComponent<PlateScript>().dish)
        {
            if (Food.tag == playerController.tag)
                FALSE++;

            if (FALSE == 1)
            {
                CanPut = false;
                break;
            }
        }

        if (FALSE == 0)
            CanPut = true;
    }

    public void CheckPlateTag()
    {
        if ((!playerController.CompareTag("Plate") && SubjectsOnTumbochka[0].CompareTag("Plate") && SubjectsOnTumbochka[0].GetComponent<PlateScript>().dish.Count == 0) || (playerController.CompareTag("Plate") && !SubjectsOnTumbochka[0].CompareTag("Plate") && playerController.GetComponent<PlateScript>().dish.Count == 0))
        {
            CanPut = true;
        }
        else if (!playerController.CompareTag("Plate") && SubjectsOnTumbochka[0].CompareTag("Plate") && SubjectsOnTumbochka[0].GetComponent<PlateScript>().dish.Count > 0)
        {
            CheckTagFood2();
        }
        else if (playerController.CompareTag("Plate") && !SubjectsOnTumbochka[0].CompareTag("Plate") && playerController.GetComponent<PlateScript>().dish.Count > 0)
        {
            CheckTagFood1();
        }
        else
            CanPut = false;
    }
}
