using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // ParticleSystem ������Ʈ�� ������
        particleSystem = GetComponent<ParticleSystem>();

        // ParticleSystem�� ������ ��� �޽����� ���
        if (particleSystem == null)
        {
            Debug.LogWarning("ParticleSystem component not found!");
        }
    }

    void Update()
    {
        // ParticleSystem�� ��� ������ Ȯ��
        if (particleSystem != null && !particleSystem.IsAlive())
        {
            // ParticleSystem�� �� �̻� ��� ���� �ƴϸ� ������Ʈ�� ����
            Destroy(gameObject);
        }
    }
}
