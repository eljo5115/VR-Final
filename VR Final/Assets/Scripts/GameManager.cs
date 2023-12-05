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
    void StateManager()
    {
        Scene s = SceneManager.GetActiveScene();
        if(s.name == "GorillaTag")
        {
            //do gorilla tag stuff
        }
        if(s.name == "SpoderMan")
        {
            //timing for spoderman time trial
        }
        if (s.name == "WheelchairHorror")
        {
            //terrify wheelchair users
        }
        if(s.name == "Climbing")
        {
            //time trial? idk climbing stuff
        }
    }
}
