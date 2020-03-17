using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float lifetime = 1f;

    private void Update()
    {
        lifetime -= 0.05f;
        if (lifetime < 0f)
            Destroy(gameObject);
    }
}
