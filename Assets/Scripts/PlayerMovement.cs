using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    enum up_or_down {down=1,up=-1};
    [SerializeField] Transform ground_check_collider;
    [SerializeField] LayerMask ground_layer;
    [SerializeField] Transform wall_check_collider;
    [SerializeField] LayerMask wall_layer;
    

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpforce = 100f;
    [SerializeField] float wallslide_speed = 0.2f;
    [SerializeField] float groundcheckradius = 0.2f;
    [SerializeField] float wallcheckradius = 0.2f;
    [SerializeField] int total_jumps = 2;
    [SerializeField] up_or_down upside_down = up_or_down.down;

    private enum MovementState {idle, running, jumping, falling, wallJumping}

    int availablejumps;
    float horizontal_input;

    bool multi_jump = false;
    bool jump_input = false;
    bool facingright = true;
    bool is_grounded = false;
    bool is_sliding = false;
    bool coyote_jump = false;
    MovementState PresentState = MovementState.idle;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (upside_down==up_or_down.up)
        {
            transform.Rotate(Vector3.right, 180f);
        }
    }

    // Update is called once per frame

    private void Update()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump_input = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump_input = false;
        }
        UpdateAnimationUpdate();
    }
    private void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
        Move(horizontal_input);
    }

    void Move(float dir)
    {
        Jump();

        #region Move
        float xval = dir * speed*(int)upside_down;
        Vector2 new_velocity = new Vector2(xval, rb.velocity.y);
        rb.velocity = new_velocity;
        if (upside_down == up_or_down.down)
        {
            if ((facingright && dir < 0f) || (!facingright && dir > 0f))
            {
                transform.Rotate(Vector3.up, 180f);
                facingright = !facingright;
            }
        }
        else {
            if ((!facingright && dir < 0f) || (facingright && dir > 0f)) {
                transform.Rotate(Vector3.up, 180f);
                facingright = !facingright;
            }
        }
        
        #endregion
    }

    private void UpdateAnimationUpdate()
    {
        MovementState State;
        MovementState TempState = PresentState;

        if (is_grounded)// on the ground
        {
            if (horizontal_input == 0)
            {
                State = MovementState.idle;
            }
            else
            {
                State = MovementState.running;
            }
        }
        else if (is_sliding)// on the wall
        {
            State = MovementState.wallJumping;
            // add wall sliding animation
        }
        else //airborne with no wall/ground support
        {
            if ((rb.velocity.y > 0f && upside_down==up_or_down.down)||(rb.velocity.y < 0f && upside_down == up_or_down.up))
            {

                if (availablejumps < total_jumps - 1) {
                    State = MovementState.jumping;
                    //add double jumping state
                }
                else {
                    State = MovementState.jumping;
                }
            }
            else
            { 
                State = MovementState.falling;
            }
        }
        if(TempState != State ) {
            anim.SetInteger("State", (int)State);
            PresentState = State;
        }
        
    }

    void GroundCheck()
    {
        bool was_grounded = is_grounded;
        is_grounded = false;
        is_grounded = Physics2D.OverlapCircle(ground_check_collider.position, groundcheckradius, ground_layer);
        if (is_grounded)
        {
            if (!was_grounded)
            {
                availablejumps = total_jumps;
                multi_jump = false;
            }
        }
        else if (was_grounded)
        {
            StartCoroutine(Coyote_Jump_Timer());
        }
    }

    IEnumerator Coyote_Jump_Timer()
    {
        coyote_jump = true;
        yield return new WaitForSeconds(0.2f);
        coyote_jump = false;
    }

    void Jump()
    {
        if (is_grounded && jump_input)
        {
            jump_input = false;
            multi_jump = true;
            availablejumps--;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce* (int)upside_down);
            FindObjectOfType<AudioManager>().Play("Jump");
        }
        else if (jump_input)
        {
            if (coyote_jump && availablejumps>0)
            {
                multi_jump = true;
                availablejumps--;
                jump_input = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpforce* (int)upside_down);
                FindObjectOfType<AudioManager>().Play("Jump");
            }
            else if (multi_jump == true && availablejumps > 0)
            {
                jump_input = false;
                availablejumps--;
                rb.velocity = new Vector2(rb.velocity.x, jumpforce* (int)upside_down);
                FindObjectOfType<AudioManager>().Play("Jump");
            }
            else if (is_sliding && availablejumps > 0)
            {
                jump_input = false;
                availablejumps--;
                rb.velocity = new Vector2(rb.velocity.x, jumpforce* (int)upside_down);
                FindObjectOfType<AudioManager>().Play("Jump");
            }
        }
    }

    void WallCheck()
    {
        if (Physics2D.OverlapCircle(wall_check_collider.position, wallcheckradius, wall_layer) && !is_grounded)
        {
            if (!is_sliding)
            {
                multi_jump = false;
                availablejumps = total_jumps;
            }
            is_sliding = true;
            bool tmp_falling = (upside_down == up_or_down.down && rb.velocity.y < 0)||
                (upside_down == up_or_down.up && rb.velocity.y > 0);
            if (tmp_falling && Mathf.Abs(horizontal_input) > 0)
            {
                Vector2 down_speed = rb.velocity;
                down_speed.y = -wallslide_speed* (int)upside_down;
                rb.velocity = down_speed;
            }
        }
        else
        {
            is_sliding = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ground_check_collider.position, groundcheckradius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(wall_check_collider.position, wallcheckradius);
    }
}

