using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterA : MonoBehaviour
{
    public NavMeshAgent nvAgent;
    public GameObject player;
    private Animator _animator;
    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState curState = CurrentState.idle;
    private Transform _transform;
    private Transform playerTransform;

    public float traceDist = 15.0f;
    public float attackDamge = 10.0f;
    public float attackDist = 3.2f;
    public float attackRate = 1.0f; // 공격 속도
    public float nextAttackTime = 0f;
    private bool isDead = false;




    // Start is called before the first frame update
    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _animator = this.gameObject.GetComponent<Animator>();
        if (nvAgent == null)
        {
            nvAgent = GetComponent<NavMeshAgent>();
        }
        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTransform.position, _transform.position);
            if (dist < attackDist)
            {
                curState = CurrentState.attack;
            }
            else if (dist <= traceDist)
            {
                curState = CurrentState.trace;
            }
            else
            {
                curState = CurrentState.dead;
            }
        }
    }
    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case CurrentState.idle:
                    nvAgent.Stop();
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", false);
                    break;
                case CurrentState.trace:
                    nvAgent.destination = playerTransform.position;
                    nvAgent.Resume();
                    _animator.SetBool("isTrace", true);
                    _animator.SetBool("isAttack", false);
                    break;
                case CurrentState.attack:
                    nvAgent.isStopped = true;
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", true);
                    if (Time.time >= nextAttackTime)
                    {
                        //Archor playerController = player.GetComponent<Archor>();
                        //if (playerController != null)
                        //{
                        //    playerController.TakeDamage(attackDamge);
                        //}

                        nextAttackTime = Time.time + attackRate;
                    }


                    break;
                case CurrentState.dead:
                    nvAgent.isStopped = true;
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", false);
                    _animator.SetTrigger("isDead"); // "isDead" 트리거를 호출하여 죽는 애니메이션 재생




                    // 죽는 애니메이션이 재생되는 동안 기다린 후 오브젝트를 비활성화 또는 제거하는 등의 추가 로직을 여기에 추가할 수 있습니다.
                    yield return new WaitForSeconds(2.0f); // 임의의 시간 대기

                    // 죽음 상태 후처리 로직 추가
                    //gameObject.SetActive(false); // 또는 Destroy(gameObject);
                    break;




            }
            yield return null;
        }
    }





    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            nvAgent.SetDestination(player.transform.position);
        }
    }

}