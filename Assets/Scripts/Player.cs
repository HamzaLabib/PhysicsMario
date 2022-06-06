using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character { Mario, Luigi}

public class Player : MonoBehaviour
{
    public Character character;

    public RoundManager manager;

    public float burstForce;
    public float dampener;
    public float rollBurstForce;

    public float maxhealth = 100f;
    float currentHealth;
    public Vector2 collisionDamageRange;
    public Vector2 collisionForceRange;
    protected bool isAlive = true;

    public UnityEngine.UI.Slider healthSlider;

    // General
    public Transform spriteObject;
    CapsuleCollider2D coli;
    Rigidbody2D rb;
    protected virtual bool FacingRight { get; set; }
    bool isFlipping;

    public float walkForce;
    public float flyForce;

    // for flying
    #region Flying
    [Space(20)]
    [Header("Flying")]
    public BoxCollider2D groundColi;
    public float jumpForce;
    public float secondJumpForce;
    public float flipForce;
    bool flippingRight;
    bool doubleJumpAllowed;
    #endregion

    // for rolling
    public float rollForce;

    // for bomb
    float timeCharged = 0;
    bool isCharging;
    public GameObject bombSprite; // fake one
    GameObject bombToclone;
    public float timeToMaxCharge;
    public Vector2 throwDir;
    public Vector2 throwForceRange;

    public float bombCoolDown;
    float coolDownTimer = 0;
    bool coolingDown;
    public GameObject coolDownSprite;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
        rb = GetComponent<Rigidbody2D>();
        coli = GetComponent<CapsuleCollider2D>();
        bombToclone = Resources.Load<GameObject>("Prefabs/bomb");
    }

    // Update is called once per frame
    void Update()
    {
        ProccessInput();
        if (isFlipping)
            CheckToStopFlip(flippingRight); // to calculate the angle of mario to check stop it or no
        if (isCharging)
            timeCharged += Time.deltaTime;
        if (coolingDown)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= bombCoolDown) // cool down has ended
            {
                coolingDown = false;
                coolDownTimer = 0;
                coolDownSprite.SetActive(true);
            }
        }

        //Debug.Log(transform.rotation.eulerAngles.z);
    }

    protected virtual void ProccessInput()
    {

    }


    protected void MoveLeft()
    {
        if (FacingRight)
            ChangeDirection(false);

        rb.AddForce(Vector2.left * (IsGrounded() ? walkForce : flyForce) * Time.deltaTime);
    }

    protected void MoveRight()
    {
        if (!FacingRight)
            ChangeDirection(true);

        rb.AddForce(Vector2.right * (IsGrounded() ? walkForce : flyForce) * Time.deltaTime);
    }

    protected void BurstLeft()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.left * burstForce, ForceMode2D.Impulse);
        }
    }

    protected void BurstRight()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.right * burstForce, ForceMode2D.Impulse);
        }
    }

    protected void DampenXLeft()
    {
        if (IsGrounded() && rb.velocity.x < 0)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * dampener, 0, 0), ForceMode2D.Impulse);
        }
    }

    protected void DampenXRight()
    {
        if (IsGrounded() && rb.velocity.x > 0)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * dampener, 0, 0), ForceMode2D.Impulse);
        }
    }

    protected void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartFlip(FacingRight, flipForce);
            doubleJumpAllowed = true;
        }
        else if (doubleJumpAllowed)
        {
            rb.AddForce(Vector2.up * secondJumpForce, ForceMode2D.Impulse);
            doubleJumpAllowed = false;
            StartFlip(FacingRight, flipForce);
        }
    }

    protected void MoveDown()
    {
        rb.AddForce(Vector2.down * (walkForce / 2) * Time.deltaTime);
    }

    bool IsGrounded()
    {
        //return coli.IsTouching(groundColi);
        List<Collider2D> hitList = new List<Collider2D>();
        coli.OverlapCollider(new ContactFilter2D(), hitList); // contactfilter for layers
        foreach (Collider2D hit in hitList)
        {
            if (hit.CompareTag("ground"))
                return true;
        }
        return false;
    }


    void ChangeDirection(bool faceRight)
    {
        spriteObject.localScale = new Vector3((faceRight ? 1 : -1), 1, 1);
        FacingRight = faceRight;
    }


    void StartFlip(bool flipRight, float force)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddTorque((flipRight ? -force : force)); // because we split sprite
        isFlipping = true;
        flippingRight = flipRight;
    }

    void CheckToStopFlip(bool right)
    {
        if (transform.rotation.eulerAngles.z < (right ? 20 : 360) && transform.rotation.eulerAngles.z > (right ? 0 : 340))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.identity;
            isFlipping = false;
        }

    }

    protected void Roll(bool right)
    {
        if (IsGrounded())
        {
            StartFlip(right, rollForce);
            rb.AddForce(right ? Vector2.right * rollBurstForce : Vector2.left * rollBurstForce, ForceMode2D.Impulse);
        }
    }


    protected void StartChargingBomb()
    {
        if (!coolingDown)
        {
            bombSprite.SetActive(true);
            isCharging = true;
            coolDownSprite.SetActive(false);
        }
    }

    protected void ThrowBomb()
    {
        if (!coolingDown)
        {
            bombSprite.SetActive(false);
            isCharging = false;

            GameObject bombObj = GameObject.Instantiate<GameObject>(bombToclone, bombSprite.transform.position, Quaternion.identity);
            Rigidbody2D bombRB = bombObj.GetComponent<Rigidbody2D>();

            Vector2 trueThrowDir = new Vector2((FacingRight ? throwDir.x : -throwDir.x), throwDir.y).normalized;
            float throwForce = Mathf.Lerp(throwForceRange.x, throwForceRange.y, Mathf.Clamp01(timeCharged / timeToMaxCharge));

            bombRB.AddForce(transform.TransformDirection(trueThrowDir) * throwForce, ForceMode2D.Impulse);


            timeCharged = 0;
            coolingDown = true;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isAlive)
        {
            Debug.Log(amount);
            currentHealth -= amount;
            healthSlider.value = currentHealth / maxhealth;
            if (currentHealth <= 0)
                Die();
        }
    }

    void Die()
    {
        spriteObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f);
        rb.constraints = RigidbodyConstraints2D.None;
        isAlive = false;
        manager.RoundEnds(character); // cuz we stored already on the unity through the variable character
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collisionForceRange);
        float force = collision.contacts[0].normalImpulse; // how hard he hit
        if (force > collisionForceRange.x)
        {
            float interpolator = Mathf.Clamp01(Mathf.InverseLerp(collisionForceRange.x, collisionForceRange.y, force));
            float damage = Mathf.Lerp(collisionDamageRange.x, collisionDamageRange.y, interpolator);
            bombSprite.SetActive(false);
            coolDownSprite.SetActive(false);
            TakeDamage(damage);
        }
    }
}
