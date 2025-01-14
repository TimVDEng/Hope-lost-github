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
    public float dashSpeed;
    public float desiredDashSpeed;
    public float dashtimer;
    private float resetdashtimer;
    public bool canDash = false;
    public bool isDashing;

    //damage physics
    public int damage;

    public CheckForDamage leftBox;
    public CheckForDamage rightBox;

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
        HandleAttack();
        grounded = CheckIfOnGround();
    }

    void HandleAttack()
    {
        if(Input.GetAxis("Horizontal") > 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (rightBox.canDamage)
            {
                rightBox.health.healthPoints -= damage;
            }
        }

        if (Input.GetAxis("Horizontal") < 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (leftBox.canDamage)
            {
                leftBox.health.healthPoints -= damage;
            }
        }
    }

    //This void handles the dash ability
    void HandleDash() 
    {
        //Checks whether we want to go right or left
        if (Input.GetAxis("Horizontal") > 0 && canDash && Input.GetKey(KeyCode.LeftShift))
        {
            isDashing = true;
            desiredDashSpeed = dashSpeed;
        }
        if (Input.GetAxis("Horizontal") < 0 && canDash && Input.GetKey(KeyCode.LeftShift))
        {
            isDashing = true;
            desiredDashSpeed = -dashSpeed;
        }

        //if we dash we subtract a number from the timer each frame.
        if (isDashing)
        {
            canDash = false;
            dashtimer -= Time.deltaTime;
            if(dashtimer < 0)
            {
                isDashing = false;
                dashtimer = resetdashtimer;
            }
        }
    }


    void HandleMovement()
    {
        //movement code ---------------------------------------------------------
        desiredMovespeed = Input.GetAxis("Horizontal") * movespeed;
        rbody.velocity = new Vector2(isDashing ? desiredDashSpeed : desiredMovespeed, isDashing ? 0 : rbody.velocity.y);

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            canDash = true;
        }
    }

}
