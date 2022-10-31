using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public enum State { IDLE, TRACE, ATTACK, DIE }
    public State state = State.IDLE;

    public float traceDist = 50.0f;
    public float attackDist = 2.0f;
    public bool isDie = false;
    public GameObject bullet;
    
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject bloodEffect;
    private float hp = 100.0f;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashDanceSpeed = Animator.StringToHash("DanceSpeed");
    private readonly int hashDanceOffset = Animator.StringToHash("DanceOffset");
    private readonly int hashDie = Animator.StringToHash("Die");

    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = playerTr.position;
        
    }

   

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.3f);
            if (state == State.DIE) yield break;
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    agent.isStopped = true;
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;
                    Collider[] colls = GetComponentsInChildren<SphereCollider>();
                    foreach (var collider in colls)
                    {
                        collider.enabled = false;
                    }
                    yield return new WaitForSeconds(3.0f);
                    state = State.IDLE;
                    hp = 100.0f;
                    isDie = false;
                    GetComponent<CapsuleCollider>().enabled = true;
                    foreach (var collider in colls)
                    {
                        collider.enabled = true;
                    }
                    this.gameObject.SetActive(false);
                    break;                
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            Destroy(coll.gameObject);
            //anim.SetTrigger(hashHit);
            //Vector3 pos = coll.GetContact(0).point;
            //Quaternion rot = Quaternion.LookRotation(-coll.GetContact(0).normal);
            //ShowBloodEffect(pos, rot);
            //hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            //if (hp <= 0.0f)
            //{
            //    state = State.DIE;
            //    GameManager.instance.DisplayScore(50);
            //}
        }
    }

    public void OnDamage(Vector3 pos, Vector3 normal)
    {
        anim.SetTrigger(hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);
        ShowBloodEffect(pos, rot);
        hp -= bullet.gameObject.GetComponent<BulletCtrl>().damage;
        if (hp <= 0.0f)
        {
            state = State.DIE;
            GameManager.instance.DisplayScore(50);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
    }

    private void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        else if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        agent.isStopped = true;
        anim.SetFloat(hashDanceSpeed, Random.Range(0.8f, 1.2f));
        anim.SetFloat(hashDanceOffset, Random.Range(0.0f, 1.0f));
        anim.SetTrigger(hashPlayerDie);
        //anim.GetCurrentAnimatorStateInfo(0).
    }
}
