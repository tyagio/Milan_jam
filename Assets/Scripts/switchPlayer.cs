using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchPlayer : MonoBehaviour
{
    enum active_state { Player1, Player2 };

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject cam;
    float timer = 0f;
    active_state player_selected=active_state.Player1;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && timer>2f)
        {
            switch (player_selected) {
                case active_state.Player1:
                    player2.GetComponent<PlayerMovement>().enabled = true;
                    player1.GetComponent<PlayerMovement>().enabled = false;
                    player_selected = active_state.Player2;
                    cam.GetComponent<CameraController>().target = player2.transform;
                    cam.GetComponent<CameraController>().flip180 = true;
                    break;

                case active_state.Player2:
                    player1.GetComponent<PlayerMovement>().enabled = true;
                    player2.GetComponent<PlayerMovement>().enabled = false;
                    player_selected = active_state.Player1;
                    cam.GetComponent<CameraController>().target = player1.transform;
                    cam.GetComponent<CameraController>().flip180 = true;
                    break;
            }
            timer = 0f;
        }
        timer += Time.deltaTime;
    }
}
