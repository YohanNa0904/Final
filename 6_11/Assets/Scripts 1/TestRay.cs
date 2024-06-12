using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private bool isShooting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartShooting();
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            StopShooting();
        }

        if (isShooting)
        {
            UpdateLineRenderer();
        }
        else
        {
            lineRenderer.enabled = false; // 쏘지 않을 때 라인 렌더러를 비활성화합니다.
        }
    }

    void StartShooting()
    {
        isShooting = true;
        lineRenderer.enabled = true; // 쏘기 시작하면 라인 렌더러를 활성화합니다.
    }

    void StopShooting()
    {
        isShooting = false;
    }

    void UpdateLineRenderer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * 100f);
        }
    }
}
