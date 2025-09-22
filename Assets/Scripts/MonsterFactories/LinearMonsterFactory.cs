using UnityEngine;
using UnityEngine.Pool;

public class LinearMonsterFactory : BaseMonsterFactory<LinearFactorySettings>
{
    private readonly Vector3 m_spawnPoint;
    private readonly LinearFactorySettings m_settings;

    public LinearMonsterFactory(ObjectPool<Monster> pool, Vector3 spawnPoint, LinearFactorySettings settings) : base(pool)
    {
        m_spawnPoint = spawnPoint;
        m_settings = settings;
    }

    public override Monster Create()
    {
        Monster newMonster = m_pool.Get();
        newMonster.name = "LinearMonster";
        newMonster.transform.position = m_spawnPoint;
        newMonster.Init(new LinearMovement(m_settings.TargetPosition, m_settings.Speed));

        return newMonster;
    }
}
