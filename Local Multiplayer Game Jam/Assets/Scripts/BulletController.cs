using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 startDirection;
    public Vector3 forceToAdd;
    public PhysicMaterial pMaterial;
    public GameObject explosionPrefab;
    private Rigidbody rb;
    //how fast the bullets travel
    public float speed = 200f;
    //damage variables for the different bullet types
    public int normalDamage = 10;
    public int bouncyDamage = 10;
    public int explosiveDamage = 20;
    public int spreadDamage = 5;
    public int fastDamage = 10;
    public int burstDamage = 5;
    //how long each bullet will last
    public float lifetime = 5f;
    // colour of the bullet
    public Color color;

    [HideInInspector]
    //damage delt
    public int damage;
    //bullet type bool
    public bool explosive = false;
    public bool fast = false;
    public bool bouncy = false;
    public bool bursting = false;
    private bool hasBounce = false;

    private bool hasMoved = false;
    private float lifeTimer = 0f;

    void Awake()
    {
        //grab the rigidbosy, set starting size and colour
        rb = GetComponent<Rigidbody>();
        transform.localScale = startScale;
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }


    void Update()
    {
        //the inital push that adds velocity to the projectile 
        if (!hasMoved)
        {
            rb.AddForce(startDirection * speed, ForceMode.Impulse);
            hasMoved = true;
        }
        //if bullet type is bouncy
        if (bouncy)
        {
            GetComponent<SphereCollider>().material = pMaterial;
            bouncy = false;
            hasBounce = true;
            damage = bouncyDamage;
        }
        //if bullet type is fast
        else if (fast)
        {
            rb.AddForce(startDirection * speed * 2, ForceMode.Impulse);
            fast = false;
            damage = fastDamage;
        }
        // if bullet type is spread
        else if (forceToAdd != new Vector3(0, 0, 0))
        {
            rb.AddForce(forceToAdd * speed, ForceMode.Impulse);
            forceToAdd = new Vector3(0, 0, 0);
            damage = spreadDamage;
        }
        else if (explosive)
        {
            damage = explosiveDamage;
        }
        else if (bursting)
        {
            damage = burstDamage;
        }
        else
        {
            damage = normalDamage;
        }
        lifeTimer += Time.deltaTime;
        //if it runs out of lifetime despawn
        if (lifeTimer > lifetime)
        {
            if (explosive)
                Explode();

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosive)
            Explode(); // destroys the object anyway
        if (hasBounce)
        {
            //allows the bouncy type bullet to bounce off an object once
            hasBounce = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
