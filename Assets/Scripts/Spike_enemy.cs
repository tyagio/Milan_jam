using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_enemy : MonoBehaviour
{
    [SerializeField] Transform pos1, pos2;
    [SerializeField] float speed=2.5f;
    [SerializeField] AnimationCurve curve;
    float lerp_t = 0f;
    int dir = 1;

    // Update is called once per frame
    void Update()
    {
        lerp_t += Time.deltaTime * speed * dir;
        if (lerp_t > 1f) {
            dir = -1;
        }
        else if(lerp_t < 0f)
        {
            dir = 1;
        }
        transform.position = Vector3.Lerp(pos1.position, pos2.position, curve.Evaluate(lerp_t));
    }
}
