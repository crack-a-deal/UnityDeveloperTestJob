using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    const float REACH_DISTANCE = 0.3f;

    [SerializeField] private int m_maxHP = 30;
    private int m_currentHP;

    private IMovementStrategy m_movementStrategy;

    void Start()
    {
        m_currentHP = m_maxHP;
    }

    private void Update()
    {
        m_movementStrategy.Move(transform);
        if (m_movementStrategy.IsReachedTarget(transform.position, REACH_DISTANCE))
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Init(IMovementStrategy movementStrategy)
    {
        m_movementStrategy = movementStrategy;
    }

    public void TakeDamage(int damage)
    {
        m_currentHP -= damage;
        if (m_currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetPredictedPosition(float time)
    {
        return m_movementStrategy.GetPredictedPosition(transform.position, time);
    }
}
