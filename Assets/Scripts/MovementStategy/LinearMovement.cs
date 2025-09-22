using UnityEngine;

public class LinearMovement : BaseMonsterMovement
{
    private Vector3 m_targetPosition;

    public LinearMovement(float speed) : base(speed)
    {
    }

    public LinearMovement(Vector3 targetPosition, float speed) : base(speed)
    {
        SetTarget(targetPosition);
    }

    public override void Move(Transform entity)
    {
        Vector3 translation = m_targetPosition - entity.position;
        if (translation.magnitude < 0.001)
        {
            return;
        }

        entity.position += translation.normalized * m_speed * Time.deltaTime;
    }

    public void SetTarget(Vector3 targetPosition)
    {
        m_targetPosition = targetPosition;
    }

    public override Vector3 GetPredictedPosition(Vector3 currentPosition, float time)
    {
        Vector3 dir = (m_targetPosition - currentPosition).normalized;
        float distance = (m_targetPosition - currentPosition).magnitude;
        float travel = m_speed * time;

        if (travel >= distance)
            return m_targetPosition;

        return currentPosition + dir * travel;
    }

    public override bool IsReachedTarget(Vector3 position,float reachDistance)
    {
        return Vector3.Distance(position, m_targetPosition) <= reachDistance;
    }
}
