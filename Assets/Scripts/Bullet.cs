using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public float dstToMove = 5f;
    private float dstMoved = 0f;

    void Update()
    {
        if (dstMoved >= dstToMove)
        {
            Destroy(gameObject);
            return;
        }

        float dst = Time.deltaTime * speed;
        dstMoved += dst;
        transform.position += transform.up * dst;
    }
}
