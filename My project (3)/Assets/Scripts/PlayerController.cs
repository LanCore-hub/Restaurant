using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Основные настройки персонажа")]
    public CharacterController controller;
    public float speed = 12f;
    public float rotationspeed = 7f;
    private float gravity = -9.0f;
    Vector3 velocity;

    [Header("Взаимодействия с предметами")]
    public Transform HandsPlayer; //Позиция рук игрока
    public List<GameObject> SubjectsAtHandsPlayer; //Список предметов в руках игрока
    public GameObject isObject; //Предмет лежит на земле
    public bool FullInventory; // Полный ли инвентарь
    public bool CanTake; //Может ли взять лежачий предмет

    [Header("Возможность брать предметы со спавнеров")]
    public bool CanTakeSubjectFromSpawner;
    public GameObject Steak;

    [Header("Возможность поставить предмет на тумбочку")]
    public bool CanTakeSubjectOnTumbochka;
    public Transform isObjectTumbochka;
    public GameObject Tumbochka;

    [Header("Взаимодействие с тарелкой")]
    public GameObject Plate;

    [Header("Возможность взять предмет с тумбочки")]
    public bool CanVziatSubjectWithTumbochka;
    void Start()
    {
        rotationspeed = 7f;
        CanTake = false;
        CanTakeSubjectFromSpawner = false;
        CanTakeSubjectOnTumbochka = false;
        CanVziatSubjectWithTumbochka = false;
        CheckFullInventory();
    }

    void Update()
    {
        Controller(); // Управление
        CheckFullInventory(); // Проверка полный ли инвентарь
        CanTakeSubject(); // Возможность взять предмет
        TakeSubjectsFromSpawner(); // Возможность брать предметы со спавнеров
        PutOnTumbochka(); // Возможность ставить предметы на тумпочку
    }

    // Управление
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

    //Возможность брать предметы
    private void CanTakeSubject()
    {
        if (Input.GetKeyDown(KeyCode.E) && FullInventory == false && CanVziatSubjectWithTumbochka == true)
        {
            if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Count == 1)
                SubjectsAtHandsPlayer.Add(Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0]);
            else if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Count == 2)
            {
                if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0].CompareTag("Plate"))
                    SubjectsAtHandsPlayer.Add(Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0]);
                else
                    SubjectsAtHandsPlayer.Add(Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[1]);
            }
            Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Clear();
            CanVziatSubjectWithTumbochka = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && CanTake == true && FullInventory == false && CanVziatSubjectWithTumbochka == false)
        {
            SubjectsAtHandsPlayer.Add(isObject);
        }
    }

    //Возможность брать предметы со спавнеров
    private void TakeSubjectsFromSpawner()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTakeSubjectFromSpawner == true && FullInventory == false)
        {
            isObject = Instantiate(Steak);
            SubjectsAtHandsPlayer.Add(isObject);
        }
    }

    //Возможность ставить предметы на тумбочку
    private void PutOnTumbochka()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTakeSubjectOnTumbochka == true && FullInventory == true && Tumbochka.GetComponent<TumbochkaScript>().CanPut == true)
        {
            SubjectsAtHandsPlayer[0].transform.position = isObjectTumbochka.transform.position;
            Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Add(SubjectsAtHandsPlayer[0]);
            SubjectsAtHandsPlayer.Clear();

            if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Count > 1)
            {
                if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0].CompareTag("Plate"))
                {
                    Plate = Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0];
                    Plate.GetComponent<PlateScript>().dish.Add(Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[1]);
                }
                else
                {
                    Plate = Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[1];
                    Plate.GetComponent<PlateScript>().dish.Add(Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0]);
                }
            }
        }
    }

    //TODO: Проверка на то, какие предметы стоят на тумбочке
    

    // Проверка полный ли инвентарь
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
            
        if (other.gameObject.CompareTag("Fridge") && FullInventory == false) // Если игрок стоит у холодильника
        {
            CanTakeSubjectFromSpawner = true;
        }
        else if (FullInventory == true)
        {
            CanTakeSubjectFromSpawner = false;
            isObject = null;
        }

        if (other.gameObject.CompareTag("Tumbochka") && FullInventory == true) // Если игрок стоит у тумбочки
        {
            CanTakeSubjectOnTumbochka = true;
            isObjectTumbochka.position = new Vector3(other.transform.position.x, other.transform.position.y + 0.6f, other.transform.position.z);
            Tumbochka = other.gameObject;
            Tumbochka.GetComponent<TumbochkaScript>().CanPut = true;
            try
            {
                Tumbochka.GetComponent<TumbochkaScript>().playerController = SubjectsAtHandsPlayer[0]; //ОШИБКА

                Tumbochka.GetComponent<TumbochkaScript>().CanPut = true;
                if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Count == 0)
                    Tumbochka.GetComponent<TumbochkaScript>().CanPut = true;
                else if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Count == 1)
                {
                    Tumbochka.GetComponent<TumbochkaScript>().CheckPlateTag();
                }
                else
                {
                    if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0].CompareTag("Plate"))
                    {
                        Plate = Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[0];
                        int FALSE = 0;
                        foreach (GameObject Food in Plate.GetComponent<PlateScript>().dish)
                        {
                            if (Food.tag == SubjectsAtHandsPlayer[0].tag || SubjectsAtHandsPlayer[0].CompareTag("Plate"))
                                FALSE++;

                            if (FALSE == 1)
                            {
                                Tumbochka.GetComponent<TumbochkaScript>().CanPut = false;
                                break;
                            }
                        }
                        if (FALSE == 0)
                            Tumbochka.GetComponent<TumbochkaScript>().CanPut = true;
                    }
                    else
                    {
                        if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[1].CompareTag("Plate"))
                        {
                            Plate = Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka[1];
                            int FALSE = 0;
                            foreach (GameObject Food in Plate.GetComponent<PlateScript>().dish)
                            {
                                if (Food.tag == SubjectsAtHandsPlayer[0].tag || SubjectsAtHandsPlayer[0].CompareTag("Plate"))
                                    FALSE++;

                                if (FALSE == 1)
                                {
                                    Tumbochka.GetComponent<TumbochkaScript>().CanPut = false;
                                    break;
                                }
                            }
                            if (FALSE == 0)
                                Tumbochka.GetComponent<TumbochkaScript>().CanPut = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }
        else if (FullInventory == false && other.gameObject.CompareTag("Tumbochka"))
        {
            Tumbochka = other.gameObject;
            if (Tumbochka.GetComponent<TumbochkaScript>().SubjectsOnTumbochka.Count > 0)
                CanVziatSubjectWithTumbochka = true;
            else
                CanVziatSubjectWithTumbochka = false;
        }
        else if (FullInventory == false)
        {
            CanTakeSubjectOnTumbochka = false;
            CanVziatSubjectWithTumbochka = false;
        }
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
            CanVziatSubjectWithTumbochka = false;
            isObject = null;
            Tumbochka = null;
        }
    }
}
