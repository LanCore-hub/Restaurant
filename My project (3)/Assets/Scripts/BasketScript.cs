using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject small;
    public GameObject srednee;
    public GameObject big;

    private GameObject ObjectToBasket;

    public bool CanThrowAway;
    private int KolInBasket;

    void Start()
    {
        CanThrowAway = false;
        KolInBasket = 0;

        small.SetActive(false);
        srednee.SetActive(false);
        big.SetActive(false);
    }

    public void CheckFullBasket()
    {
        KolInBasket++;
        ObjectToBasket = playerController.Subject[0];
        playerController.Subject[0] = null;
        Destroy(ObjectToBasket);
        if (KolInBasket == 0)
        {
            small.SetActive(false);
            srednee.SetActive(false);
            big.SetActive(false);
        }
        else if (KolInBasket == 1)
        {
            small.SetActive(true);
            srednee.SetActive(false);
            big.SetActive(false);
        }
        else if (KolInBasket == 2)
        {
            small.SetActive(false);
            srednee.SetActive(true);
            big.SetActive(false);
        }
        else if (KolInBasket == 3)
        {
            small.SetActive(false);
            srednee.SetActive(false);
            big.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && KolInBasket < 3 && playerController.Subject[0].tag != "Plate")
            CanThrowAway = true;
        else
            CanThrowAway = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanThrowAway = false;
        }
    }
}
