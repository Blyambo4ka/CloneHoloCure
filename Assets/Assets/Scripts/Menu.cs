using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject panelSettings;
    void Start()
    {
        if (panelSettings != null)
            panelSettings.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Settings()
    {
        if( panelSettings.activeSelf == false)
        {
            panelSettings.SetActive(true);
        }
        else if(panelSettings.activeSelf == true)
        {
            panelSettings.SetActive(false);
        }
    }
    public void Exit()
    {
        Application.Quit();
    } 
}
