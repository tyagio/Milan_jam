using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCreditsScroll : MonoBehaviour
{
    private Vector2 pos;
    [SerializeField]
    public float ScrollSpeed=1f;
    [SerializeField]
    public float UpperBound=1300f+540f;
    
    [SerializeField]
    public float LowerBound=-780f+540f;
    private void Start() {
        this.transform.position = new Vector2(960f, LowerBound);
    }
    void Update()
    {
        pos = this.GetComponent<Transform>().position ;
        if(pos.y <= UpperBound){
            pos += new Vector2(0f,ScrollSpeed);
        }
        this.transform.position = pos;
    }
}
