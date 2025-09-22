using UnityEngine;

public class CircularMovement : BaseMonsterMovement
{
    private readonly Vector3 m_towerPosition;
    private readonly float m_radius;

    private float m_currentAngle = 0f;

    public CircularMovement(Vector3 towerPosition, float radius, float angularSpeed) : base(angularSpeed)
    {
        m_towerPosition = towerPosition;
        m_radius = radius;
    }

    public override void Move(Transform entity)
    {
        m_currentAngle += m_speed * Time.deltaTime;

        float rad = m_currentAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * m_radius;
        entity.position = m_towerPosition + offset;
    }

    public override Vector3 GetPredictedPosition(Vector3 currentPosition, float time)
    {
        float futureAngle = m_currentAngle + m_speed * time;
        float rad = futureAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * m_radius;
        return m_towerPosition + offset;
    }

    public override bool IsReachedTarget(Vector3 position, float reachDistance)
    {
        // For endless movement
        return false;
    }
}
