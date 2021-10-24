using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public float jump;
    public Text score;
    public Text lives;
    private int scoreValue = 0;
    private int livesValue = 3;
    public GameObject winText;
    public GameObject loseText;
    
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public AudioClip musicClipOne; 
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    private bool facingRight = true;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        
        winText.SetActive(false);
        loseText.SetActive(false);

        musicSource.clip = musicClipOne;
        musicSource.Play();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * jump));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    
    }


    void Update()
    {
        float hozMovement = Input.GetAxis("Horizontal");

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (isOnGround)
            {
                anim.SetInteger("State", 0);
            }
        }
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
        }
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
        }
        if (collision.collider.tag == "Coin")
        {
            if (scoreValue == 4)
            {
                transform.position = new Vector2 (1.0f, -26.5f);
                livesValue = 3;
                lives.text = "Lives: 3";
            }
            if (scoreValue == 8)
            {
                musicSource.Stop();
                musicSource.clip = musicClipTwo;
                musicSource.Play();
            }
        }

        if (scoreValue == 8)
        {
            winText.SetActive(true);
        }
        if (livesValue <=0)
        {
            winText.SetActive(false);
            loseText.SetActive(true);
            Destroy(this);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2 (0, 3), ForceMode2D.Impulse);
            }
        }
    }
}
