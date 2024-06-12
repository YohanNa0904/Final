using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;
    public float maxDistance = 0.5f;

    void Start()
    {
        // Rigidbody ������Ʈ�� �������� �ʱ� ��ġ�� �����մϴ�.
        rb = GetComponent<Rigidbody>();
        initialPosition = rb.position;
    }

    void FixedUpdate()
    {
        // ������Ʈ�� �̵��� �Ÿ��� ����մϴ�.
        Vector3 offset = rb.position - initialPosition;

        // �̵��� �Ÿ��� �ִ� �Ÿ��� �ʰ��ϴ��� Ȯ���մϴ�.
        if (offset.magnitude > maxDistance)
        {
            // �ִ� �Ÿ��� �ʰ��ߴٸ�, �ʰ����� �ʵ��� ��ġ�� �����մϴ�.
            Vector3 clampedPosition = initialPosition + offset.normalized * maxDistance;
            rb.MovePosition(clampedPosition);
        }
    }
}
