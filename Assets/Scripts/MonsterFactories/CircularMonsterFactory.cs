using UnityEngine;
using UnityEngine.Pool;

public class CircularMonsterFactory : BaseMonsterFactory<CircularFactorySettings>
{
    private readonly CircularFactorySettings m_settings;

    public CircularMonsterFactory(ObjectPool<Monster> pool, CircularFactorySettings settings) : base(pool)
    {
        m_settings = settings;
    }

    public override Monster Create()
    {
        Monster newMonster = m_pool.Get();
        newMonster.name = "CircularMonster";
        
        Vector3 offset = Vector3.right * m_settings.Radius;

        newMonster.transform.position = m_settings.Tower + offset;
        newMonster.Init(new CircularMovement(m_settings.Tower, m_settings.Radius, m_settings.Speed));

        return newMonster;
    }
}
