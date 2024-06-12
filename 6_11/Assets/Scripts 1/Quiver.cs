using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiver : MonoBehaviour
{
    public int[] arrowList = new int[5];
    public int arrowIndex = 0;

    public GameObject hand;

    public Hover hover;
    public ShootRay shootRay;

    public Text[] arrowTexts;

    private Color[] arrowColors = {
        Color.white,
        Color.yellow,
        Color.blue,
        Color.red,
        Color.black
    };

    void Start()
    {
        arrowList[0] = 10;
        arrowList[1] = 10;
        arrowList[2] = 10;
        arrowList[3] = 10;
        arrowList[4] = 20;

        InitializeArrowTexts();
    }

    void InitializeArrowTexts()
    {
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (arrowTexts[i] != null)
            {
                arrowTexts[i].text = arrowList[i].ToString();
                arrowTexts[i].color = arrowColors[i];
                arrowTexts[i].enabled = false;
            }
        }
    }

    public void UpdateArrowText(int index)
    {
        if (arrowTexts[index] != null)
        {
            arrowTexts[index].text = arrowList[index].ToString();
            arrowTexts[index].color = arrowColors[index];
            StartCoroutine(ShowArrowTextTemporarily(arrowTexts[index]));
        }
    }

    IEnumerator ShowArrowTextTemporarily(Text arrowText)
    {
        arrowText.enabled = true;
        yield return new WaitForSeconds(1f);
        arrowText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == hand)
        {
            Hand handScript = hand.GetComponent<Hand>();
            if (handScript != null && arrowList[handScript.arrowIndex] > 0)
            {
                handScript.SetArrow();
                arrowIndex = handScript.arrowIndex;
                hover.arrowIndex = arrowIndex;
                shootRay.skillIndex = arrowIndex;
                UpdateArrowText(arrowIndex);
            }
            else
            {
                Debug.Log("Invalid arrow index or no arrows left at index: " + handScript.arrowIndex);
            }
        }
    }
}
