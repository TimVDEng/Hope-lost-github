using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement physics 
    public float jumpforce;
    public float movespeed;
    Rigidbody2D rbody;
    bool grounded;
    private float desiredMovespeed;
   
    //crouch physics
    public float crouchHeight;
    private float resetHeight;

    //ground check physics
    public float groundCheckLength;

    //dash physics
    private bool dash;
    private bool hasDashed;
    public float dashSpeed;
    private float currentDashSpeed;
    public float dashtimer;
    private float resetdashtimer;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        resetHeight = transform.localScale.y;
        resetdashtimer = dashtimer;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        Crouch();
        HandleDash();
        grounded = CheckIfOnGround();
    }

    //TODO
    //Make it so that dash is a burst
    //make a dash cooldown
    void HandleDash() 
    {
        if (desiredMovespeed > 0 && Input.GetKey(KeyCode.LeftShift) && !dash && !hasDashed)
        {
            currentDashSpeed = dashSpeed;
            dash = true;
            hasDashed = true;
        }
        if (desiredMovespeed < 0 && Input.GetKey(KeyCode.LeftShift) && !dash && !hasDashed)
        {
            currentDashSpeed = -dashSpeed;
            dash = true;
            hasDashed = true;
        }

        if (dash)
        {
            dashtimer -= Time.deltaTime;   

            if (dashtimer < 0)
            {
                dash = false;
                dashtimer = resetdashtimer;
            }
        }
        else
        {
            hasDashed = !CheckIfOnGround();
        }
    }


    void HandleMovement()
    {
        //movement code ---------------------------------------------------------
        desiredMovespeed = Input.GetAxis("Horizontal") * movespeed;
        rbody.velocity = new Vector2(dash ? currentDashSpeed : desiredMovespeed, dash ? 0 : rbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            rbody.AddForce(new Vector2(0, jumpforce));
            grounded = false;
        }

    }

    void Crouch()
    {
        //crouch code ------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.localScale = new Vector2(transform.localScale.x, crouchHeight);
            if (grounded == true)
            {
                Vector2 desiredPos = new Vector2(transform.position.x, transform.position.y - crouchHeight);
                transform.position = desiredPos;
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            transform.localScale = new Vector2(transform.localScale.x, resetHeight);
        }

    }
    // ground check code
    bool CheckIfOnGround()
    {
        // creates player rays
        RaycastHit2D middleRay = Physics2D.Raycast(transform.position - (transform.up * transform.localScale.y / 2), -transform.up, groundCheckLength);
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position - (transform.up * transform.localScale.y / 2) + (transform.right * transform.localScale.x / 2), -transform.up, groundCheckLength);
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position - (transform.up * transform.localScale.y / 2) - (transform.right * transform.localScale.x / 2), -transform.up, groundCheckLength);

        if (middleRay || rightRay || leftRay)
        {
            return true;
        }

        return false;
        
    }

}
