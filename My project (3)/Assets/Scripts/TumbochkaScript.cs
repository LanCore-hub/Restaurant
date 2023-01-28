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

    public void CheckSubjectsTag()
    {
        int True = 0;
        for(int i = 0; i < SubjectsOnTumbochka.Count; ++i)
        {
            if (playerController.tag == SubjectsOnTumbochka[i].tag)
            {
                True++;
                break;
            }
        }
        if (True == 1)
            CanPut = false;
        else
            CanPut = true;
    }

    public void CheckPlateTag()
    {
        if ((!playerController.CompareTag("Plate") && SubjectsOnTumbochka[0].CompareTag("Plate")) || (playerController.CompareTag("Plate") && !SubjectsOnTumbochka[0].CompareTag("Plate")))
            CanPut = true;
        else
            CanPut = false;
    }
}
