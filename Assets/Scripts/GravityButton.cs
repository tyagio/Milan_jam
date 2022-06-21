using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityButton : MonoBehaviour
{            
    private int Gtriggercount;
    private bool triggerstate= false;
    [SerializeField]
    public List<GameObject> Gtriggers;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")){
            triggerstate =true;
            Changestate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")){
            triggerstate =false;
            Changestate();
        }
    }

    private void Changestate(){
    Gtriggercount = Gtriggers.Count;
        for(int i=0 ; i<Gtriggercount;i++){
                Gtriggers[i].SetActive(triggerstate);
            }
    }
}
