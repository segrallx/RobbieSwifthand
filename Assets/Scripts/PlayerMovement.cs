using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移动参数")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;


    [Header("envrionments")]
    public LayerMask groundLayer;

    public float footOffset = 0.4f;
    public float headClearance = 0.4f;
    public float groundDistance = 0.4f;


    float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;


    [Header("Jump Parameters")]
    private float hangingJumpForce = 15f;
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    float jumpTime;


    //input setting
    bool jumpPressed;
    bool jumpHeld;
    bool crouchHeld;
	bool crouchPress;

    float xVelocity;

    // colloider size
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        coll = this.GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;

        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpPressed = false;
        }

        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
		crouchPress = Input.GetButtonDown("Crouch");
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck || rightCheck)
            isOnGround = true;
        else
            isOnGround = false;

        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        if (headCheck)
            isHeadBlocked = true;
        else
            isHeadBlocked = false;

        // 小于0是人物朝左，大于0是人物朝右。
        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0);
        //Debug.LogFormat("direction {0} playerHeight {1} grabDir {2} grabDistance {3}", transform.localScale, playerHeight, grabDir, grabDistance);

        RaycastHit2D blockCheck = Raycast(new Vector2(footOffset * direction, playerHeight),
                                          grabDir, grabDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight),
                                         grabDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight),
                                          Vector2.down, grabDistance, groundLayer);

        Debug.LogFormat("hanging check velocity.y {0}", rb.velocity.y);
        if (!isOnGround && rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockCheck)
        {
            this.rb.bodyType = RigidbodyType2D.Static;
            this.isHanging = true;
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            Debug.LogFormat("hanging enter velocity.y {0}", rb.velocity.y);
        }

    }


    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection.normalized * length, color, 0);
        return hit;
    }

    void GroundMovement()
    {
        if (isHanging)
        {
            return;
        }

        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
        {
            StandUp();
        }
        else if (!isOnGround && isCrouch)
        {
            StandUp();
        }

        xVelocity = Input.GetAxis("Horizontal");// [-1 1]

        if (isCrouch)
        {
            xVelocity /= crouchSpeedDivisor;
        }

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        FlipDirection();
    }

    void FlipDirection()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (xVelocity > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
                Debug.LogFormat("hanging exit velocity.y {0}", rb.velocity.y);
            }

			if(crouchPress)  {
				rb.bodyType = RigidbodyType2D.Dynamic;
				isHanging = false;
			}

        }

        if (jumpPressed && isOnGround && !isJump)
        {
            if (isCrouch && isCrouch && !isHeadBlocked)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            isJump = true;
            isOnGround = false;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            jumpTime = Time.time + jumpHoldDuration;
        }
        else if (isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0, jumpHoldForce), ForceMode2D.Impulse);
            }

            if (jumpTime < Time.time)
            {
                isJump = false;
            }
        }
    }
}
