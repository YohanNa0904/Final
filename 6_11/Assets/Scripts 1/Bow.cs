using UnityEngine;
using UnityEngine.XR;

public class Bow : MonoBehaviour
{
    // 레이 발사 관련 변수들
    [SerializeField] private GameObject rayOrigin; // 레이 발사 시작 지점
    public Quiver quiver;

    // 부착 지점과 발사 지점
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform releasePoint;

    public float maxPull = 1;
    public float maxRange = 100;

    // VR 컨트롤러 설정
    public XRNode controllerNode = XRNode.RightHand; // 사용할 컨트롤러 (오른손)
    private InputDevice controller;

    private bool isPulling = false; // 활을 당기는 중인지 여부를 나타내는 플래그

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    // 활을 놓을 때 호출되는 메서드
    public void Release()
    {
        if (rayOrigin != null)
        {
            quiver.arrowList[quiver.arrowIndex] -= 1;
            // attachPoint와 releasePoint 사이의 거리에 따라 파워를 계산합니다.
            float pullDistance = Vector3.Distance(attachPoint.position, releasePoint.position);

            attachPoint.position = releasePoint.position;
            attachPoint.rotation = Quaternion.LookRotation(releasePoint.forward, Vector3.up);

            // attachPoint의 Rigidbody를 가져와서 속도를 0으로 설정하고 움직임을 멈춥니다.
            Rigidbody attachPointRb = attachPoint.GetComponent<Rigidbody>();
            if (attachPointRb != null)
            {
                // 추가 움직임을 막기 위해 kinematic을 true로 설정합니다.
                attachPointRb.isKinematic = true;
                // 속도를 0으로 설정합니다.
                attachPointRb.velocity = Vector3.zero;
                attachPointRb.angularVelocity = Vector3.zero;
            }

            attachPoint.gameObject.SetActive(false);

            // 활을 놓을 때의 진동 강도 설정
            float releaseVibrationStrength = 1.0f; // 활을 놓을 때는 최대 강도로 진동
            TriggerHaptic(releaseVibrationStrength, 0.2f); // 진동 시간은 0.2초로 설정

            // 레이 발사
            rayOrigin.GetComponent<ShootRay>().Shoot(pullDistance * 20);

            isPulling = false; // 활을 놓았으므로 당기기 종료
        }
    }

    public void ReloadArrow()
    {
        attachPoint.gameObject.SetActive(true);
        isPulling = true; // 활을 다시 당기기 시작
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

    // attachPoint를 rayOrigin 쪽으로 회전시키는 메서드
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
            // attachPoint와 releasePoint 사이의 거리에 따라 진동 강도를 설정합니다.
            float pullDistance = Vector3.Distance(attachPoint.position, releasePoint.position);
            float vibrationStrength = Mathf.Clamp(pullDistance / maxPull, 0, 1);

            // 진동을 지속적으로 업데이트합니다.
            TriggerHaptic(vibrationStrength, Time.deltaTime);
        }
    }
}
