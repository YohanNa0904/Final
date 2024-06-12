using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsteA : MonoBehaviour
{
    public NavMeshAgent nvAgent;
    public GameObject player;
    private Animator _animator;
    public enum CurrentState {idle, trace, attack, dead};
    public CurrentState curState = CurrentState.idle;
    private Transform _transform;
    private Transform playerTransform;
    public float traceDist = 15.0f;
    public float attackDist = 3.2f;
    private bool isDead = false;
    public float HP = 1f;    
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
        while(!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTransform.position, _transform.position);
            if(dist < attackDist)
            {
                curState = CurrentState.attack;
            }
            else if (dist <= traceDist)
            {
                curState = CurrentState.trace;
            }
            else{
                curState = CurrentState.dead;
            }
        }
    }
    IEnumerator CheckStateForAction()
    {
        while(!isDead)
        {
            switch (curState)
            {
                case CurrentState.idle:
                nvAgent.Stop();
                _animator.SetBool("isTrace",false);
                break;
                case CurrentState.trace:
                nvAgent.destination = playerTransform.position;
                nvAgent.Resume();
                _animator.SetBool("isTrace",true);
                break;
                case CurrentState.attack:
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
        // 몬스터가 죽을 때의 로직 (예: 몬스터 삭제, 애니메이션 재생 등)
        Destroy(gameObject);
    }
}
