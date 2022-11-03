using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagerUI : MonoBehaviour
{
    
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitButton;

    private void Start() 
    {
        playAgainButton.onClick.AddListener( () => { SceneManager.LoadScene(0); } );
        exitButton.onClick.AddListener( () => { Application.Quit(); } );
    }


}
