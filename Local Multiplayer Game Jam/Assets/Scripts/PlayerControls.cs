
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerControls : MonoBehaviour
{
    float xInput;
    float yInput;
    float xRotationInput = 0;
    float yRotationInput = 1;
    public float playerSpeed = 1;
    public int playerNumber;
    public float maxShootingTimer = 3;
    public float burstTimer = 1.0f;
    float shootingTimer;
    private Vector3 shootDirection;
    public GameObject projectile;
    public Transform spawner;
    Rigidbody body;
    private Vector3 lookDirrection;
    public Vector3 startLook;
    public Image ammoTracker;
    public float maxAmmo = 10;
    private float currentAmmo;
    
    public enum bulletType
    {
        normal,
        bouncy,
        explosive,
        fast,
        spread,
        burst
    };

    public float reloadSpeed;
    private float reloadTimer;
    private bool hasReloaded = true;

    private float reloadCounter = 0;
    private float maxRedloadCounter = 100;
    public Image gunType;
    public Sprite normalGun;
    public Sprite bounceGun;
    public Sprite explosiveGun;
    public Sprite fastGun;
    public Sprite spreadGun;
    public Sprite burstGun;

    public bulletType currentBullet;

    public AudioClip bulletShot;
    public AudioClip powerUpSound;

    private AudioSource audio;

    void Start()
    {
        currentAmmo = maxAmmo;
        body = gameObject.GetComponent<Rigidbody>();
        shootingTimer = maxShootingTimer;
        currentBullet = bulletType.normal;
        reloadTimer = reloadSpeed;
        shootDirection = startLook;
        shootDirection.Normalize();

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //gets the input form the left control stick form the Xbox controller
        xInput = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerNumber);
        yInput = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerNumber);
        //makes the player move speed more uniform
        xInput *= playerSpeed * Time.deltaTime;
        yInput *= playerSpeed * Time.deltaTime;
        //moves the player
        Vector3 position = new Vector3(xInput, 0, yInput);
        transform.Translate(position, Space.World);
        //when the "A" button is held down
        if (XCI.GetButton(XboxButton.RightBumper, (XboxController)playerNumber) && shootingTimer >= maxShootingTimer && currentAmmo > 0)
        {
            shootProjectile();
        }
        xRotationInput = XCI.GetAxis(XboxAxis.RightStickX, (XboxController)playerNumber);
        yRotationInput = XCI.GetAxis(XboxAxis.RightStickY, (XboxController)playerNumber);


        if (!(Mathf.Approximately(xRotationInput, 0.0f) && Mathf.Approximately(yRotationInput, 0.0f)))
        {
            shootDirection = new Vector3(xRotationInput, 0, yRotationInput);
            shootDirection.Normalize();

        }

        //deadzone on joysticks
        if (xRotationInput < 0.1f && xRotationInput > -0.1f)
            xRotationInput = 0;
        if (yRotationInput < 0.1f && yRotationInput > -0.1f)
            yRotationInput = 0;
        //rotate the playre in the direction the right joystick is faceing
        if (xRotationInput != 0 || yRotationInput != 0)
        {
            lookDirrection = new Vector3((xRotationInput), 0, (yRotationInput));
            lookDirrection.Normalize();
            transform.rotation = Quaternion.LookRotation(lookDirrection, new Vector3(0, 1, 0));
        }
        //shooting timer count
        if (shootingTimer < maxShootingTimer)
            shootingTimer += Time.deltaTime;

        body.velocity = new Vector3(0, 0, 0);

        MoveAmmo();

        if (reloadTimer <= reloadSpeed)
        {
            reloadTimer += Time.deltaTime;
            ammoTracker.fillAmount = reloadTimer / reloadSpeed;
        }
        else if (reloadTimer >= reloadSpeed && hasReloaded == false)
        {
            reload();
        }

    }

    void shootProjectile()
    {
        switch (currentBullet)
        {
            case bulletType.normal:
                {
                    //default bullet
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    audio.PlayOneShot(bulletShot);
                    break;
                }
            case bulletType.bouncy:
                {
                    //default bullet
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn.startDirection = shootDirection;
                    bulletSpawn.bouncy = true;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    audio.PlayOneShot(bulletShot);
                    break;
                }
            case bulletType.explosive:
                {
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    //makes the bullet larger
                    bulletSpawn.transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
                    bulletSpawn.explosive = true;
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    audio.PlayOneShot(bulletShot);
                    break;
                }
            case bulletType.fast:
                {
                    BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
                    //makes the bullet faster
                    bulletSpawn.fast = true;
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    audio.PlayOneShot(bulletShot);
                    break;
                }
            case bulletType.spread:
                {
                    //first
                    BulletController bulletSpawn = Instantiate(projectile, (spawner.transform.position), Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    bulletSpawn.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    //second
                    BulletController bulletSpawn2 = Instantiate(projectile, (spawner.transform.position), (Quaternion.identity)).GetComponent<BulletController>();
                    bulletSpawn2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    shootDirection = Quaternion.AngleAxis(20, Vector3.up) * shootDirection;
                    bulletSpawn2.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn2.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    //third
                    BulletController bulletSpawn3 = Instantiate(projectile, (spawner.transform.position), Quaternion.identity).GetComponent<BulletController>();
                    bulletSpawn3.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    shootDirection = Quaternion.AngleAxis(-40, Vector3.up) * shootDirection;
                    bulletSpawn3.startDirection = shootDirection;
                    Physics.IgnoreCollision(bulletSpawn3.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    shootDirection = Quaternion.AngleAxis(20, Vector3.up) * shootDirection;
                    audio.PlayOneShot(bulletShot);
                    break;
                }
            case bulletType.burst:
                {
                    StartCoroutine(ShootThree());
                    break;
                }
        }

        shootingTimer = 0;
        Debug.Log(currentAmmo);
        currentAmmo--;
        ammoTracker.fillAmount = currentAmmo / maxAmmo;

        if (currentAmmo == 0)
        {
            currentBullet = bulletType.normal;
            gunType.sprite = normalGun;
            reloadTimer = 0;
            hasReloaded = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "powerUp")
        {
            audio.PlayOneShot(powerUpSound);
            powerUp newpower = collision.gameObject.GetComponent<powerUp>();
            currentBullet = (bulletType)newpower.currentPower;
            hasReloaded = true;
            reloadTimer = reloadSpeed;
            switch (currentBullet)
            {
                case bulletType.normal:
                    {
                        maxAmmo = 10;
                        currentAmmo = 10;
                        gunType.sprite = normalGun;
                        ammoTracker.fillAmount = maxAmmo / maxAmmo;
                        break;
                    }
                case bulletType.bouncy:
                    {
                        maxAmmo = 10;
                        currentAmmo = 10;
                        gunType.sprite = bounceGun;
                        ammoTracker.fillAmount = maxAmmo / maxAmmo;
                        break;
                    }
                case bulletType.explosive:
                    {
                        maxAmmo = 4;
                        currentAmmo = 4;
                        gunType.sprite = explosiveGun;
                        ammoTracker.fillAmount = maxAmmo / maxAmmo;
                        break;
                    }
                case bulletType.fast:
                    {
                        maxAmmo = 7;
                        currentAmmo = 7;
                        gunType.sprite = fastGun;
                        ammoTracker.fillAmount = maxAmmo / maxAmmo;
                        break;
                    }
                case bulletType.spread:
                    {
                        maxAmmo = 10;
                        currentAmmo = 10;
                        gunType.sprite = spreadGun;
                        ammoTracker.fillAmount = maxAmmo / maxAmmo;
                        break;
                    }
                case bulletType.burst:
                    {
                        maxAmmo = 5;
                        currentAmmo = 5;
                        gunType.sprite = burstGun;
                        ammoTracker.fillAmount = maxAmmo / maxAmmo;
                        break;
                    }
            }
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ShootThree()
    {
        for (int i = 0; i < 3; i++)
        {
            BulletController bulletSpawn = Instantiate(projectile, spawner.transform.position, Quaternion.identity).GetComponent<BulletController>();
            bulletSpawn.bursting = true;
            bulletSpawn.startDirection = shootDirection;
            Physics.IgnoreCollision(bulletSpawn.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            audio.PlayOneShot(bulletShot);
            yield return new WaitForSeconds(burstTimer);
        }
    }

    void MoveAmmo()
    {
        //moves the ammo ring above the player
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
        ammoTracker.transform.position = namePos;
    }

    void reload()
    {
        hasReloaded = true;
        maxAmmo = 10;
        currentAmmo = 10;
        ammoTracker.fillAmount = maxAmmo / maxAmmo;
    }
}

