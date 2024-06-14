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
        // ȭ���� �ܷ��� �������� ������ �����ϰ� �մϴ�.
        
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

    // ������ �۾��� �����ϴ� �޼���
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
        // �Ǵ� �ٸ� �۾��� �����մϴ�.
        // ��: ������ ������Ű��
        // ScoreManager.Instance.IncreaseScore(10);
    }
}
