using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject highscoresMenu;
    [SerializeField]
    private GameObject NameMenu;

    [SerializeField]
    private InputField NameInput;
    

    void Awake()
    {
        string name = PlayerPrefs.GetString("Name");
        if(name != null)
        NameInput.text = name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Options()
    {
        if (optionsMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }
    }

    public void Highscores()
    {
        if (highscoresMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            highscoresMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            highscoresMenu.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ButtonPlay()
    {
        if (NameMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            NameMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            NameMenu.SetActive(true);
        }
    }

    public void Play()
    {
        PlayerPrefs.SetString("Name", NameInput.text);
        SceneManager.LoadScene(1);
    }
    
}
