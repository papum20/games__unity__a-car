using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    void Start()
    {
        gameObject.SetActive(false);
    }



    public void go_to_mainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void return_to_game()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

}
