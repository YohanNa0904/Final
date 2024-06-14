using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Goblin : Monster
{
    public void SetStats()
    {
        this.HP = 100.0f; // 체력

        this.attackDamage = 10.0f; // 공격 데미지
        this.attackDist = 2.0f; // 공격 범위
        this.attackRate = 1.0f; // 공격 속도
        this.nextAttackTime = 0f;

        this.MoveSpeed = 5.0f; // 이동속도
        this.traceDist = 15.0f; // 추적 범위

        this.scoreValue = 10; // 몬스터가 죽었을 때 얻는 점수
        this.goldValue = 5;   // 몬스터가 죽었을 때 얻는 골드
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
