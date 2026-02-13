using System.Collections;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int canJumpForTimes = 2;
    public int waitForRespawn = 1;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    public Image hpbar;
    
    

    private Animator animator;

    private SpriteRenderer spriteRenderer;
    private int canJumpFor;
    private bool isGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        


        canJumpFor = canJumpForTimes;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2 (moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && canJumpFor > 0)
        {
            canJumpFor--;
            rb.linearVelocityY = jumpForce;
            isGrounded = false; 
        }

        SetAnimation(moveInput);
    }



    private void SetAnimation(float moveInput) 
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("Player_Idle");
            }
            else
            {
                animator.Play("Player_Run");
            }
        }
        else 
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else 
            {
                animator.Play("Player_Fall");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canJumpFor = canJumpForTimes;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage") 
        {
            health -= 25;
            rb.linearVelocityY = jumpForce;
            StartCoroutine(BlinkRed());

            if (health <= 0) 
            {

                cc.enabled = false;
                StartCoroutine(resetGameInSeconds());

            }
            hpbar.fillAmount = health / 100f;
        }

    }
    private IEnumerator BlinkRed() 
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;

    }
    private IEnumerator resetGameInSeconds() 
    {
        yield return new WaitForSeconds(waitForRespawn);
        SceneManager.LoadScene("Main");
    }
    
}
