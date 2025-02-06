using UnityEngine;
using TMPro; // Для работы с TextMeshPro

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Текст таймера (TextMeshPro)
    private float startTime;
    private float currentTime;
    private bool gameIsRunning = true;
    public static GameManager Instance { get; private set; }

     void Awake()
    {
        //Проверка singleton
        if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                 return;
             }
        Instance = this;
    }

    void Start()
    {
        startTime = Time.time; // Запоминаем время начала
        UpdateUI();
    }

    void Update()
    {
        if (!gameIsRunning) return;

       currentTime = Time.time - startTime; // Вычисляем прошедшее время

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}