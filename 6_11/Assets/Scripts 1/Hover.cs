using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public GameObject hand;
    public GameObject arrow;
    public Quiver quiver;
    private Hand handScript;
    [SerializeField] GameObject[] arrowList;
    [SerializeField] public int arrowIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        // 화살의 잔량이 남아있을 때에만 장전하게 합니다.
        
        if (quiver.arrowList[arrowIndex] > 0 && other.gameObject == hand)
        {
            handScript = other.GetComponent<Hand>();
            if (handScript.setArrow)
            {
                PerformAction();
                handScript.setArrow = false;
                handScript.ArrowOff(arrowIndex);
            }
        }
    }

    // 수행할 작업을 정의하는 메서드
    private void PerformAction()
    {
        arrow.SetActive(true);
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (i == arrowIndex)
                arrowList[arrowIndex].SetActive(true);
            else
                arrowList[i].SetActive(false);
        }
        // 또는 다른 작업을 수행합니다.
        // 예: 점수를 증가시키기
        // ScoreManager.Instance.IncreaseScore(10);
    }
}
