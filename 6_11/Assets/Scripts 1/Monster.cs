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

    public float HP; // ü��
    public float attackDamage; // ���� ������
    public float attackDist; // ���� ����
    public float attackRate; // ���� �ӵ�
    public float nextAttackTime = 0f;

    public float MoveSpeed; // �̵��ӵ�
    public float traceDist; // ���� ����

    public int scoreValue; // ���Ͱ� �׾��� �� ��� ����
    public int goldValue; // ���Ͱ� �׾��� �� ��� ���

    private bool isDead = false; // ���� ����

   

    // Start�� ù ������ ������Ʈ ���� ȣ��˴ϴ�.
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
            Debug.LogError("Player�� ã�� �� �����ϴ�. Player ���� ������Ʈ�� 'Player' �±װ� �ִ��� Ȯ���ϼ���.");
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
                        // �÷��̾�� �������� �ִ� ����
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

                    yield return new WaitForSeconds(2.0f); // �״� �ִϸ��̼� ��� ���

                    
                    break;
            }
            yield return null;
        }
    }

    // Update�� �� �����Ӹ��� ȣ��˴ϴ�.
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
        // ���� �� ó�� ����
        GameManager.Instance.AddScore(scoreValue);
        GameManager.Instance.AddGold(goldValue);

        Destroy(gameObject);
    }
}
