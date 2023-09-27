using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEditor;
using DG.Tweening;

public class DogController : MonoBehaviour
{
    #region VARIABLES
    GameManager m_GameManager;


    [Header("<----PARTICLES---->")]

    public ParticleSystem PettingStart;
    public ParticleSystem PettingDone;
    [Header("<----FLOAT VALUE---->")]
    public float RestTime = 5f;
    public float PettingTime = 5f;
    public float EattingTime = 5f;
    public float range;

    [Header("<----BOOL VALUE---->")]
    public bool isResting;
    public bool Eating;
    public bool IsGoing;
    public bool isMoveable = true;
    public bool isDogDie;
    public bool isDogRunning;
    [Header("<----EXTRAS---->")]
    public Transform centrePoint;
    public NavMeshAgent agent;
    public Animator DogAnim;

    public Transform dogEndpoint;

    #endregion
    #region PRIVATE VARIABLES

    Transform center;
    bool PositionGet;
    float wait;
    float stopDistance;

    #endregion

    #region BUILT IN FUNCTIONS

    private void Awake()
    {
        m_GameManager = GameManager.Instance;
    }

    private void Start()
    {
        wait = RestTime;
        center = centrePoint;
        stopDistance = agent.stoppingDistance;
    }
    private void Update()
    {
        if (!isDogDie && m_GameManager.m_Food.dogIsDie)
        {
            isDogDie = m_GameManager.m_Food.dogIsDie;
            MakeTheDogDie();
        }

        if (!isMoveable) return;

        if(!isDogRunning && m_GameManager.m_Happy.isDogstartRunning)
        {
            isDogRunning = m_GameManager.m_Happy.isDogstartRunning;
        }

        if (agent.remainingDistance <= agent.stoppingDistance && PositionGet && !isResting)
        {
            agent.isStopped = true;
            //Debug.Log("cango2");
            isResting = true;
            if(Eating)
            {
                EattingDog();
            }
        }
        else if (isResting)
        {
            agent.isStopped = true;
            DogAnim.SetBool("Idle", true);
            wait -= Time.deltaTime;
            if (wait <= 0f)
            {
                agent.isStopped = false;
                isResting = false;
                PositionGet = false;
                IsGoing = false;
            }
        }
        else if (!IsGoing)
        {
            Vector3 point;
            if (RandomPoint(center.position, range, out point))
            {
                IsGoing = true;
                //Debug.DrawRay(point, Vector3.up, Color.blue, 10.0f);
                DogAnim.SetBool("Idle", false);
                DogAnim.SetBool("Walk", true);
                agent.isStopped = false;
                PositionGet = true;
                agent.SetDestination(point);
                wait = RestTime;
            }
        }
        else if (isDogRunning)
        {
            DogAnim.SetBool("Idle", false);
            DogAnim.SetBool("Walk", false);
            DogAnim.SetBool("Run", true);
            agent.SetDestination(dogEndpoint.transform.position);
            agent.speed = 3.5f;
            isMoveable = false;
        }
    }
    #endregion
    #region CUSTOM FUNCTION
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(GameManager.Instance.FoodFull && GameManager.Instance.m_Food.slider.value<1f)
        {
            randomPoint = GameManager.Instance.m_Player.FoodPoint.transform.position;
            agent.stoppingDistance = 1f;
            Eating = true;
        }
        else
        {
            Eating = false;
            agent.stoppingDistance = stopDistance;
        }
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }

    }
    public void PettingDog()
    {
        PettingStart.Play();
        GameManager.Instance.m_Happy.elapsedTime = 0f;
        StartCoroutine(StartPetting());
    }
    IEnumerator StartPetting()
    {
        if (GameManager.Instance.m_Happy.slider.value < 1f)
        {
            GameManager.Instance.m_Happy.slider.DOValue(GameManager.Instance.m_Happy.slider.value + 0.2f, PettingTime);
        }
        else
        {
            Debug.Log("Silder value is already reached to MaX");
        }
        yield return new WaitForSeconds(PettingTime);
        PettingDone.Play();
        PettingStart.Stop();
        GameManager.Instance.m_Player.IsInteracting = false;
    }
    public void MakeTheDogDie()
    {
        Debug.LogError("Die funcaiton called..");
        agent.isStopped = true;

        DogAnim.SetBool("Idle", false);
        DogAnim.SetBool("Walk", false);
        DogAnim.SetBool("Die", true);

        isMoveable = false;
        isResting = true;

        StartCoroutine(Show_OverPanel(2.5f));
    }
    IEnumerator Show_OverPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Time.timeScale
        m_GameManager.overPanel.SetActive(true);
    }
    public void MakeTheDogRun()
    {

    }
    public void StopDog()
    {
        agent.isStopped = true;
        DogAnim.SetBool("Idle", true);
        isResting = true;
        isMoveable = false;
    }
    public void StartDogMovement()
    {
        agent.isStopped = false;
        isResting = false;
        isMoveable = true;
        DogAnim.SetBool("Idle", false);
        DogAnim.SetBool("Walk", true);

    }
    public void EattingDog()
    {
        GameManager.Instance.m_Food.elapsedTime = 0f;

        if (GameManager.Instance.m_Food.slider.value < 1)
        {
            GameManager.Instance.m_Food.slider.DOValue(GameManager.Instance.m_Food.slider.value + 0.2f, EattingTime);
        }
        else
        {
            Debug.Log("Silder value is already reached to MaX");
        }

        GameManager.Instance.FoodFull = false;
        StartCoroutine(delay_to_disable_food());
    }
    IEnumerator delay_to_disable_food()
    {
        yield return new WaitForSeconds(2f);
        m_GameManager.m_Food.foodobject.SetActive(false);

    }
    #endregion
}
