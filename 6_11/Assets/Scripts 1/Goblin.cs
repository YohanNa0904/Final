using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Goblin : Monster
{
    void Start()
    {
        // Goblin ������ �ʱ�ȭ �ڵ� �߰�
        base.Start(); // Monster Ŭ������ Start �޼��� ȣ��
    }

    void Update()
    {
        base.Update(); // Monster Ŭ������ Update �޼��� ȣ��
        // Goblin ������ ������Ʈ ���� �߰�
    }
}

