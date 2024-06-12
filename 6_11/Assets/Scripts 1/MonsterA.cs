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
    public float attackRate = 1.0f; // ���� �ӵ�
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
                    _animator.SetTrigger("isDead"); // "isDead" Ʈ���Ÿ� ȣ���Ͽ� �״� �ִϸ��̼� ���




                    // �״� �ִϸ��̼��� ����Ǵ� ���� ��ٸ� �� ������Ʈ�� ��Ȱ��ȭ �Ǵ� �����ϴ� ���� �߰� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
                    yield return new WaitForSeconds(2.0f); // ������ �ð� ���

                    // ���� ���� ��ó�� ���� �߰�
                    //gameObject.SetActive(false); // �Ǵ� Destroy(gameObject);
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