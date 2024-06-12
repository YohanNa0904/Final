using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public bool setArrow = false;
    public int arrowIndex = 4;
    public GameObject[] arrowList;

    private InputAction aButtonAction;
    private InputAction bButtonAction;

    void Awake()
    {
        aButtonAction = new InputAction("AButton", binding: "<XRController>{RightHand}/primaryButton");
        bButtonAction = new InputAction("BButton", binding: "<XRController>{RightHand}/secondaryButton");

        aButtonAction.performed += ctx => DecreaseArrowIndex();
        bButtonAction.performed += ctx => IncreaseArrowIndex();

        aButtonAction.Enable();
        bButtonAction.Enable();
    }

    void OnDestroy()
    {
        aButtonAction.Disable();
        bButtonAction.Disable();
    }

    void DecreaseArrowIndex()
    {
        arrowIndex = (arrowIndex + 4) % 5;
        Debug.Log("Arrow Index Decreased: " + arrowIndex);
    }

    void IncreaseArrowIndex()
    {
        arrowIndex = (arrowIndex + 1) % 5;
        Debug.Log("Arrow Index Increased: " + arrowIndex);
    }

    public void SetArrow()
    {
        setArrow = true;
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (i == arrowIndex)
            {
                arrowList[i].SetActive(true);
            }
            else
            {
                arrowList[i].SetActive(false);
            }
        }
        Quiver quiver = FindObjectOfType<Quiver>();
        if (quiver != null)
        {
            quiver.UpdateArrowText(arrowIndex);
        }
    }
}
