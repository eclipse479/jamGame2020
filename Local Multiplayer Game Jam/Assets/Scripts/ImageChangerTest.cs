using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangerTest : MonoBehaviour
{
    public Image p1Gun;

    public Sprite normalGun;
    public Sprite bounceGun;
    public Sprite explosiveGun;
    public Sprite fastGun;
    public Sprite spreadGun;
    public Sprite burstGun;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        p1Gun.sprite = spreadGun;
    }
}
