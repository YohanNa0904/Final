using UnityEngine;

public class ShootRay : MonoBehaviour
{
    public Hand hand;
    public Quiver quiver;
    [SerializeField] private LineRenderer lineRenderer;
    public float damageAmount = 100f; // 데미지 양
    public int skillIndex = 0; // 스킬 인덱스
    [SerializeField] private Skill skill; // Skill 스크립트 참조

    private int monsterLayer;

    void Start()
    {
        if (lineRenderer != null)
        {
            // LineRenderer 컴포넌트 초기화
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }

        // "Monster" 레이어의 인덱스 가져오기
        monsterLayer = LayerMask.NameToLayer("Monster");
    }

    public void Shoot(float n)
    {
        Ray ray = new Ray(transform.position, transform.forward); // 오브젝트의 위치와 전방 방향으로 레이 생성
        RaycastHit hit;

        // 레이가 최대 거리 n까지 충돌 여부를 검사
        if (Physics.Raycast(ray, out hit, n))
        {
            // 레이가 충돌한 지점을 설정
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);

            // 충돌한 물체의 레이어가 "Monster"인 경우 데미지를 줌
            if (hit.collider.gameObject.layer == monsterLayer)
            {
                hand.setArrow = false;
                quiver.arrowList[skillIndex] -= 1;
                // 충돌한 몬스터에게 데미지를 줌
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damageAmount);
                }

                // 스킬 실행
                SkillAction(skillIndex, ray.origin, ray.direction);
            }
        }
        else
        {
            // 레이가 아무 물체와도 충돌하지 않았을 때
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * n*100); // 최대 거리 n 지점으로 설정
        }

        // 레이를 시각적으로 보이도록 설정
        lineRenderer.enabled = true;

        // 0.1초 후에 레이를 다시 비활성화하여 사라지게 함
        Invoke("DisableLineRenderer", 0.1f);
    }

    private void SkillAction(int n, Vector3 origin, Vector3 direction)
    {
        switch (n)
        {
            case 0:
                // 스킬 0: Penetrate 스킬 발동
                skill.penetrateSkill.PenetrateMonsters(origin, direction);
                break;
            case 1:
                // 스킬 1: Explosion 스킬 발동
                skill.explosionSkill.Explode();
                break;
            case 2:
                // 스킬 2: Freeze 스킬 발동
                StartCoroutine(skill.freezeSkill.FreezeOverTime());
                break;
            case 3:
                // 스킬 3: Fire 스킬 발동
                StartCoroutine(skill.fireSkill.DealDamageOverTime());
                break;
            case 4:
                // 스킬 4: 추가적인 스킬 동작
                break;
            default:
                break;
        }
    }

    void DisableLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}
