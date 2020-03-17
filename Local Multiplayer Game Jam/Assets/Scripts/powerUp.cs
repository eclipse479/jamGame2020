using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public enum powerUpType
    {
        normal,
        bouncy,
        explosive,
        fast,
        spread,
        burst
    };
    private int counter = 0;
    public powerUpType currentPower;
    private int rand = -1;
    // Start is called before the first frame update
    void Start()
    {
        
        rand = Random.Range(0, 5);
        switch (rand)
        {
            //give the powerup a random colour depending on the type of powerup
            case 0:
                {
                    currentPower = powerUpType.bouncy;
                    GetComponent<Renderer>().material.color = new Color(1, 0.5f, 0);
                    break;
                }
            case 1:
                {
                    currentPower = powerUpType.explosive;
                    GetComponent<Renderer>().material.color = Color.blue;
                    break;
                }
            case 2:
                {
                    currentPower = powerUpType.fast;
                    GetComponent<Renderer>().material.color = Color.magenta;
                    break;
                }
            case 3:
                {
                    currentPower = powerUpType.spread;
                    GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                }
            case 4:
                {
                    currentPower = powerUpType.burst;
                    GetComponent<Renderer>().material.color = Color.green;
                    break;
                }
        }

    }
    private void Update()
    {
        if(counter < 100)
        counter++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "powerUp")
        {
            powerUp choice = collision.gameObject.GetComponent<powerUp>();
            if (choice.counter < counter)
                Destroy(gameObject);
        }

        Debug.Log("SHIT");

    }

}
