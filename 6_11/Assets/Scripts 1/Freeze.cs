using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [SerializeField] private float damageDuration = 10f;
    [SerializeField] private float fireRadius = 5f;

    private Dictionary<Monster, float> originalSpeeds = new Dictionary<Monster, float>();

    private void OnEnable()
    {
        StartCoroutine(FreezeOverTime());
    }

    private IEnumerator FreezeOverTime()
    {
        float elapsed = 0f;

        while (elapsed < damageDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fireRadius);
            HashSet<Monster> affectedMonsters = new HashSet<Monster>();

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    Monster monster = collider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        if (!originalSpeeds.ContainsKey(monster))
                        {
                            originalSpeeds[monster] = monster.MoveSpeed;
                        }

                        monster.MoveSpeed = originalSpeeds[monster] / 2;
                        affectedMonsters.Add(monster);
                    }
                }
            }

            List<Monster> monstersToRemove = new List<Monster>();
            foreach (var entry in originalSpeeds)
            {
                if (!affectedMonsters.Contains(entry.Key))
                {
                    entry.Key.MoveSpeed = entry.Value;
                    monstersToRemove.Add(entry.Key);
                }
            }
            foreach (Monster monster in monstersToRemove)
            {
                originalSpeeds.Remove(monster);
            }

            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        foreach (var entry in originalSpeeds)
        {
            entry.Key.MoveSpeed = entry.Value;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }
    void SomeMethod(Monster monster)
    {
        // monster.MoveSpeed 속성에 접근할 수 있습니다.
        float speed = monster.MoveSpeed;
        // 추가적인 로직 구현
    }
}
