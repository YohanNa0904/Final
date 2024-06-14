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

    public float HP = 100f; // 체력 (기본값 설정)
    public float attackDamage = 10f; // 공격 데미지 (기본값 설정)
    public float attackDist = 2.0f; // 공격 범위 (기본값 설정)
    public float attackRate = 1.0f; // 공격 속도 (기본값 설정)
    public float nextAttackTime = 0f;

    public float traceDist = 10.0f; // 추적 범위 (기본값 설정)
    public float MoveSpeed = 3.5f; // 이동속도 (기본값 설정)

    public int scoreValue = 50; // 몬스터가 죽었을 때 얻는 점수
    public int goldValue = 20; // 몬스터가 죽었을 때 얻는 골드

    private bool isDead = false; // 죽음 여부

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
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
            Debug.LogError("Player를 찾을 수 없습니다. Player 게임 오브젝트에 'Player' 태그가 있는지 확인하세요.");
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
                Debug.LogError("플레이어 트랜스폼을 찾을 수 없습니다.");
                continue;
            }

            float dist = Vector3.Distance(playerTransform.position, transform.position);
            Debug.Log($"현재 상태: {curState}, 플레이어와 거리: {dist}");

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
            Debug.Log($"현재 상태: {curState}");
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

                    yield return new WaitForSeconds(2.0f); // 죽는 애니메이션 재생 대기

                    break;
            }
            yield return null;
        }
    }

    void AttackPlayer()
    {
        if (playerHP != null)
        {
            Debug.Log("플레이어에게 데미지를 입힘");
            playerHP.TakeDamage((int)attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"데미지를 받음, 남은 체력: {HP}");
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
        Debug.Log("몬스터가 죽음");

        Destroy(gameObject);
    }

    // Update는 매 프레임마다 호출됩니다.
    protected virtual void Update()
    {
        // 추가적으로 Update 메서드가 필요한 경우 여기에 구현합니다.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHP.TakeDamage((int)attackDamage);
        }
    }
}
