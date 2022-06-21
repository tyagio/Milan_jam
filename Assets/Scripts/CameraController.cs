using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target,left_bottom,top_right;
    public bool flip180 = false;
    [SerializeField] Vector3 offset;
    [SerializeField] float chase_factor;
    [SerializeField] float rot_duration = 2f;

    float height,width;
    bool isrotating = false;
    Quaternion final_rot;
    Camera cam;
    
    private void Start() {
        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        final_rot = transform.rotation;
    }
    private void FixedUpdate()
    {
        if (flip180 && !isrotating) {
            StartCoroutine("FlipCamera");
            flip180 = false;
        }
        track_target_lerp();
    }

    void track_target_lerp()
    {
        Vector3 target_position = target.position + offset;
        Vector3 boundposition = new Vector3(Mathf.Clamp(target_position.x, left_bottom.position.x + width/2, top_right.position.x - width/2),
        Mathf.Clamp(target_position.y, left_bottom.position.y + height/2 ,top_right.position.y -height/2), target_position.z);
        Vector3 smooth_chase = Vector3.Lerp(transform.position, boundposition, chase_factor * Time.deltaTime);
        transform.position = smooth_chase;
    }

    IEnumerator FlipCamera()
    {
        final_rot = final_rot * Quaternion.Euler(0, 0, 180);
        Quaternion startRot = transform.rotation;
        isrotating = true;
        float rot_elapsedTime = 0.0F;
        while (rot_elapsedTime < rot_duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, final_rot, rot_elapsedTime / rot_duration);
            rot_elapsedTime += Time.deltaTime;
            yield return null;
        }
        isrotating = false;
        transform.rotation=final_rot;
    }
}
