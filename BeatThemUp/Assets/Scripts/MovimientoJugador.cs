using UnityEngine;
public class MovimientoJugador : MonoBehaviour {
    //General variables
    public Animator animator;

    //Intance parts
    public Collider2D collisionBox;
    public Collider2D attackRange;
    public Collider2D attackHitbox;
    public Collider2D characterHitbox;
    public GameObject characterSprite;

    //Instance attributes
    public int lifepoints;
    public float maxHealth = 100.0f;
    public float currentHealth = 100.0f;
    public float maxXP = 100.0f;
    public float currentXP = 0.0f;
    public float maxEnergy = 100.0f;
    public float currentEnergy = 100.0f;
    public float speed = 5.0f;
    public float damage = 10.0f;
    public float jumpingFactor = 1.0f;
    public bool attacking = false;

    //Instance variables
    public MovementState moveState = MovementState.Idle;
    public float attackCooldown = 0.0f;
    public float jumpCooldown = 0.0f;

    //Enums for states
    public enum MovementState {
        Idle,
        Walk,
        Attack,
        Block,
        Jump,
        Stun,
    }
    //Method called before first frame
    private void Start() {
    }
    //Method called each frame
    private void Update() {
        /*
        if(attackCooldown > 0.0f) {
            attackHitbox.enabled = false;
        }*/
        if (Input.GetKeyDown(KeyCode.J) && this.attackCooldown < 0.05f) {
            if (this.moveState != MovementState.Jump) {
                this.moveState = MovementState.Attack;
            } else { 
                this.moveState = MovementState.Jump;
            }
            this.attacking = true;
            this.attackHitbox.enabled = true;
            this.attackCooldown = 0.1f;
        }
        
        if (this.jumpCooldown < 0.05f && this.moveState != MovementState.Attack) {
            if (!this.attacking) { 
            this.moveState = MovementState.Idle;
            }
            if (Input.GetKey(KeyCode.Space)) { 
                this.moveState = MovementState.Jump;
                this.attacking = false;
                this.jumpCooldown = 0.5f;
            }
        }
    }
    //Method called each fixed time
    private void FixedUpdate() {
        Vector2 newPosition = this.transform.position;
        if(!(this.moveState == MovementState.Attack || this.moveState == MovementState.Jump)) {
            if (Input.GetKey(KeyCode.W)) {
                newPosition.y += 0.5f * this.speed * Time.fixedDeltaTime;
                this.moveState = MovementState.Walk;
            } else if(Input.GetKey(KeyCode.S)) {
                newPosition.y -= 0.33f * this.speed * Time.fixedDeltaTime;
                this.moveState = MovementState.Walk;
            }
            if(Input.GetKey(KeyCode.D)) {
                newPosition.x += this.speed * Time.fixedDeltaTime;
                this.moveState = MovementState.Walk;
            } else if(Input.GetKey(KeyCode.A)) {
                newPosition.x -= this.speed * Time.fixedDeltaTime;
                this.moveState = MovementState.Walk;
            } 
            if(!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D))){
                this.moveState = MovementState.Idle;
            }
            this.transform.position = newPosition;
        }
        if(this.attackCooldown > 0.0f) {
            this.attackCooldown -= Time.fixedDeltaTime;
        }
        if(this.jumpCooldown > 0.0f) {
            this.jumpCooldown -= Time.fixedDeltaTime;
        }
        animator.SetInteger("AnimationStateIndex", (int)moveState);
        animator.SetBool("isAttacking", this.attacking);
    }
    private float CalculateDamage() {
        float newDamage = this.damage;
        if(this.moveState == MovementState.Walk) {
            newDamage *= this.jumpingFactor;
        }
        return newDamage;
    }
    //Method called once the colliders detect collisions
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Enemy") || other.CompareTag("Scenery")) {
            if (other.IsTouching(this.attackRange) && other.IsTouching(this.attackHitbox)) {
                //Enemy range must collide with character range
                //Then check for collisions on enemy hitbox
                other.GetComponent<EnemyAI>().Damage(CalculateDamage());
                this.attackHitbox.enabled = false;
            }
        }
    }

    public void AttackAnimationEnded() {
        this.moveState = MovementState.Idle;
        this.attacking = false;
    }
}