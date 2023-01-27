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

    [Header("�������������� � ����������")]
    public Transform HandsPlayer; //������� ��� ������
    public List<GameObject> SubjectsAtHandsPlayer; //������ ��������� � ����� ������
    public GameObject isObject; //������� ����� �� �����
    public bool FullInventory; // ������ �� ���������
    public bool CanTake; //����� �� ����� ������� �������

    [Header("����������� ����� �������� �� ���������")]
    public bool CanTakeSubjectFromSpawner;
    public GameObject Steak;

    [Header("����������� ��������� ������� �� ��������")]
    public bool CanTakeSubjectOnTumbochka;
    public Transform isObjectTumbochka;
    void Start()
    {
        rotationspeed = 7f;
        CanTake = false;
        CanTakeSubjectFromSpawner = false;
        CanTakeSubjectOnTumbochka = false;
        CheckFullInventory();
    }

    void Update()
    {
        Controller(); // ����������
        CheckFullInventory(); // �������� ������ �� ���������
        CanTakeSubject(); // ����������� ����� �������
        TakeSubjectsFromSpawner(); // ����������� ����� �������� �� ���������
        PutOnTumbochka(); // ����������� ������� �������� �� ��������
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

    //����������� ����� ��������
    private void CanTakeSubject()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTake == true && FullInventory == false)
        {
            SubjectsAtHandsPlayer.Add(isObject);
        }
    }

    //����������� ����� �������� �� ���������
    private void TakeSubjectsFromSpawner()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTakeSubjectFromSpawner == true && FullInventory == false)
        {
            isObject = Instantiate(Steak);
            SubjectsAtHandsPlayer.Add(isObject);
        }
    }

    //����������� ������� �������� �� ��������
    private void PutOnTumbochka()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTakeSubjectOnTumbochka == true && FullInventory == true)
        {
            SubjectsAtHandsPlayer[0].transform.position = isObjectTumbochka.transform.position;
            SubjectsAtHandsPlayer.Clear();
        }
    }

    //TODO: �������� �� ��, ����� �������� ����� �� ��������
    

    // �������� ������ �� ���������
    private void CheckFullInventory()
    {
        if (SubjectsAtHandsPlayer.Count == 1)
        {
            SubjectsAtHandsPlayer[0].transform.position = HandsPlayer.transform.position;
            if (SubjectsAtHandsPlayer[0].CompareTag("Meat"))
            {
                SubjectsAtHandsPlayer[0].transform.rotation = HandsPlayer.transform.rotation;
            }
            FullInventory = true;
        }
        else
            FullInventory = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Plate") && FullInventory == false)
        {
            CanTake = true;
            isObject = other.gameObject;
        }
        else if (FullInventory == true)
        {
            CanTake = false;
            isObject = null;
        }
            
        if (other.gameObject.CompareTag("Fridge") && FullInventory == false) // ���� ����� ����� � ������������
        {
            CanTakeSubjectFromSpawner = true;
        }
        else if (FullInventory == true)
        {
            CanTakeSubjectFromSpawner = false;
            isObject = null;
        }

        if (other.gameObject.CompareTag("Tumbochka") && FullInventory == true) // ���� ����� ����� � ��������
        {
            CanTakeSubjectOnTumbochka = true;
            isObjectTumbochka.position = new Vector3(other.transform.position.x, other.transform.position.y + 0.6f, other.transform.position.z);
        }
        else if (FullInventory == false)
            CanTakeSubjectOnTumbochka = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Plate"))
        {
            CanTake = false;
            isObject = null;
        }

        if (other.gameObject.CompareTag("Fridge"))
        {
            CanTakeSubjectFromSpawner = false;
            isObject = null;
        }

        if (other.gameObject.CompareTag("Tumbochka"))
        {
            CanTakeSubjectOnTumbochka = false;
            isObject = null;
        }
    }
}
