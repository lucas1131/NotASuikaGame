using UnityEngine;

public class PieceGraphics : MonoBehaviour, IPieceGraphics
{
    float gravity;

    Rigidbody2D rb;
    CircleCollider2D collider;

    IPieceController pieceController;

    public float Radius => collider.radius * transform.lossyScale.x;
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    void Awake()
    {
        // Make sure object has no physics before enabling physics -- just setting isKinematic still
        collider = gameObject.GetComponent<CircleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        EnablePhysics(false);
    }

    public void Setup(IPieceController pieceController, bool enablePhysics, float gravity)
    {
        this.pieceController = pieceController;
        this.gravity = gravity;
        EnablePhysics(enablePhysics);
    }

    public void Destroy()
    {
        Object.Destroy(gameObject);
    }

    public void EnablePhysics(bool enable)
    {
        collider.enabled = enable;
        rb.isKinematic = !enable;
        rb.gravityScale = gravity * (enable ? 1f : 0f);
    }

    public void ApplyScaleFactor(float scale) => transform.localScale *= scale;
    public void SetMass(float mass) => rb.mass = mass;
    public void SetGravity(float gravity) => rb.gravityScale = gravity;

    void OnCollisionEnter2D(Collision2D collision)
    {
        PieceGraphics other = collision.gameObject.GetComponent<PieceGraphics>();
        if (other == null) return;

        pieceController.OnCollision(other.pieceController);
    }
}
