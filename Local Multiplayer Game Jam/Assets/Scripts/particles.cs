using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particles : MonoBehaviour
{
    public float timeTillparticlesStop;
    public float timeTillDeath;
    private float timer = 0;
    public ParticleSystem smoke;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeTillparticlesStop)
        {
            smoke.Stop();
        }
        if (timer > timeTillDeath)
        {
            Destroy(gameObject);
        }
    }
}
