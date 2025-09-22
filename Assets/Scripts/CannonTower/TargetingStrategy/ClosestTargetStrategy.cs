using UnityEngine;

public class ClosestTargetStrategy : ITargetingStrategy
{
    private readonly Vector3 m_towerPosition;
    private readonly float m_range;
    private readonly LayerMask m_monsterLayer;
    private Collider[] m_monstersPool = new Collider[10];

    public ClosestTargetStrategy(Vector3 towerPosition, float range, LayerMask monsterLayer)
    {
        m_towerPosition = towerPosition;
        m_range = range;
        m_monsterLayer = monsterLayer;
    }

    public Monster SelectTarget()
    {
        int monsterCount = Physics.OverlapSphereNonAlloc(m_towerPosition, m_range, m_monstersPool, m_monsterLayer);

        if (monsterCount == 0)
            return null;

        Monster closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (Collider target in m_monstersPool)
        {
            if (target == null)
                continue;

            float distance = Vector3.Distance(m_towerPosition, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.GetComponent<Monster>();
            }
        }

        return closestTarget;
    }
}