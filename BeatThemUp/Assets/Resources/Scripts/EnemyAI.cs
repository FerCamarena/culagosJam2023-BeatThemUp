
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour {


    public Animator animator;
    public Collider2D hitbox;
    public Collider2D rango;
    public Collider2D ataque;
    public Rigidbody2D rb;

    public GameObject player1;
    public GameObject player2;

    public int currentHealth = 100;

    public int attack_cooldown;
    //Method called before first frame
    private void Start() {
    }
    //Method called each frame
    private void Update() {
        if(rb.velocity.x > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rb.velocity.x < 0)
        {

            this.transform.localScale = new Vector3(-1, 1,1);
        }

        if (!player1)
        {
            player1 = GameObject.FindWithTag("Player1");

        }
        if (!player2)
        {

            player2 = GameObject.FindWithTag("Player2");
        }
        if (player1)
        {
            if (player2)
            {
                if (Vector2.Distance(this.transform.position, player1.transform.position) <= Vector2.Distance(this.transform.position, player2.transform.position))
                {
                    if(this.transform.position.x < player1.transform.position.x - .5)
                    {
                        this.rb.velocity = Vector2.right;
                    } else if (this.transform.position.x > player1.transform.position.x + .5)
                    {
                        this.rb.velocity = Vector2.left;
                    }
                    if (this.transform.position.y < player1.transform.position.y -.4)
                    {
                        this.rb.velocity += Vector2.up;
                    }
                    else if (this.transform.position.y > player1.transform.position.y + .4)
                    {
                        this.rb.velocity += Vector2.down;
                    }
                } else
                {
                    if (this.transform.position.x < player2.transform.position.x)
                    {
                        this.rb.velocity = Vector2.right;
                    }
                    else
                    {
                        this.rb.velocity = Vector2.left;
                    }
                    if (this.transform.position.y < player1.transform.position.y)
                    {
                        this.rb.velocity += Vector2.up;
                    }
                    else
                    {
                        this.rb.velocity += Vector2.down;
                    }
                }
            } else
            {
                if (this.transform.position.x < player1.transform.position.x - .5)
                {
                    this.rb.velocity = Vector2.right;
                }
                else if (this.transform.position.x > player1.transform.position.x + .5)
                {
                    this.rb.velocity = Vector2.left;
                }
                if (this.transform.position.y < player1.transform.position.y - .4)
                {
                    this.rb.velocity += Vector2.up;
                }
                else if (this.transform.position.y > player1.transform.position.y + .4)
                {
                    this.rb.velocity += Vector2.down;
                }
            }
        }
    }
    //Method called each fixed time
    private void FixedUpdate() {

    }
    //Method called for harming
    public void Damage() {
        currentHealth = currentHealth - 20;
        if (currentHealth < 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void atacar() {
        ataque.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(attack_cooldown < 300)
        {
            ataque.enabled = false;
            attack_cooldown++;
        } else
        {
            ataque.enabled = true;
            attack_cooldown = 0;
            if (collision.CompareTag("Player1"))
            {
                collision.GetComponent<Jugador1>().danar();
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
            }
            if (collision.CompareTag("Player2"))
            {
                collision.GetComponent<Jugador2>().danar();
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
            }
        }
    }
}
