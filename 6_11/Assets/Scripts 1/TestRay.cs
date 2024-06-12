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
            lineRenderer.enabled = false; // ���� ���� �� ���� �������� ��Ȱ��ȭ�մϴ�.
        }
    }

    void StartShooting()
    {
        isShooting = true;
        lineRenderer.enabled = true; // ��� �����ϸ� ���� �������� Ȱ��ȭ�մϴ�.
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
