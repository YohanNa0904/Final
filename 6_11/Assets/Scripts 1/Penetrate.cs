using UnityEngine;

public class Penetrate : MonoBehaviour
{
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private float maxPenetrationDistance = 100f;

    void Start()
    {
        // 오브젝트가 생성될 때 바로 정면 방향으로 레이를 발사하여 몬스터에게 데미지를 줍니다.
        PenetrateMonsters();
    }

    void PenetrateMonsters()
    {
        // 오브젝트의 위치와 정면 방향으로 레이 생성
        Ray ray = new Ray(transform.position, transform.forward);

        // 정면 방향의 모든 몬스터를 검출
        RaycastHit[] hits = Physics.RaycastAll(ray, maxPenetrationDistance, LayerMask.GetMask("Monster"));

        // 검출된 몬스터에게 데미지를 줌
        foreach (RaycastHit hit in hits)
        {
            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damageAmount);
            }
        }
    }
}
