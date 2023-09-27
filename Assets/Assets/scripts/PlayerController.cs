using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsInteracting = false;
    public float interactionRadius = 5f; // The radius within which interaction can occur.
    public float EatinginteractionRadius = 5f;
    public GameObject DogLovePoint;
    public GameObject DogFOODPoint;
    public GameObject FoodPoint;
    public float TimeForPetting = 5f;
    GameManager m_GameManager;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    private Vector3 _input;
    bool IsInRadius = false;
    bool outRadius = false;
    bool IsInFoodRadius = false;
    bool outFoodRadius = false;
    bool DogRadius = false;
    bool FoodRadius = false;

    private void Awake()
    {
        m_GameManager = GameManager.Instance;
    }

    private void Update()
    {

        if (IsInteracting) return;
     
        GatherInput();
        Look();
        if (Input.GetKeyDown(KeyCode.E) && !m_GameManager.m_DogController.isMoveable)
        {
            IsInteracting = true;
            DogLovePoint.SetActive(false);
            m_GameManager.m_DogController.PettingDog();
        }
        else if (Input.GetKeyDown(KeyCode.F) && FoodRadius && !m_GameManager.FoodFull)
        {
            Renderer renderer = FoodPoint.GetComponent<Renderer>();
            if (renderer != null) 
            {
                m_GameManager.FoodFull = true;
                m_GameManager.m_Food.foodobject.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            if (!FoodRadius)
            {  
                if (!IsInRadius && m_GameManager.m_Happy.slider.value < 1f)
                {
                    DogRadius = true;
                    IsInRadius = true;
                    outRadius = true;
                    DogLovePoint.SetActive(true);
                    m_GameManager.m_DogController.StopDog();
                }
                else
                {
                    m_GameManager.m_DogController.PettingStart.Play();
                }
            }
        }
        else if (other.CompareTag("FoodBowl"))
        {

            if (!IsInFoodRadius && m_GameManager.m_Food.slider.value < 1f)
            {
               
                if (!DogRadius && !m_GameManager.FoodFull)
                {
                    FoodRadius = true;
                    IsInFoodRadius = true;
                    outFoodRadius = true;
                    DogFOODPoint.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            if (outRadius)
            {
               
                DogRadius = false;
                outRadius = false;
                IsInRadius = false;
                DogLovePoint.SetActive(false);
                m_GameManager.m_DogController.StartDogMovement();

             
            }

            if (m_GameManager.m_DogController.PettingStart.isPlaying)
            {
                m_GameManager.m_DogController.PettingStart.Stop();
                Debug.Log("Executed");
            }
        }

        else if (other.CompareTag("FoodBowl"))
        {
            if (outFoodRadius)
            {
                FoodRadius = false;
                outFoodRadius = false;
                IsInFoodRadius = false;
                DogFOODPoint.SetActive(false);
            }
        }
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

