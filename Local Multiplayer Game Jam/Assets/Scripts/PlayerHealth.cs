using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentLifeHealth = 100;
    public Image heartIndicator;
    private int damageAmount;
    public GameObject deathParticle;

    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            damageAmount = bullet.damage;
            takeDamage();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BOOM")
        {
            damageAmount = 20;
            takeDamage();
        }
    }

    public void takeDamage()
    {
        currentLifeHealth -= damageAmount;
        Debug.Log(this.gameObject.name);
        Debug.Log(currentLifeHealth);

        heartIndicator.fillAmount = currentLifeHealth / 100;
      
        if (currentLifeHealth <= 0)
        {
            Instantiate(deathParticle,transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
            this.GetComponent<PlayerControls>().ammoTracker.gameObject.SetActive(false);
        }

    }
}
