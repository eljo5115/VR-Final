using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerProxy : MonoBehaviour
{
    private GameObject gameManager;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
    }
    public void MonkeyClick(){gm.LoadMonkeyTagScene();}
    public void SpiderClick(){gm.LoadSpiderManScene();}
    public void WheelchairClick(){gm.LoadWheelchairScene();}
    public void ClimbClick(){gm.LoadClimbingScene();}
    public void ExitClick(){gm.ExitGame();}
}
