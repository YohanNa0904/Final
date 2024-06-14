using MyGameNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestBoard
{
    public class Status : MonoBehaviour
    {
        [SerializeField]
        int gold;
        [SerializeField]
        int hp;
        [SerializeField]
        int ad;
        [SerializeField]
        int moveSpeed;
        [SerializeField]
        float cltical;

        public int Gold
        {
            get { return gold; }
            set
            {
                gold = value;
                GameManager.Instance.AddGold(gold);
            }
        }

        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                GameManager.Instance.UpdateHealthBar(hp);
            }
        }

        public void DecreaseHP(int amount)
        {
            HP -= amount;
            StartCoroutine(ShowHealthBarTemporarily());
        }

        IEnumerator ShowHealthBarTemporarily()
        {
            GameManager.Instance.ShowHealthBar();
            yield return new WaitForSeconds(1f);
            GameManager.Instance.HideHealthBar();
        }
    }
}
