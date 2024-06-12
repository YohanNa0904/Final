using UnityEngine;

public class FollowLine : MonoBehaviour
{
    public Transform target; // 따라갈 대상 오브젝트

    private LineRenderer lineRenderer; // 라인 렌더러 컴포넌트

    void Start()
    {
        // 라인 렌더러 동적으로 생성
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // 라인 색상 설정 (검정색)
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        // 라인 두께 설정 (0.01)
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        // 라인 렌더러의 소재 설정 (기본 소재 사용)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // 처음 시작할 때 라인 비활성화
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // LineRenderer와 target이 null이 아닌지 확인
        if (lineRenderer != null && target != null)
        {
            // 라인의 시작점 설정 (해당 객체의 위치)
            lineRenderer.SetPosition(0, transform.position);
            // 라인의 끝점 설정 (타겟 객체의 위치)
            lineRenderer.SetPosition(1, target.position);

            // 처음 시작할 때 라인 활성화
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
        }
        else
        {
            // 타겟이 설정되지 않은 경우 라인 비활성화
            if (lineRenderer != null && lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }
}
