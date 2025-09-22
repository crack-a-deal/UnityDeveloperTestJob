using UnityEngine;

public interface IMovementStrategy
{
    void Move(Transform entity);
    Vector3 GetPredictedPosition(Vector3 currentPosition, float time);
    bool IsReachedTarget(Vector3 position, float reachDistance);
}