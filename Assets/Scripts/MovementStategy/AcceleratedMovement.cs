using UnityEngine;

public class AcceleratedMovement : BaseMonsterMovement
{
    private readonly Vector3 m_targetPosition;
    private readonly float m_acceleration;

    private float m_currentSpeed;
    public AcceleratedMovement(Vector3 targetPosition, float initialSpeed, float acceleration) : base(initialSpeed)
    {
        m_targetPosition = targetPosition;
        m_acceleration = acceleration;
        m_currentSpeed = initialSpeed;
    }

    public override void Move(Transform entity)
    {
        Vector3 translation = m_targetPosition - entity.position;

        m_currentSpeed += m_acceleration * Time.deltaTime;
        entity.position += translation.normalized * m_currentSpeed * Time.deltaTime;
    }

    public override Vector3 GetPredictedPosition(Vector3 currentPosition, float time)
    {
        Vector3 dir = (m_targetPosition - currentPosition).normalized;
        float predictedDistance = m_currentSpeed * time + 0.5f * m_acceleration * time * time;

        float distance = (m_targetPosition - currentPosition).magnitude;
        if (predictedDistance >= distance)
            return m_targetPosition;

        return currentPosition + dir * predictedDistance;
    }

    public override bool IsReachedTarget(Vector3 position, float reachDistance)
    {
        return Vector3.Distance(position, m_targetPosition) <= reachDistance;
    }
}
