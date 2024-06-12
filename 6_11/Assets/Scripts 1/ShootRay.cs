using UnityEngine;

public class ShootRay : MonoBehaviour
{
    public Hand hand;
    public Quiver quiver;
    [SerializeField] private LineRenderer lineRenderer;
    public float damageAmount = 100f; // ������ ��
    public int skillIndex = 0; // ��ų �ε���
    [SerializeField] private Skill skill; // Skill ��ũ��Ʈ ����

    private int monsterLayer;

    void Start()
    {
        if (lineRenderer != null)
        {
            // LineRenderer ������Ʈ �ʱ�ȭ
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }

        // "Monster" ���̾��� �ε��� ��������
        monsterLayer = LayerMask.NameToLayer("Monster");
    }

    public void Shoot(float n)
    {
        Ray ray = new Ray(transform.position, transform.forward); // ������Ʈ�� ��ġ�� ���� �������� ���� ����
        RaycastHit hit;

        // ���̰� �ִ� �Ÿ� n���� �浹 ���θ� �˻�
        if (Physics.Raycast(ray, out hit, n))
        {
            // ���̰� �浹�� ������ ����
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);

            // �浹�� ��ü�� ���̾ "Monster"�� ��� �������� ��
            if (hit.collider.gameObject.layer == monsterLayer)
            {
                hand.setArrow = false;
                quiver.arrowList[skillIndex] -= 1;
                // �浹�� ���Ϳ��� �������� ��
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damageAmount);
                }

                // ��ų ����
                SkillAction(skillIndex, ray.origin, ray.direction);
            }
        }
        else
        {
            // ���̰� �ƹ� ��ü�͵� �浹���� �ʾ��� ��
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * n*100); // �ִ� �Ÿ� n �������� ����
        }

        // ���̸� �ð������� ���̵��� ����
        lineRenderer.enabled = true;

        // 0.1�� �Ŀ� ���̸� �ٽ� ��Ȱ��ȭ�Ͽ� ������� ��
        Invoke("DisableLineRenderer", 0.1f);
    }

    private void SkillAction(int n, Vector3 origin, Vector3 direction)
    {
        switch (n)
        {
            case 0:
                // ��ų 0: Penetrate ��ų �ߵ�
                skill.penetrateSkill.PenetrateMonsters(origin, direction);
                break;
            case 1:
                // ��ų 1: Explosion ��ų �ߵ�
                skill.explosionSkill.Explode();
                break;
            case 2:
                // ��ų 2: Freeze ��ų �ߵ�
                StartCoroutine(skill.freezeSkill.FreezeOverTime());
                break;
            case 3:
                // ��ų 3: Fire ��ų �ߵ�
                StartCoroutine(skill.fireSkill.DealDamageOverTime());
                break;
            case 4:
                // ��ų 4: �߰����� ��ų ����
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
