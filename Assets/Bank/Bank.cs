using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{    
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] int startingBalance = 300;
    [SerializeField] int currentBalance;
    [SerializeField] TextMeshProUGUI displayBalance;
    [SerializeField] private GameObject gameOverMenu;

    void Awake() {
        currentBalance = startingBalance;
        gameOverMenu.gameObject.SetActive(false);

        UpdateDisplay();
    }

    public void Deposit(int amount) {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount) {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();

        if(currentBalance < 0) {
            gameOverMenu.gameObject.SetActive(true);
        }
    }

    void UpdateDisplay() {
        displayBalance.text = "Gold: " + currentBalance;

    }

    void ReloadScene() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

}
