using MyGameNamespace;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private Animator _animator;
    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState curState = CurrentState.idle;

    private Transform playerTransform;
    private PlayerHP playerHP;

    public float HP = 100f; // ü�� (�⺻�� ����)
    public float attackDamage = 10f; // ���� ������ (�⺻�� ����)
    public float attackDist = 2.0f; // ���� ���� (�⺻�� ����)
    public float attackRate = 1.0f; // ���� �ӵ� (�⺻�� ����)
    public float nextAttackTime = 0f;

    public float traceDist = 10.0f; // ���� ���� (�⺻�� ����)
    public float MoveSpeed = 3.5f; // �̵��ӵ� (�⺻�� ����)

    public int scoreValue = 50; // ���Ͱ� �׾��� �� ��� ����
    public int goldValue = 20; // ���Ͱ� �׾��� �� ��� ���

    private bool isDead = false; // ���� ����

    // Start�� ù ������ ������Ʈ ���� ȣ��˴ϴ�.
    protected virtual void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
            playerHP = player.GetComponent<PlayerHP>();
        }
        else
        {
            Debug.LogError("Player�� ã�� �� �����ϴ�. Player ���� ������Ʈ�� 'Player' �±װ� �ִ��� Ȯ���ϼ���.");
            return;
        }

        _animator = GetComponent<Animator>();

        StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            if (playerTransform == null)
            {
                Debug.LogError("�÷��̾� Ʈ�������� ã�� �� �����ϴ�.");
                continue;
            }

            float dist = Vector3.Distance(playerTransform.position, transform.position);
            Debug.Log($"���� ����: {curState}, �÷��̾�� �Ÿ�: {dist}");

            if (dist < attackDist)
            {
                curState = CurrentState.attack;
            }
            else if (dist < traceDist)
            {
                curState = CurrentState.trace;
            }
            else
            {
                curState = CurrentState.idle;
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            Debug.Log($"���� ����: {curState}");
            switch (curState)
            {
                case CurrentState.idle:
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", false);
                    break;

                case CurrentState.trace:
                    _animator.SetBool("isTrace", true);
                    _animator.SetBool("isAttack", false);
                    break;

                case CurrentState.attack:
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", true);
                    if (Time.time >= nextAttackTime)
                    {
                        AttackPlayer();
                        nextAttackTime = Time.time + attackRate;
                    }
                    break;

                case CurrentState.dead:
                    _animator.SetBool("isTrace", false);
                    _animator.SetBool("isAttack", false);
                    _animator.SetTrigger("isDead");

                    yield return new WaitForSeconds(2.0f); // �״� �ִϸ��̼� ��� ���

                    break;
            }
            yield return null;
        }
    }

    void AttackPlayer()
    {
        if (playerHP != null)
        {
            Debug.Log("�÷��̾�� �������� ����");
            playerHP.TakeDamage((int)attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"�������� ����, ���� ü��: {HP}");
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
        Debug.Log("���Ͱ� ����");

        Destroy(gameObject);
    }

    // Update�� �� �����Ӹ��� ȣ��˴ϴ�.
    protected virtual void Update()
    {
        // �߰������� Update �޼��尡 �ʿ��� ��� ���⿡ �����մϴ�.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHP.TakeDamage((int)attackDamage);
        }
    }
}
