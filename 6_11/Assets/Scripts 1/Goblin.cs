using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Goblin : Monster
{
    public void SetStats()
    {
        this.HP = 100.0f; // ü��

        this.attackDamage = 10.0f; // ���� ������
        this.attackDist = 2.0f; // ���� ����
        this.attackRate = 1.0f; // ���� �ӵ�
        this.nextAttackTime = 0f;

        this.MoveSpeed = 5.0f; // �̵��ӵ�
        this.traceDist = 15.0f; // ���� ����

        this.scoreValue = 10; // ���Ͱ� �׾��� �� ��� ����
        this.goldValue = 5;   // ���Ͱ� �׾��� �� ��� ���
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetStats();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
