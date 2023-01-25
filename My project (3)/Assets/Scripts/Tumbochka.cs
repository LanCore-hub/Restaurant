using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbochka : MonoBehaviour
{
    public PlayerController playerController;
    public Transform onTumbochka;
    public bool CanPut;

    [Header("Предметы на столе")]
    public List<GameObject> ObjectsOnTumbochka;

    void Start()
    {
        CanPut = false;
    }

    private void CheckTrueTag() //Проверка, есть ли на столе такой же тег
    {
        int FALSE = 0;
        for (int i = 0; i < ObjectsOnTumbochka.Count; ++i)
        {
            if (ObjectsOnTumbochka[i].tag == playerController.Subject[0].tag)
            {
                FALSE++;
                break;
            }
        }
        if (FALSE == 1)
            CanPut = false;
        else
            CanPut = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerController.FullInventory == true && ObjectsOnTumbochka.Count == 0) // Если на столе ничего не лежит и инвентарь игрока полный
        {
            CanPut = true;
        }
        else if (other.gameObject.CompareTag("Player") && playerController.FullInventory == false)// Если инвентарь игрока пустой
        {
            CanPut = false;
        }
        else if (other.gameObject.CompareTag("Player") && playerController.FullInventory == true && ObjectsOnTumbochka.Count > 0)
        {
            if (ObjectsOnTumbochka[0].tag == "Plate")
                CheckTrueTag();
            else if (playerController.Subject[0].tag == "Plate")
                CheckTrueTag();
            else
                CanPut = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanPut = false;
        }
    }
}
