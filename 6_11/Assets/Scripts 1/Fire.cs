using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // 데미지 값
    [SerializeField] private float damageAmountPerSecond = 10f;
    // 데미지 지속 시간
    [SerializeField] private float damageDuration = 10f;
    // 충돌 범위
    [SerializeField] private float fireRadius = 5f;

    // 스크립트가 활성화될 때 호출됩니다.
    private void OnEnable()
    {
        // 데미지를 주는 코루틴을 시작합니다.
        StartCoroutine(DealDamageOverTime());
    }

    private IEnumerator DealDamageOverTime()
    {
        float elapsed = 0f;

        // 데미지를 지속 시간 동안 주기적으로 줍니다.
        while (elapsed < damageDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fireRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    // 충돌한 몬스터에게 데미지를 줍니다.
                    Monster monster = collider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damageAmountPerSecond);
                    }
                }
            }

            // 1초 기다립니다.
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        // 데미지 지속 시간이 끝나면 오브젝트를 제거합니다.
        Destroy(gameObject);
    }

    // 충돌 범위를 시각화하기 위해 기즈모를 표시합니다.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }
}
