using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenScript : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject characterScreen;
    public GameObject settingsScreen;
    public GameObject resetSuccessful;

    void Start()
    {
        mainScreen.SetActive(true);
        characterScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }

    public void GoToMainScreen() {
        mainScreen.SetActive(true);
        characterScreen.SetActive(false);
        settingsScreen.SetActive(false);
        resetSuccessful.SetActive(false);
    }

    public void GoToCharacter() {
        mainScreen.SetActive(false);
        characterScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void GoToSettings()
    {
        mainScreen.SetActive(false);
        characterScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ExitGame() {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScreen");
    }

    public void ResetGame()
    {
        //Delete the saved data file
        if (SaveScript.LoadData() != null) {
            string path = Application.persistentDataPath + "/savedData.dat";
            File.Delete(path);
        }
        resetSuccessful.SetActive(true);
    }
}
