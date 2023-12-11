using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //scenes loaded in order, load +1 on complete
    static GameManager instance;
    public static int collectibleCount;
    void Start()
    {
        collectibleCount = 0;
    }
    void Awake()
    {
        if (instance != null){
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Update()
    {
        StateManager();
    }
    void StateManager()
    {
        Scene s = SceneManager.GetActiveScene();
        if(s.name == "GorillaTag")
        {
            //do gorilla tag stuff
            GorillaTagLogic();
        }
        if(s.name == "SpoderMan")
        {
            //timing for spoderman time trial
            SpoderManLogic();
        }
        if (s.name == "WheelchairHorror")
        {
            //terrify wheelchair users
        }
        if(s.name == "Climbing")
        {
            //time trial? idk climbing stuff
            GameObject collectibles = GameObject.Find("Collectibles");
            if(collectibles.transform.childCount < 1)
            {
                Invoke(nameof(LoadMainMenu),3);
            }
        }
    }
    void GorillaTagLogic()
    {

        GameObject collectibles = GameObject.Find("Collectibles");
        if(collectibles.transform.childCount < 1)
        {
            Invoke(nameof(LoadMainMenu),3);
        }
    }
    void SpoderManLogic()
    {
        GameObject collectibles = GameObject.Find("Collectibles");
        if(collectibles.transform.childCount < 1)
        {
            Invoke(nameof(LoadMainMenu),3);
        }
    }
    public void LoadMonkeyTagScene(){SceneManager.LoadScene("GorillaTag");}
    public void LoadSpiderManScene(){SceneManager.LoadScene("SpoderMan");}
    public void LoadWheelchairScene(){SceneManager.LoadScene("WheelchairHorror");}
    public void LoadClimbingScene(){SceneManager.LoadScene("Climbing");}
    public void LoadMainMenu(){SceneManager.LoadScene("MainMenu");}
    public void ExitGame(){Application.Quit();}
}
