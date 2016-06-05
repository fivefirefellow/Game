using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public int condition;
    void Start()
    {
        condition = 2;
    }

    void WinWindow(int WindowID)
    {
        if (GUI.Button(new Rect(10, 30, 40, 50), "OK"))
        {
            SceneManager.LoadScene("Level");
        }
    }

    void OnGUI()
    {
        if(condition == 0)
        {
            GUI.Window(0, new Rect(0, 0, 500, 500), WinWindow, "You Win");
        }
    }
}
