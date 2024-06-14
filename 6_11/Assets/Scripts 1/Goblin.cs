using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Goblin : Monster
{
    void Start()
    {
        // Goblin 고유의 초기화 코드 추가
        base.Start(); // Monster 클래스의 Start 메서드 호출
    }

    void Update()
    {
        base.Update(); // Monster 클래스의 Update 메서드 호출
        // Goblin 고유의 업데이트 로직 추가
    }
}

