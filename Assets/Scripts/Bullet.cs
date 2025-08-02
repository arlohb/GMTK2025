using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public float dstToMove = 5f;
    public bool isFromEnemy;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform parent = collision.transform.parent;
        bool isActor = parent.TryGetComponent(out Actor actor);
        if (!isActor) return;

        actor.Hit(isFromEnemy);
    }
}
