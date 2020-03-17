using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpStation : MonoBehaviour
{
    public GameObject PowerUp;
    public float maxTimer;
    private float timer;
    private Vector3 up;
    // Start is called before the first frame update
    void Start()
    {
        up = new Vector3(0, 1, 0);
        timer = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            Instantiate(PowerUp,(transform.position + up), transform.rotation);
            timer += maxTimer;
        }
        else
        {
        timer -= Time.deltaTime;
        }
    }
}
