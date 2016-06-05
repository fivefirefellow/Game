using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

	public void OnStartBtnClick(){
		SceneManager.LoadScene ("Level");
	}
		

	public void OnOptionBtnClick(){
		SceneManager.LoadScene("Option");
	}

	public void OnBackBtnClick(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void OnQuitBtnClick(){
		Application.Quit();
	}

    public void OnLevelChooseBtnClick(GameObject obj)
    {

        SceneManager.LoadScene(obj.name);
    }
}
