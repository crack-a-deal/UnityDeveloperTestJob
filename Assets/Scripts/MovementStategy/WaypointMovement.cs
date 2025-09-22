using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : BaseMonsterMovement
{
    const float REACH_DISTANCE = 0.2f;

    private readonly List<Vector3> m_waypoints;

    private int m_currentIndex = 0;

    private LinearMovement m_linearMovement;
    private Vector3 m_target;

    public WaypointMovement(List<Vector3> waypoints, float speed) : base(speed)
    {
        m_waypoints = waypoints;
        m_linearMovement = new LinearMovement(speed);
    }

    public override void Move(Transform entity)
    {
        if (m_waypoints == null || m_waypoints.Count == 0)
        {
            return;
        }

        m_target = m_waypoints[m_currentIndex];

        m_linearMovement.SetTarget(m_target);
        m_linearMovement.Move(entity);

        if (Vector3.Distance(entity.transform.position, m_target) < REACH_DISTANCE)
        {
            m_currentIndex++;
            if (m_currentIndex >= m_waypoints.Count)
            {
                m_currentIndex = 0;
            }
        }
    }

    public override Vector3 GetPredictedPosition(Vector3 currentPosition, float time)
    {
        Vector3 pos = currentPosition;
        float remainingTime = time;
        float speed = m_speed;

        int index = m_currentIndex;
        while (remainingTime > 0 && m_waypoints.Count > 0)
        {
            Vector3 target = m_waypoints[index];
            float distance = Vector3.Distance(pos, target);
            float travel = speed * remainingTime;

            if (travel >= distance)
            {
                pos = target;
                remainingTime -= distance / speed;
                index = (index + 1) % m_waypoints.Count;
            }
            else
            {
                Vector3 dir = (target - pos).normalized;
                pos += dir * travel;
                remainingTime = 0;
            }
        }

        return pos;
    }

    public override bool IsReachedTarget(Vector3 position, float reachDistance)
    {
        // For endless movement
        return false;
    }
}