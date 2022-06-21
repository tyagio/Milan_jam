using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Intro_menu : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> arr;
    [SerializeField] GameObject button;
    int curr_index = 0;

    private void Start(){
        Time.timeScale = 0f;
    }
    public void Next_text()
    {
        if (curr_index < arr.Count - 1)
        {
            arr[curr_index].enabled = false;
            arr[++curr_index].enabled = true;
            FindObjectOfType<AudioManager>().Play("Click");
        }
        else {
            arr[curr_index].enabled = false;
            button.SetActive(false);
            Time.timeScale = 1f;
            FindObjectOfType<AudioManager>().Play("Click");
        }
    }
}
