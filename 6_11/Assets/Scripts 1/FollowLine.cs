using UnityEngine;

public class FollowLine : MonoBehaviour
{
    public Transform target; // ���� ��� ������Ʈ

    private LineRenderer lineRenderer; // ���� ������ ������Ʈ

    void Start()
    {
        // ���� ������ �������� ����
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // ���� ���� ���� (������)
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        // ���� �β� ���� (0.01)
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        // ���� �������� ���� ���� (�⺻ ���� ���)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // ó�� ������ �� ���� ��Ȱ��ȭ
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // LineRenderer�� target�� null�� �ƴ��� Ȯ��
        if (lineRenderer != null && target != null)
        {
            // ������ ������ ���� (�ش� ��ü�� ��ġ)
            lineRenderer.SetPosition(0, transform.position);
            // ������ ���� ���� (Ÿ�� ��ü�� ��ġ)
            lineRenderer.SetPosition(1, target.position);

            // ó�� ������ �� ���� Ȱ��ȭ
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
        }
        else
        {
            // Ÿ���� �������� ���� ��� ���� ��Ȱ��ȭ
            if (lineRenderer != null && lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }
}
