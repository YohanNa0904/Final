using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;
    public float maxDistance = 0.5f;

    void Start()
    {
        // Rigidbody 컴포넌트를 가져오고 초기 위치를 저장합니다.
        rb = GetComponent<Rigidbody>();
        initialPosition = rb.position;
    }

    void FixedUpdate()
    {
        // 오브젝트가 이동한 거리를 계산합니다.
        Vector3 offset = rb.position - initialPosition;

        // 이동한 거리가 최대 거리를 초과하는지 확인합니다.
        if (offset.magnitude > maxDistance)
        {
            // 최대 거리를 초과했다면, 초과하지 않도록 위치를 조정합니다.
            Vector3 clampedPosition = initialPosition + offset.normalized * maxDistance;
            rb.MovePosition(clampedPosition);
        }
    }
}
