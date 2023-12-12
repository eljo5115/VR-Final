using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //scenes loaded in order, load +1 on complete
    static GameManager instance;
    public static int collectibleCount;
    List<InputDevice> inputDevices = new List<InputDevice>();
    [SerializeField] private KeyCode escapeButton = KeyCode.Joystick1Button0;
    void Start()
    {
        InitializeInputReader();
    }
    void InitializeInputReader()
    {
        InputDevices.GetDevices(inputDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, inputDevices);

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
        if(Input.GetKeyDown(escapeButton))
        {
            LoadMainMenu();
        }
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
            GameObject player = GameObject.Find("VR Wheelchair");
            if(player.transform.position.y < -10 || player.transform.position.y > 100)
            {
                LoadMainMenu();
            }
            GameObject collectibles = GameObject.Find("Collectibles");
            if(collectibles.transform.childCount < 1)
            {
                Invoke(nameof(LoadMainMenu),3);
            }
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
