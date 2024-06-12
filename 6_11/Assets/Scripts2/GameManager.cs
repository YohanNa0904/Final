using UnityEngine;
using UnityEngine.UI;

namespace MyGameNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int score = 0;
        public int gold = 0;

        public Text scoreText;
        public Text goldText;
        public Slider healthBar;
        public Image healthFill;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateScoreText();
            UpdateGoldText();
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
                SetHealthBarColor();
            }
        }

        public void AddScore(int amount)
        {
            score += amount;
            UpdateScoreText();
            Debug.Log("Score: " + score);
        }

        public void AddGold(int amount)
        {
            gold += amount;
            UpdateGoldText();
            Debug.Log("Gold: " + gold);
        }

        public void UpdateHealthBar(int hp)
        {
            if (healthBar != null)
            {
                healthBar.value = hp;
            }
        }

        public void ShowHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(true);
            }
        }

        public void HideHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        private void SetHealthBarColor()
        {
            if (healthFill != null)
            {
                healthFill.color = Color.green;
            }
            if (healthBar.fillRect != null && healthBar.fillRect.parent != null)
            {
                Image background = healthBar.fillRect.parent.GetComponent<Image>();
                if (background != null)
                {
                    background.color = Color.red;
                }
            }
        }

        private void UpdateScoreText()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
        }

        private void UpdateGoldText()
        {
            if (goldText != null)
            {
                goldText.text = "Gold: " + gold.ToString();
            }
        }
    }
}
