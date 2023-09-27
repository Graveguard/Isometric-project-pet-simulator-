using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DogController m_DogController;
    public PlayerController m_Player;
    public HappinessController m_Happy;
    public FoodManager m_Food;
    public bool FoodFull;
    public GameObject overPanel,pausePanel;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance==null)
            Instance = this;
    }
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void Pause()
    {
        Time.timeScale = .001f;
        pausePanel.SetActive(true);

    }
}
