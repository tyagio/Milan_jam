using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeTrigger : MonoBehaviour
{
    private Rigidbody2D box;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            box = collision.gameObject.GetComponent<Rigidbody2D>();
            box.gravityScale *= (-1);
            box.velocity = new Vector2(0f,0f);
        }
    }
}
