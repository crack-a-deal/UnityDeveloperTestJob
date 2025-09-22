using UnityEngine;

public abstract class BaseMonsterMovement : IMovementStrategy
{
    protected readonly float m_speed;

    protected BaseMonsterMovement(float speed)
    {
        m_speed = speed;
    }

    public abstract void Move(Transform entity);
    public virtual Vector3 GetPredictedPosition(Vector3 currentPosition, float time)
    {
        return Vector3.zero;
    }

    public abstract bool IsReachedTarget(Vector3 position, float reachDistance);
}
