using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [System.Serializable]
    public class Penetrate
    {
        [SerializeField] private float damageAmount = 100f;
        [SerializeField] private float maxPenetrationDistance = 100f;
        [SerializeField] private GameObject arrowPrefab;

        private Transform transform;

        public Penetrate(Transform transform)
        {
            this.transform = transform;
        }

        public void PenetrateMonsters(Vector3 origin, Vector3 direction)
        {
            Ray ray = new Ray(origin, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, maxPenetrationDistance, LayerMask.GetMask("Monster"));

            foreach (RaycastHit hit in hits)
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damageAmount);
                }

                if (arrowPrefab != null)
                {
                    Instantiate(arrowPrefab, hit.point, Quaternion.LookRotation(ray.direction));
                }
            }
        }
    }

    [System.Serializable]
    public class Explosion
    {
        [SerializeField] private float damageAmount = 100f;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private GameObject explosionPrefab;

        private Transform transform;

        public Explosion(Transform transform)
        {
            this.transform = transform;
        }

        public void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    Monster monster = collider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damageAmount);
                    }

                    if (explosionPrefab != null)
                    {
                        Instantiate(explosionPrefab, collider.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        public void DrawGizmos()
        {
            if (transform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, explosionRadius);
            }
        }
    }

    [System.Serializable]
    public class Freeze
    {
        [SerializeField] private float damageDuration = 10f;
        [SerializeField] private float fireRadius = 5f;
        [SerializeField] private GameObject freezePrefab;

        private Transform transform;
        private Dictionary<Monster, float> originalSpeeds = new Dictionary<Monster, float>();

        public Freeze(Transform transform)
        {
            this.transform = transform;
        }

        public IEnumerator FreezeOverTime()
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

                            if (freezePrefab != null)
                            {
                                Instantiate(freezePrefab, collider.transform.position, Quaternion.identity);
                            }
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

            originalSpeeds.Clear();
        }

        public void DrawGizmos()
        {
            if (transform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, fireRadius);
            }
        }
    }

    [System.Serializable]
    public class Fire
    {
        [SerializeField] private float damageAmountPerSecond = 10f;
        [SerializeField] private float damageDuration = 10f;
        [SerializeField] private float fireRadius = 5f;
        [SerializeField] private GameObject firePrefab;

        private Transform transform;

        public Fire(Transform transform)
        {
            this.transform = transform;
        }

        public IEnumerator DealDamageOverTime()
        {
            float elapsed = 0f;

            while (elapsed < damageDuration)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, fireRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                    {
                        Monster monster = collider.GetComponent<Monster>();
                        if (monster != null)
                        {
                            monster.TakeDamage(damageAmountPerSecond);

                            if (firePrefab != null)
                            {
                                Instantiate(firePrefab, collider.transform.position, Quaternion.identity);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
                elapsed += 1f;
            }
        }

        public void DrawGizmos()
        {
            if (transform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, fireRadius);
            }
        }
    }

    public Penetrate penetrateSkill;
    public Explosion explosionSkill;
    public Freeze freezeSkill;
    public Fire fireSkill;

    private void Awake()
    {
        penetrateSkill = new Penetrate(transform);
        explosionSkill = new Explosion(transform);
        freezeSkill = new Freeze(transform);
        fireSkill = new Fire(transform);
    }

    private void OnDrawGizmosSelected()
    {
        if (explosionSkill != null)
        {
            explosionSkill.DrawGizmos();
        }

        if (freezeSkill != null)
        {
            freezeSkill.DrawGizmos();
        }

        if (fireSkill != null)
        {
            fireSkill.DrawGizmos();
        }
    }
}
