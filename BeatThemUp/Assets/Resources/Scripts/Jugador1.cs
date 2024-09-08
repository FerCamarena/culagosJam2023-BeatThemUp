using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugador1 : MonoBehaviour {
    //General variables
    public Animator animator;

    //Instance parts
    public Rigidbody2D rb;
    public Collider2D attackRange;
    public Collider2D attackHitboxOne;
    public Collider2D attackHitboxTwo;
    public Collider2D characterHitbox;
    public SpriteRenderer front;
    public SpriteRenderer mid;
    public SpriteRenderer back;

    //Instance attributes
    public int direction = 1;
    public float maxHealth = 100.0f;
    public float currentHealth = 100.0f;
    public float maxXP = 100.0f;
    public float currentXP = 0.0f;
    public float maxEnergy = 100.0f;
    public float currentEnergy = 100.0f;
    public float speed = 5.0f;
    public float damage = 10.0f;
    public float movingFactor = 1.0f;
    public float jumpingFactor = 1.0f;
    public bool move = false;
    public bool jumping = false;
    public bool comboing = false;
    public bool alive = false;

    //Instance variables
    public States state;

    public void danar()
    {
        currentHealth = currentHealth - 10;
        if(currentHealth < 1)
        {
            SceneManager.LoadScene(5);
            state = States.Muerto;
        }
    }

    //Enums for states
    public enum States {
        Idle,
        Walk,
        Block,
        Jump,
        Harmed,
        Stunned,
        Simple,
        Strong,
        Combo,
        Muerto,
    }
    //Method called before first frame
    private void Start() {
    }

    //Method called each frame
    private void Update() {
        if(state != States.Muerto && Time.timeScale > 0) { 
            front.sortingOrder = ((int)(this.transform.position.y * 10) - 1) * -1;
            mid.sortingOrder = ((int)(this.transform.position.y * 10)) * -1;
            back.sortingOrder = ((int)(this.transform.position.y * 10) + 1) * -1;
        }

        //Animating the character
        if (alive) { 
            this.animator.SetInteger("State", (int)this.state);
            this.animator.SetBool("Moving", this.move);
            this.animator.SetBool("Jump", this.jumping);
        } 

        //Calculating for stuns

        //Calculating for damages

        //Calculating actions
        if (this.state == States.Idle) {
            if (Input.GetAxisRaw("Vertical1") != 0 || Input.GetAxisRaw("Horizontal1") != 0) {
                //Debug.Log("Move");
                this.state = States.Walk;
            } else if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Simple1")) {
                //Debug.Log("X");
                this.state = States.Simple;
            } else if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Strong1")) {
                //Debug.Log("B");
                this.state = States.Strong;
            } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump1")) {
                //Debug.Log("Jump");
                this.state = States.Jump;
            } else if (Input.GetKey(KeyCode.Q) || Input.GetButtonDown("Block1")) {
                //Debug.Log("Block");
                this.state = States.Block;
            }
        } else if (this.state == States.Walk) {
            if ((Input.GetKey(KeyCode.G) || Input.GetButtonDown("Simple1")) && (this.rb.velocity.x < -3.0f || this.rb.velocity.x > 3.0f)) {
                //Debug.Log("Charge X");
                this.state = States.Simple;
            } else if ((Input.GetKey(KeyCode.H) || Input.GetButtonDown("Strong1")) && (this.rb.velocity.x < -3.0f || this.rb.velocity.x > 3.0f)) {
                //Debug.Log("Charge B");
                this.state = States.Strong;
            } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump1")) {
                //Debug.Log("Jump");
                this.state = States.Jump;
            } else if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Block1")) {
                //Debug.Log("Block");
                this.state = States.Block;
            }
            if(!jumping) {
                this.move = true;
            }
        } else if (this.state == States.Block) {
            if (Input.GetKeyUp(KeyCode.Q) || Input.GetButtonUp("Block1")) {
                //Debug.Log("End block");
                this.state = States.Idle;
            }
        } else if (this.state == States.Jump) {
            if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Simple1")) {
                //Debug.Log("Air X");
                this.state = States.Simple;
            } else if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Strong1")) {
                //Debug.Log("Air B");
                this.state = States.Strong;
            }
            this.jumping = true;
        } else if (this.state == States.Simple) {
            if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Simple1")) {
                //Debug.Log("Combo X");
                this.state = States.Combo;
            } else if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Strong1")) {
                //Debug.Log("Combo XB");
                this.state = States.Strong;
            } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump1")) {
                //Debug.Log("Combo X Jump");
                this.state = States.Jump;
                this.attackHitboxTwo.enabled = true;
            }
        } else if (this.state == States.Combo) {
            if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Strong1")) {
                //Debug.Log("XXB");
                this.state = States.Strong;
            } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump1")) {
                //Debug.Log("XX Jump X");
                this.state = States.Jump;
            }
        }
    }

    //Method called each fixed time
    private void FixedUpdate() {
        Move();
        if(this.state == States.Strong || this.state == States.Simple || this.state == States.Combo) {
            Attack();
            if (move) {
                this.rb.AddForce(Vector2.right * speed * 1 * direction, ForceMode2D.Impulse);
            }
        }
    }

    //Method for making the player attack
    private void Attack() {
        this.attackHitboxOne.enabled = true;
    }

    //Method for moving the character
    private void Move() {
        if ((state == States.Idle || state == States.Walk) && !jumping) { 
            Vector2 velocity = Vector2.zero; 
            if(Input.GetAxisRaw("Horizontal1") != 0.0f) {
                velocity.x = Input.GetAxisRaw("Horizontal1") * 1.4f;
                if(this.rb.velocity.magnitude > 3.0f) { 
                    if(this.rb.velocity.x < 0f) {
                        this.direction = -1;
                    } else if(this.rb.velocity.x > 0) {
                        this.direction = 1;
                    }
                }
                this.transform.localScale = new Vector3Int(direction, 1, 1);
            }
            if(Input.GetAxisRaw("Vertical1") != 0.0f) {
                velocity.y = Input.GetAxisRaw("Vertical1");
            }
            this.rb.velocity = velocity * this.speed;
        } else {
            this.rb.velocity = Vector2.zero;
        }
    }

    //Calculating damage on attacks
    private float CalculateDamage() {
        float newDamage = this.damage;
        if (this.move) {
            newDamage *= this.movingFactor;
        }
        if (this.jumping) {
            newDamage *= this.jumpingFactor;
        }
        return newDamage;
    }

    //Method called once the colliders detect collisions
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Enemy") || other.CompareTag("Scenery")) {
            if (other.IsTouching(this.attackRange) && (other.IsTouching(this.attackHitboxOne) || other.IsTouching(this.attackHitboxTwo))) {
                //Enemy range must collide with character range
                //Then check for collisions on enemy hitbox
                other.GetComponent<EnemyAI>().Damage();
                Destroy(other.gameObject);
                this.attackHitboxOne.enabled = false;
                this.attackHitboxTwo.enabled = false;
            }
        }
    }
    
    public void AnimationReset() {
        this.state = States.Idle;
        this.jumping = false;
        this.comboing = false;
        this.move = false;
    }
    public void Spawned() {
        this.alive = true;
    }
    public void Died() {
    }

    public void OnAnimationMoving() {
    }
}