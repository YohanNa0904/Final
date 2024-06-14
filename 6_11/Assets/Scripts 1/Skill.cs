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
        [SerializeField] private LineRenderer lineRendererPrefab; // LineRenderer 프리팹

        private Transform transform;
        private LineRenderer lineRendererInstance;

        public void Initialize(Transform transform)
        {
            this.transform = transform;
            if (lineRendererPrefab != null)
            {
                lineRendererInstance = GameObject.Instantiate(lineRendererPrefab);
                lineRendererInstance.positionCount = 0;
                lineRendererInstance.enabled = false;
                InitializeLineRenderer();
            }
        }

        private void InitializeLineRenderer()
        {
            lineRendererInstance.startColor = Color.blue;
            lineRendererInstance.endColor = Color.blue;
            lineRendererInstance.startWidth = 0.2f; // 두께 조정
            lineRendererInstance.endWidth = 0.2f;   // 두께 조정
        }

        public void PenetrateMonsters(Vector3 origin, Vector3 direction)
        {
            float offset = 0.1f; // 예시로 0.1f만큼 떨어뜨리겠습니다.
            Vector3 adjustedOrigin = origin - direction * offset;

            Ray ray = new Ray(adjustedOrigin, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, maxPenetrationDistance, LayerMask.GetMask("Monster"));

            List<Vector3> points = new List<Vector3>();
            points.Add(adjustedOrigin);

            foreach (RaycastHit hit in hits)
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damageAmount);
                    points.Add(hit.point);
                }
                if (arrowPrefab != null)
                {
                    Instantiate(arrowPrefab, origin, Quaternion.LookRotation(ray.direction));
                }
            }

            if (points.Count > 1)
            {
                DrawLines(points);
            }
        }

        private void DrawLines(List<Vector3> points)
        {
            if (lineRendererInstance != null)
            {
                lineRendererInstance.positionCount = points.Count;
                lineRendererInstance.SetPositions(points.ToArray());
                lineRendererInstance.enabled = true;
                transform.GetComponent<MonoBehaviour>().StartCoroutine(DisableLineRendererAfterTime(0.1f));
            }
        }

        private IEnumerator DisableLineRendererAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (lineRendererInstance != null)
            {
                lineRendererInstance.enabled = false;
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

        public void Initialize(Transform transform)
        {
            this.transform = transform;
        }

        public void Explode(Transform hitTransform)
        {
            Collider[] colliders = Physics.OverlapSphere(hitTransform.position, explosionRadius);
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, hitTransform.position, Quaternion.Euler(-90, 0, 0));
            }
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    Monster monster = collider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damageAmount);
                    }
                }
            }
        }

        public void DrawGizmos(Vector3 hitPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPoint, explosionRadius);
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

        public void Initialize(Transform transform)
        {
            this.transform = transform;
        }

        public IEnumerator FreezeOverTime(Vector3 hitPoint)
        {
            float elapsed = 0f;
            GameObject freezeInstance = null;

            if (freezePrefab != null)
            {
                freezeInstance = Instantiate(freezePrefab, hitPoint, Quaternion.identity);
            }

            while (elapsed < damageDuration)
            {
                Collider[] colliders = Physics.OverlapSphere(hitPoint, fireRadius);
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

            originalSpeeds.Clear();

            if (freezeInstance != null)
            {
                Destroy(freezeInstance);
            }
        }

        public void DrawGizmos(Vector3 hitPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPoint, fireRadius);
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

        public void Initialize(Transform transform)
        {
            this.transform = transform;
        }

        public IEnumerator DealDamageOverTime(Vector3 hitPoint)
        {
            float elapsed = 0f;
            GameObject fireInstance = null;

            if (firePrefab != null)
            {
                fireInstance = Instantiate(firePrefab, hitPoint, Quaternion.Euler(-90, 0, 0));
            }

            while (elapsed < damageDuration)
            {
                Collider[] colliders = Physics.OverlapSphere(hitPoint, fireRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                    {
                        Monster monster = collider.GetComponent<Monster>();
                        if (monster != null)
                        {
                            monster.TakeDamage(damageAmountPerSecond);
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
                elapsed += 1f;
            }

            if (fireInstance != null)
            {
                Destroy(fireInstance);
            }
        }

        public void DrawGizmos(Vector3 hitPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPoint, fireRadius);
        }
    }

    public Penetrate penetrateSkill = new Penetrate();
    public Explosion explosionSkill = new Explosion();
    public Freeze freezeSkill = new Freeze();
    public Fire fireSkill = new Fire();

    private void Awake()
    {
        penetrateSkill.Initialize(transform);
        explosionSkill.Initialize(transform);
        freezeSkill.Initialize(transform);
        fireSkill.Initialize(transform);
    }

    private void OnDrawGizmosSelected()
    {
        if (explosionSkill != null)
        {
            explosionSkill.DrawGizmos(transform.position);
        }

        if (freezeSkill != null)
        {
            freezeSkill.DrawGizmos(transform.position);
        }

        if (fireSkill != null)
        {
            fireSkill.DrawGizmos(transform.position);
        }
    }
}
