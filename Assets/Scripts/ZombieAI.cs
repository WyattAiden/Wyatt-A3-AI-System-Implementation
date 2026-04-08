using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class ZombieAI : MonoBehaviour
{
    public enum State { Sleep, Idle, Move, Search, Follow, AlertOthers, Attack }



    public Transform character;
    public Transform[] wayponts;
    public GameObject alertObg;
    public ZombieAI Leader;
    

    public float IdleTimeEnd = 1.0f;
    public float WaypointTreshold = 1.1f;
    public float viewRadius = 10f;
    public float vewAngle = 60f;
    public float searchTimeThreshold = 5.0f;
    public float DistinceHeard = 10.0f;
    public float Followspeed = 2f;
    public float Normalspeed = 3.5f;


    NavMeshAgent Zom;
    State state;
    int MoveIndex = 0;
    

    float IdleTime;
    float searchTime;
    bool viewEnabled = false;
    bool canSeePlayer = false;
    public bool ZominRange = false;
    Vector3 LastKnownPos = Vector3.zero;

    bool Soundhaerd = false;
    Vector3 SoundLocal = Vector3.zero;

    private void Awake()
    {
        Zom = GetComponent<NavMeshAgent>();
        
    }

    private void Start()
    {
        state = State.Sleep;
    }

    public void Update()
    {
        switch (state)
        {
            case State.Sleep:
                Sleep();
                break;
            case State.Idle:
                Idle();
                break;
            case State.Move:
                Move();
                break;
            case State.Search:
                Search();
                break;
            case State.Follow:
                Follow();
                break;
            case State.AlertOthers:
                AlertOthers();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    public void SoundTriggered(SoundObgect soundObgect)
    {
        if (Vector3.Distance(soundObgect.transform.position, gameObject.transform.position) <= DistinceHeard)
        {
            SoundLocal = soundObgect.transform.position;
            Soundhaerd = true;
        }
        
        
    }

    void Sleep()
    {
        viewEnabled = false;

        if (Soundhaerd)
        {
            state = State.Search;
            searchTime = Time.time;
        }
        if (ZominRange == true)
        {
            state = State.Follow;
        }
    }
    
    void Idle()
    {
        viewEnabled = true;
        float idleTimeElapsed = Time.time - IdleTime;
        if (idleTimeElapsed >= IdleTimeEnd)
        {
            state = State.Move;
        }
        canSeePlayer = InViewCone();
        if (canSeePlayer)
        {
            state = State.AlertOthers;
        }
        if (ZominRange == true)
        {
            state = State.Follow;
        }
    }

    void Move()
    {
        Zom.speed = Normalspeed;
        Vector3 waypoint = wayponts[MoveIndex].position;

        Zom.SetDestination(waypoint);

        viewEnabled = true;
        canSeePlayer = InViewCone();
        alertObg.SetActive(false);

        if (Vector3.Distance(transform.position, waypoint) < WaypointTreshold)
        {
            MoveIndex++;
            if (MoveIndex >= wayponts.Length) MoveIndex = 0;

            state = State.Idle;
            IdleTime = Time.time;
        }
        
        if (canSeePlayer)
        {
            state = State.AlertOthers;
        }
        if (ZominRange == true)
        {
            state = State.Follow;
        }
    }

    void Search()
    {
        Zom.speed = Normalspeed;
        float searchTimeElapsed = Time.time - searchTime;
        
        transform.Rotate(Vector3.up, Mathf.Sin(Time.time * 4) * Time.deltaTime * 200f);
        canSeePlayer = InViewCone();
        if (Soundhaerd)
        {
            Zom.SetDestination(SoundLocal);
        }
        else 
        {
            Zom.SetDestination(LastKnownPos);
        }
       
        viewEnabled = true;
        if (canSeePlayer)
        {
            state = State.AlertOthers;
        }

        if (searchTimeElapsed >= searchTimeThreshold)
        {
            state = State.Move;
            Soundhaerd = false;
        }
    }

    void AlertOthers()
    {
        alertObg.SetActive(true);
        state = State.Attack;
    }

    void Attack()
    {
        Zom.speed = Normalspeed;
        Zom.SetDestination(character.position);

        canSeePlayer = InViewCone();
        if (!canSeePlayer)
        {
            state = State.Search;
            Soundhaerd = false;
            searchTime = Time.time;
        }
        
    }

    void Follow()
    {
        Zom.speed = Followspeed;
        Zom.SetDestination(Leader.transform.position);
        canSeePlayer = InViewCone();
        viewEnabled = true;
        if (canSeePlayer)
        {
            state = State.Attack;
        }
    }

    bool InViewCone()
    {
        if (Vector3.Distance(transform.position, character.transform.position) > viewRadius)
            return false;

        Vector3 npcToCharacter = character.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, npcToCharacter) > 0.5f * vewAngle)
            return false;

        Vector3 toCaracterDir = npcToCharacter.normalized;
        if (Physics.Raycast(transform.position, toCaracterDir, out RaycastHit ray, viewRadius))
        {
            bool Resalt = ray.transform == character.transform;
            if (Resalt)
            {
                LastKnownPos = character.transform.position;
            }
            return Resalt;
        }


        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform WaypointTransform in wayponts)
        {
            Gizmos.DrawWireSphere(WaypointTransform.position, 0.5f);
        }

        if (viewEnabled)
        {
            Handles.color = new Color(0f, 1f, 1f, 0.25f);

            if (canSeePlayer) Handles.color = new Color(1f, 0f, 0f, 0.25f);

            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, vewAngle / 2, viewRadius);
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -vewAngle / 2, viewRadius);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Charicter>())
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
