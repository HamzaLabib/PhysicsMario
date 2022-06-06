using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Vector2 damageRange;
    public float explosionRadius;
    public Vector2 forceRange;
    public float timeToExplode;
    public Gradient colorGradient;
    public SpriteRenderer rend;
    public SpriteRenderer explosionSprite;
    public float explosionFadeTime;

    float fadeTimer = 0;
    float timer = 0;
    bool hasExploded;

    // Start is called before the first frame update
    void Start() // first frame after instiate a bomb
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToExplode && !hasExploded)
            Explode();

        rend.color = colorGradient.Evaluate(timer / timeToExplode);

        if (hasExploded)
        {
            fadeTimer += Time.deltaTime;
            explosionSprite.color = Color.Lerp(Color.white /*new Color(1,1,1,1)*/, new Color(1, 1, 1, 0), fadeTimer / explosionFadeTime);
            if (fadeTimer >= explosionFadeTime)
                GameObject.Destroy(this.gameObject);
        }
    }

    void Explode()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            Rigidbody2D rb = hitObjects[i].GetComponent<Rigidbody2D>();
            if (rb)
            {
                Vector2 dir = (hitObjects[i].transform.position - this.transform.position).normalized;
                float distance = Vector3.Distance(hitObjects[i].transform.position, this.transform.position);
                float interpolator = distance / explosionRadius;
                float force = Mathf.Lerp(forceRange.y, forceRange.x, interpolator);
                rb.AddForce(dir * force, ForceMode2D.Impulse);
                if(rb.CompareTag("Player"))
                {
                    float damageInterolator = Mathf.Clamp01(Mathf.InverseLerp(forceRange.x, forceRange.y, force));
                    float damage = Mathf.Lerp(damageRange.x, damageRange.y, damageInterolator);
                    Player p = rb.GetComponent<Player>(); // player script in mario or luigi
                    p.TakeDamage(damage);
                }
            }
        }
        explosionSprite.gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<CircleCollider2D>().enabled = false;
        rend.enabled = false;
        hasExploded = true;
        //GameObject.Destroy(this.gameObject);
    }
}
