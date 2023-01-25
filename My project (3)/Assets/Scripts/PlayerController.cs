using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�������� ��������� ���������")]
    public CharacterController controller;
    public float speed = 12f;
    public float rotationspeed = 7f;
    private float gravity = -9.0f;
    Vector3 velocity;

    [Header("�������� � ����������")]
    public bool CanTake; //����� �������
    public bool CanTakeFood; //����� ������� �� ��������
    public GameObject[] Subject; // ����� ������� � ���������
    public bool FullInventory; //���� �� � ��������� �������?
    public GameObject isObject; //������� ������� ����� ����� � �����
    public Transform transformSubject; //�����, ��� ����� ����� ����� �������

    [Header("����������� �������")]
    public Tumbochka tumbochkaScript;
    public FridgeScript fridgeScript;
    public BasketScript basketScript;

    void Start()
    {
        rotationspeed = 7f;
        CanTake = false;
        CanTakeFood = false;
        FullInventory = false;
    }

    void Update()
    {
        Controller(); // ����������
        PutOnSubject();
        TakeSubjectFromSpawner();
    }

    // ����������
    private void Controller()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);
        move = Vector3.ClampMagnitude(move, 1);
        if (move.magnitude > Mathf.Abs(0.1f))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), rotationspeed * Time.deltaTime);

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void PutOnSubject()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTake == true) //����������� ����� ������� ������� � ����
        {
            Subject[0] = isObject;
            isObject = null;
            FullInventory = true;
        }

        if (FullInventory == true)
        {
            Subject[0].transform.position = transformSubject.position;
            if (Subject[0].CompareTag("Meat"))
            {
                Subject[0].transform.rotation = transformSubject.rotation;
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && FullInventory == true) // ��������� ������� �� ��������..
        {
            if (tumbochkaScript.CanPut == true)
            {
                Subject[0].transform.position = tumbochkaScript.onTumbochka.position; // ����������� �������� �� ����
                tumbochkaScript.ObjectsOnTumbochka.Add(Subject[0]);
                FullInventory = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && basketScript.CanThrowAway == true) //����������� ������ ������� � �������
        {
            basketScript.CheckFullBasket();
            FullInventory = false;
        }    
    }

    private void TakeSubjectFromSpawner()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTakeFood == true) // ����� ��� �� ��������
        {
            Subject[0] = Instantiate(fridgeScript.Steak);
            FullInventory = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.CompareTag("Meat") || other.gameObject.CompareTag("Plate")) && FullInventory == false)
        {
            CanTake = true;
            isObject = other.gameObject;
        }
        else if ((other.gameObject.CompareTag("Meat") || other.gameObject.CompareTag("Plate")) && FullInventory == true)
        {
            CanTake = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("Meat") || other.gameObject.CompareTag("Plate")) && FullInventory == false)
        {
            CanTake = false;
            isObject = null;
        }
        else if ((other.gameObject.CompareTag("Meat") || other.gameObject.CompareTag("Plate")) && FullInventory == true)
        {
            CanTake = false;
        }
    }
}
