using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoykaforplateScript : MonoBehaviour
{
    public PlayerController controller;

    public List<GameObject> Plates;
    public GameObject plate;
    public Transform StoykaTransform; // Перемещение в стойке
    public GameObject PlateForStoyka; // Тарелка после того как игрок отпускает тарелку

    public bool CanPut;//Положить
    public bool CanTake;//Взять
    public int KolvoPlatesInStoyka;//Количество тарелок в стойке

    // Start is called before the first frame update
    void Start()
    {
        KolvoPlatesInStoyka = 6;
        CanTake = false;
        CanPut = false;

        for (int i = 0; i < KolvoPlatesInStoyka; ++i)
        {
            Plates.Add(Instantiate(plate));
            Plates[i].transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f + 0.15f * i, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTake == true)
        {
            controller.SubjectsAtHandsPlayer.Add(Plates[KolvoPlatesInStoyka - 1]);
            Plates.RemoveAt(KolvoPlatesInStoyka - 1);
            KolvoPlatesInStoyka--;
        }

        if(Input.GetKeyDown(KeyCode.E) && CanPut == true)
        {
            PlateForStoyka = controller.SubjectsAtHandsPlayer[0];
            controller.SubjectsAtHandsPlayer.Clear();
            PlateForStoyka.transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f + 0.15f * KolvoPlatesInStoyka, transform.position.z);
            Plates.Add(PlateForStoyka);

            KolvoPlatesInStoyka++;
            PlateForStoyka = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Player") && KolvoPlatesInStoyka < 6 && controller.SubjectsAtHandsPlayer[0].CompareTag("Plate"))
                CanPut = true;
            else
                CanPut = false;
        }
        catch { }
        

        if (other.gameObject.CompareTag("Player") && KolvoPlatesInStoyka > 0 && controller.FullInventory == false)
            CanTake = true;
        else
            CanTake = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CanTake = false;
            CanPut = false;
            PlateForStoyka = null;
        }
    }
}
