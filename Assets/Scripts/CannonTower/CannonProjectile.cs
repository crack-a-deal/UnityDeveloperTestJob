using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CannonProjectile : MonoBehaviour
{
    [SerializeField] private float m_speed = 0.2f;
    [SerializeField] private int m_damage = 10;

    private Rigidbody m_rigidbody;

    public float Speed => m_speed;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 velocity)
    {
        m_rigidbody.velocity = velocity;
    }

    public void SetGravity(bool useGravity)
    {
        m_rigidbody.useGravity = useGravity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<IDamagable>(out var monster))
        {
            return;
        }

        monster.TakeDamage(m_damage);
        Destroy(gameObject);
    }
}
