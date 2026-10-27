using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Tunable Parameters
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float fireballSpeed = 8f;
    public float fireballVerticalSpeed = -2f;
    private bool isGrounded;
    #endregion
    #region Component References
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Animator anim;
    private GroundCheck gndChk; //groundcheck function
    public GameObject fireballPrefab;
    public Transform firePoint;
    public bool hasFireballPowerUp = false; //fireball powerup check
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        gndChk = GetComponent<GroundCheck>();

        gndChk.Init(bc, rb);

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
            isGrounded = gndChk.CheckGround();

        float moveX = Input.GetAxisRaw("Horizontal");
        bool jumpPresssed = Input.GetButtonDown("Jump");
        bool attackPressed = Input.GetKeyDown(KeyCode.LeftShift);
        bool upPressed = Input.GetKey(KeyCode.W);

        rb.linearVelocityX = moveX * moveSpeed;

        if (jumpPresssed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            if (upPressed)
            {
                anim.SetTrigger("JumpAttack");
            }
        }
        if (attackPressed && hasFireballPowerUp)
        {
            Debug.Log("Attack button pressed!");
            anim.SetTrigger("Attack");
            ShootFireball();
        }
        if (moveX != 0f) SpriteFlip(moveX);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveX", Mathf.Abs(moveX));
    }
    //fireball attack function
    void SpriteFlip(float moveX)
    {
        sr.flipX = moveX < 0f;
    }
    void ShootFireball()
    {
        // Determine which direction Mario is facing
        float direction = sr.flipX ? -1f : 1f;

        // Determine where the fireball should spawn
        Vector3 spawnPosition = firePoint.position;
        spawnPosition.x = transform.position.x + Mathf.Abs(firePoint.localPosition.x) * direction;

        // Create the fireball
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

        // Prevent the fireball from colliding with Mario
        Collider2D fireballCollider = fireball.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bc, fireballCollider);

        // Set the fireball's velocity
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        fireballRb.linearVelocity = new Vector2(direction * fireballSpeed, fireballVerticalSpeed);
    }
    //collide with powerup
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireballPowerUp"))
        {
            hasFireballPowerUp = true;
            Destroy(collision.gameObject);
        }
    }
}