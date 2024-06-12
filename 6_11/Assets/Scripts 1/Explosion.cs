using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // 데미지 값
    [SerializeField] private float damageAmount = 20f;
    // 충돌 범위
    [SerializeField] private float explosionRadius = 5f;

    // 스크립트가 활성화되고 있는 동안 매 프레임마다 호출됩니다.
    private void Update()
    {
        // 주변에 있는 모든 몬스터를 찾아서 데미지를 주고 자신을 제거합니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                // 충돌한 몬스터에게 데미지를 줍니다.
                Monster monster = collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damageAmount);
                }
            }
        }

        // 스크립트가 자신을 제거합니다.
        Destroy(gameObject);
    }

    // 폭발 범위를 시각화하기 위해 기즈모를 표시합니다.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
