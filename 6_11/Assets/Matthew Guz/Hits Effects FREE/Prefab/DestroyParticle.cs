using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // ParticleSystem 컴포넌트를 가져옴
        particleSystem = GetComponent<ParticleSystem>();

        // ParticleSystem이 없으면 경고 메시지를 출력
        if (particleSystem == null)
        {
            Debug.LogWarning("ParticleSystem component not found!");
        }
    }

    void Update()
    {
        // ParticleSystem이 재생 중인지 확인
        if (particleSystem != null && !particleSystem.IsAlive())
        {
            // ParticleSystem이 더 이상 재생 중이 아니면 오브젝트를 삭제
            Destroy(gameObject);
        }
    }
}
