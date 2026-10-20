using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Tunable Parameters
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Vector2 CalculateGroundCheckPos()
    {
        Bounds bounds = bc.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }
    #endregion

    #region Ground Check Parameters
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;
    #endregion
    #region Component References
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Animator anim;
    public GameObject fireballPrefab;
    public Transform firePoint;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityY <= 0f)
        {
            isGrounded = Physics2D.OverlapCircle(CalculateGroundCheckPos(), groundCheckRadius, groundLayer);
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        bool jumpPresssed = Input.GetButtonDown("Jump");
        bool attackPressed = Input.GetKeyDown(KeyCode.LeftShift);

        rb.linearVelocityX = moveX * moveSpeed;

        if (jumpPresssed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        if (attackPressed)
        {
            Debug.Log("Attack button pressed!");
            anim.SetTrigger("Attack");
            ShootFireball();
        }
        if (moveX != 0f) SpriteFlip(moveX);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveX", Mathf.Abs(moveX));
    }

    void SpriteFlip(float moveX) => sr.flipX = (moveX < 0f);
    void ShootFireball()
    {
        float direction = sr.flipX ? -1f : 1f;

        Vector3 spawnPosition = firePoint.position;

        spawnPosition.x = transform.position.x +
            Mathf.Abs(firePoint.localPosition.x) * direction;

        GameObject fireball = Instantiate(
            fireballPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();

        fireballRb.linearVelocityX = direction * 8f;
    }
}