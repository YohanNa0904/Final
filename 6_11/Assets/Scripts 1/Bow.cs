using UnityEngine;
using UnityEngine.XR;

public class Bow : MonoBehaviour
{
    // ���� �߻� ���� ������
    [SerializeField] private GameObject rayOrigin; // ���� �߻� ���� ����
    public Quiver quiver;

    // ���� ������ �߻� ����
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform releasePoint;

    public float maxPull = 1;
    public float maxRange = 100;

    // VR ��Ʈ�ѷ� ����
    public XRNode controllerNode = XRNode.RightHand; // ����� ��Ʈ�ѷ� (������)
    private InputDevice controller;

    private bool isPulling = false; // Ȱ�� ���� ������ ���θ� ��Ÿ���� �÷���

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    // Ȱ�� ���� �� ȣ��Ǵ� �޼���
    public void Release()
    {
        if (rayOrigin != null)
        {
            quiver.arrowList[quiver.arrowIndex] -= 1;
            // attachPoint�� releasePoint ������ �Ÿ��� ���� �Ŀ��� ����մϴ�.
            float pullDistance = Vector3.Distance(attachPoint.position, releasePoint.position);

            attachPoint.position = releasePoint.position;
            attachPoint.rotation = Quaternion.LookRotation(releasePoint.forward, Vector3.up);

            // attachPoint�� Rigidbody�� �����ͼ� �ӵ��� 0���� �����ϰ� �������� ����ϴ�.
            Rigidbody attachPointRb = attachPoint.GetComponent<Rigidbody>();
            if (attachPointRb != null)
            {
                // �߰� �������� ���� ���� kinematic�� true�� �����մϴ�.
                attachPointRb.isKinematic = true;
                // �ӵ��� 0���� �����մϴ�.
                attachPointRb.velocity = Vector3.zero;
                attachPointRb.angularVelocity = Vector3.zero;
            }

            attachPoint.gameObject.SetActive(false);

            // Ȱ�� ���� ���� ���� ���� ����
            float releaseVibrationStrength = 1.0f; // Ȱ�� ���� ���� �ִ� ������ ����
            TriggerHaptic(releaseVibrationStrength, 0.2f); // ���� �ð��� 0.2�ʷ� ����

            // ���� �߻�
            rayOrigin.GetComponent<ShootRay>().Shoot(pullDistance * 20);

            isPulling = false; // Ȱ�� �������Ƿ� ���� ����
        }
    }

    public void ReloadArrow()
    {
        attachPoint.gameObject.SetActive(true);
        isPulling = true; // Ȱ�� �ٽ� ���� ����
    }

    private void TriggerHaptic(float amplitude, float duration)
    {
        if (controller.isValid)
        {
            HapticCapabilities capabilities;
            if (controller.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
            {
                uint channel = 0;
                controller.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

    // attachPoint�� rayOrigin ������ ȸ����Ű�� �޼���
    private void OrientAttachPointTowardsRayOrigin()
    {
        if (rayOrigin != null)
        {
            Vector3 direction = rayOrigin.transform.position - attachPoint.position;
            attachPoint.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void LateUpdate()
    {
        OrientAttachPointTowardsRayOrigin();

        if (isPulling)
        {
            // attachPoint�� releasePoint ������ �Ÿ��� ���� ���� ������ �����մϴ�.
            float pullDistance = Vector3.Distance(attachPoint.position, releasePoint.position);
            float vibrationStrength = Mathf.Clamp(pullDistance / maxPull, 0, 1);

            // ������ ���������� ������Ʈ�մϴ�.
            TriggerHaptic(vibrationStrength, Time.deltaTime);
        }
    }
}
