using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Global Variables
    public float moveSpeed = 8;
    public float jumpForce = 12;
    public float jumpTimeCounter = 0.3f;
    private float jumpTime;
    public Vector2 jumpBoxSize;
    public float attackCooldown = 0.29f;
    public float spellCooldown;
    private float lastAttack;
    private float lastSpell;
    public float stasisTimer = 0.2f;
    public float timescale;
    private float startTimeScale;
    private float startFixedDeltaTime;

    public int health;
    public int numOfHearts;
    private int currentColorIndex = 0;

    private bool isGrounded;
    private bool isJumping;
    private bool isMenuActive;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameObject colorwheel;
    public GameObject[] colors = new GameObject[6];
    public GameObject projectilePrefab;

    private Rigidbody2D rb;
    public Transform feetPos;
    public Transform projSpawnPos;
    private Animator PlayerAnimator;
    public Animator MenuAnimator;

    public LayerMask groundLayer;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        health = numOfHearts;
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        ColorManager.Instance.CurrentColor = "blue";
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
        ColorWheel();
        HealthSystem();
    }

    private void Move()
    {
        while (PublicVariables.isColoring) { return; }
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new(inputHorizontal * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (inputHorizontal > 0)
        {
            // transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime, 0);
            transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            PlayerAnimator.SetBool("isRunning", true);
        }
        else if (inputHorizontal < 0)
        {
            // transform.Translate(new Vector2(-moveSpeed, 0) * Time.deltaTime, 0);
            transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            PlayerAnimator.SetBool("isRunning", true);
        }
        else
        {
            PlayerAnimator.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        while (PublicVariables.isColoring) { return; }
        isGrounded = Physics2D.OverlapBox(feetPos.position, jumpBoxSize, 0, groundLayer);
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime = jumpTimeCounter;
        }

        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            if (jumpTime > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        PlayerAnimator.SetBool("isJumping", !isGrounded);
    }

    private void Attack()
    {
        while (PublicVariables.isColoring) { return; }
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.J) && Time.time - lastAttack > attackCooldown)
        {
            PlayerAnimator.SetTrigger("attack");
            lastAttack = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.K) && inputHorizontal != 0 && Time.time - lastSpell > spellCooldown)
        {
            Instantiate(projectilePrefab, projSpawnPos.position, Quaternion.Euler(0, 0, 45));
            lastSpell = Time.time;

            Vector2 currentMovement = new(0, 0);
            rb.velocity = currentMovement;
            rb.velocity = new(0, 0);
            if (Time.time + stasisTimer >= Time.time)
            {
                rb.velocity = currentMovement;
            }
        }
    }

    private void ColorWheel()
    {
        if (Input.GetKey(KeyCode.X))
        {
            var inputRight = Input.GetKeyDown(KeyCode.RightArrow);
            var inputLeft = Input.GetKeyDown(KeyCode.LeftArrow);

            Time.timeScale = timescale;
            Time.fixedDeltaTime = startFixedDeltaTime * timescale;
            colorwheel.SetActive(true);
            PlayerAnimator.SetBool("isRunning", false);
            PublicVariables.isColoring = true;
            for (int i = 0; i < colors.Length; i++)
            {
                if (i == currentColorIndex)
                {
                    colors[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }

            int inputDir = 0;
            if (inputRight || inputLeft)
            {
                if (inputRight)
                {
                    inputDir = 1;
                }
                else if (inputLeft)
                {
                    inputDir = -1;
                }
                ChangeColor(inputDir);
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            Time.timeScale = startTimeScale;
            Time.fixedDeltaTime = startFixedDeltaTime;
            colorwheel.SetActive(false);
            PublicVariables.isColoring = false;
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
        }
    }

    public void HealthSystem()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (health <= 0)
        {
            KillPlayer();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            health += 1;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            health -= 1;
        }
    }

    public void TakeDamage(int damage, float enemyX)
    {
        PlayerAnimator.SetTrigger("hurt");
        health -= damage;
        if (health <= 0)
        {
            KillPlayer();
        }

        double playerX = gameObject.transform.position.x;
        if (enemyX > playerX)
        {
            rb.AddForce(new Vector2(-300, 500));
        }
        else
        {
            rb.AddForce(new Vector2(300, 500));
        }
    }

    public void KillPlayer()
    {
        hearts[0].sprite = emptyHeart;
        PlayerAnimator.SetTrigger("die");
        GetComponent<MonoBehaviour>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void ChangeColor(int direction)
    {
        currentColorIndex += direction;
        if (currentColorIndex == 6)
        {
            currentColorIndex = 0;
        }
        else if (currentColorIndex == -1)
        {
            currentColorIndex = 5;
        }
        colors[currentColorIndex].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        for (int i = 0; i < colors.Length; i++)
        {
            if (i != currentColorIndex)
            {
                colors[i].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
        }
        ColorManager.Instance.CurrentColor = colors[currentColorIndex].name;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(feetPos.position, jumpBoxSize);
    }
}
