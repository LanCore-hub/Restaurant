using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript : MonoBehaviour
{
    public PlayerController controller;
    public GameObject Rubbish;
    public bool CanPut, CanPutButton, CanPutButtonPlate;//Положить
    public bool CanTake, CanTakeButton;//Взять

    public GameObject small;
    public GameObject medium;
    public GameObject big;


    public int KolvoRubbishInBasket;//Количество мусора в корзине
    

    // Start is called before the first frame update
    void Start()
    {
        CanPut = false;
        CanPutButton = false;
        CanPutButtonPlate = false;
        CanTake = false;
        CanTakeButton = false;
        KolvoRubbishInBasket = 0;

        small.SetActive(false);
        medium.SetActive(false);
        big.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanPutButton == true && controller.FullInventory == true)
        {
            KolvoRubbishInBasket++;
            Destroy(controller.SubjectsAtHandsPlayer[0]);
            controller.SubjectsAtHandsPlayer.Clear();

            if(KolvoRubbishInBasket == 1)
            {
                small.SetActive(true);
                medium.SetActive(false);
                big.SetActive(false);
            }
            else if (KolvoRubbishInBasket == 2)
            {
                small.SetActive(false);
                medium.SetActive(true);
                big.SetActive(false);
            }
            else if (KolvoRubbishInBasket == 3)
            {
                small.SetActive(false);
                medium.SetActive(false);
                big.SetActive(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && CanTakeButton == true && controller.FullInventory == false)
        {
            controller.isObject = Instantiate(Rubbish);
            controller.SubjectsAtHandsPlayer.Add(controller.isObject);
            KolvoRubbishInBasket = 0;
            small.SetActive(false);
            medium.SetActive(false);
            big.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.E) && CanPutButtonPlate == true && controller.FullInventory == true)
        {
            KolvoRubbishInBasket++;
            foreach (GameObject Food in controller.SubjectsAtHandsPlayer[0].GetComponent<PlateScript>().dish)
                Destroy(Food);

            controller.SubjectsAtHandsPlayer[0].GetComponent<PlateScript>().dish.Clear();

            if (KolvoRubbishInBasket == 1)
            {
                small.SetActive(true);
                medium.SetActive(false);
                big.SetActive(false);
            }
            else if (KolvoRubbishInBasket == 2)
            {
                small.SetActive(false);
                medium.SetActive(true);
                big.SetActive(false);
            }
            else if (KolvoRubbishInBasket == 3)
            {
                small.SetActive(false);
                medium.SetActive(false);
                big.SetActive(true);
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && KolvoRubbishInBasket < 3)
            CanPut = true;
        else
            CanPut = false;

        if (other.gameObject.CompareTag("Player") && KolvoRubbishInBasket > 0)
            CanTake = true;
        else
            CanTake = false;
        


        if (other.gameObject.CompareTag("Player") && controller.FullInventory == true && CanPut == true && !controller.SubjectsAtHandsPlayer[0].CompareTag("Plate") && !controller.SubjectsAtHandsPlayer[0].CompareTag("Rubbish"))
        {
            CanPutButton = true;
        }
        else
            CanPutButton = false;

        if (other.gameObject.CompareTag("Player") && controller.FullInventory == false && CanTake == true)
        {
            CanTakeButton = true;
        }
        else
            CanTakeButton = false;

        if(other.gameObject.CompareTag("Player") && controller.FullInventory == true && CanPut == true && controller.SubjectsAtHandsPlayer[0].CompareTag("Plate") && controller.SubjectsAtHandsPlayer[0].GetComponent<PlateScript>().dish.Count > 0)
        {
            CanPutButtonPlate = true;
        }
        else
            CanPutButtonPlate = false;

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CanTake = false;
            CanTakeButton = false;
            CanPutButton = false;
            CanPutButtonPlate = false;
            CanPut = false;
            controller.isObject = null;
        }
    }
}
