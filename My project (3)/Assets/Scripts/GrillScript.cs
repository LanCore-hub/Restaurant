using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillScript : MonoBehaviour
{
    public List<GameObject> MeatOnGrill;
    private GameObject Player;
    private GameObject Meat;
    public bool CanPut;//Положить
    public bool CanTake;//Взять
    public float Time_Now;
    public float Time_Finish;
    public bool Count;
    // Start is called before the first frame update
    void Start()
    {
        Time_Finish = 5f;
        CanPut = false;
        CanTake = false;
        Player = null;
        Count = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanPut == true)
        {
            MeatOnGrill.Add(Player.GetComponent<PlayerController>().SubjectsAtHandsPlayer[0]);
            Player.GetComponent<PlayerController>().SubjectsAtHandsPlayer.Clear();
            MeatOnGrill[0].transform.position = new Vector3(transform.position.x, transform.position.y + 0.68f, transform.position.z);

            Time_Now = 0;
            Count = true;
        }

        if (Time_Now < Time_Finish && Count == true)
        {
            Time_Now += Time.deltaTime;
        }
        else
        {
            try
            {
                Meat = MeatOnGrill[0];
            }
            catch { }
            Time_Now = 0;
            Destroy(Meat);
            MeatOnGrill.Clear();
            Meat = Instantiate(Player.GetComponent<PlayerController>().Steak_Low);
            MeatOnGrill.Add(Meat);
            MeatOnGrill[0].transform.position = new Vector3(transform.position.x, transform.position.y + 0.68f, transform.position.z);
            Count = false;
        }

        if(Input.GetKeyDown(KeyCode.E) && CanTake == true)
        {
            Player.GetComponent<PlayerController>().SubjectsAtHandsPlayer.Add(MeatOnGrill[0]);
            MeatOnGrill.RemoveAt(0);
            Count = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().FullInventory == true && other.gameObject.GetComponent<PlayerController>().SubjectsAtHandsPlayer[0].CompareTag("Meat") && MeatOnGrill.Count == 0)
        {
            Player = other.gameObject;
            CanPut = true;
        }
        else
            CanPut = false;

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().FullInventory == false && MeatOnGrill.Count == 1)
        {
            CanTake = true;
        }
        else
            CanTake = false;
    }

    private void OnTriggerExit(Collider other)
    {
        CanPut = false;
        CanTake = false;
        Player = null;
    }
}
