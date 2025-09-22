using UnityEngine;
using UnityEngine.Pool;

public class AcceleratedMonsterFactory : BaseMonsterFactory<AcceleratedFactorySettings>
{
    private readonly Vector3 m_spawnPoint;
    private readonly AcceleratedFactorySettings m_settings;

    public AcceleratedMonsterFactory(ObjectPool<Monster> pool, Vector3 spawnPoint, AcceleratedFactorySettings settings) : base(pool)
    {
        m_spawnPoint = spawnPoint;
        m_settings = settings;
    }

    public override Monster Create()
    {
        Monster newMonster = m_pool.Get();
        newMonster.name = "AcceleratedMonster";

        newMonster.transform.position = m_spawnPoint;
        newMonster.Init(new AcceleratedMovement(m_settings.TargetPosition, m_settings.Speed, m_settings.Acceleration));

        return newMonster;
    }
}

