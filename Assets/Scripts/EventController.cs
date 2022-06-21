using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EventController : MonoBehaviour
{
    private bool level = false;
    public void Play(){
        SceneManager.LoadScene("GameplaySelect");
    }
    public void Tutorial(){
        SceneManager.LoadScene("SampleScene");
    }
    public void Story(){
        SceneManager.LoadScene("StoryLevelSelect");
    }
    public void Level_1(){
        SceneManager.LoadScene("Level_1");
    }
    public void Level_2(){
        SceneManager.LoadScene("Level_2");
    }
    public void Back(){
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void Reload(){
        Scene activescene= SceneManager.GetActiveScene();
        SceneManager.LoadScene(activescene.name);
    }
    public void Quit(){
        Application.Quit();
    }

    [SerializeField] List<GameObject> FinishDoors;

    private void Start()
    {
        if( FinishDoors.Count==2 && FinishDoors[0]!=null && FinishDoors[1] != null)
        {
            level = true;
        } 
    }

    public void Update(){
        if (level)
        {
            bool P1_door = FinishDoors[0].GetComponent<DoorTrigger>().complete;
            bool P2_door = FinishDoors[1].GetComponent<DoorTrigger>().complete;
            Scene ActiveScene = SceneManager.GetActiveScene();
            if (P1_door & P2_door)
            {
                if (ActiveScene.name == "SampleScene")
                {
                    SceneManager.LoadScene("StoryLevelSelect");
                }
                if (ActiveScene.name == "Level_1")
                {
                    SceneManager.LoadScene("EndCredits");
                }
            }
        }
    }
}
