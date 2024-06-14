using MyGameNamespace;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public NavMeshAgent nvAgent;
    private Animator _animator;
    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState curState = CurrentState.idle;

    private Transform _transform;
    private Transform playerTransform;

    public float HP; // 체력
    public float attackDamage; // 공격 데미지
    public float attackDist; // 공격 범위
    public float attackRate; // 공격 속도
    public float nextAttackTime = 0f;

    public float MoveSpeed; // 이동속도
    public float traceDist; // 추적 범위

    public int scoreValue; // 몬스터가 죽었을 때 얻는 점수
    public int goldValue; // 몬스터가 죽었을 때 얻는 골드

    private bool isDead = false; // 죽음 여부

   

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    protected virtual void Start()
    {
        _transform = GetComponent<Transform>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        else
        {
            Debug.LogError("Player를 찾을 수 없습니다. Player 게임 오브젝트에 'Player' 태그가 있는지 확인하세요.");
            return;
        }

        _animator = GetComponent<Animator>();
        nvAgent = GetComponent<NavMeshAgent>();

       

        StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTransform.position, _transform.position);
            //float dist = (playerTransform.position - _transform.position).sqrMagnitude;
            if (dist < attackDist)
            {
                curState = CurrentState.attack;
            }
            else
            {
                curState = CurrentState.trace;
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
                    if (nvAgent.isOnNavMesh)
                    {
                        nvAgent.isStopped = true;
                    }
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", false);
                    break;

                case CurrentState.trace:
                    if (nvAgent.isOnNavMesh)
                    {
                        Debug.Log(curState);
                        nvAgent.isStopped = false;
                        nvAgent.destination = playerTransform.position;
                    }
                    _animator.SetBool("isTrace", true);
                    _animator.SetBool("isAttack", false);
                    break;
                case CurrentState.attack:
                    if (nvAgent.isOnNavMesh)
                    {
                        nvAgent.isStopped = true;
                    }
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", true);
                    if (Time.time >= nextAttackTime)
                    {
                        // 플레이어에게 데미지를 주는 로직
                        nextAttackTime = Time.time + attackRate;
                    }
                    break;
                case CurrentState.dead:
                    if (nvAgent.isOnNavMesh)
                    {
                        nvAgent.isStopped = true;
                    }
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", false);
                    _animator.SetTrigger("isDead");

                    yield return new WaitForSeconds(2.0f); // 죽는 애니메이션 재생 대기

                    
                    break;
            }
            yield return null;
        }
    }

    // Update는 매 프레임마다 호출됩니다.
    protected virtual void Update()
    {
        if (curState == CurrentState.trace && playerTransform != null)
        {
            if (nvAgent.isOnNavMesh)
            {
                nvAgent.SetDestination(playerTransform.position);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        // 죽음 후 처리 로직
        GameManager.Instance.AddScore(scoreValue);
        GameManager.Instance.AddGold(goldValue);

        Destroy(gameObject);
    }
}
