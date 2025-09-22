using UnityEngine.Pool;

public class WaypointsMonsterFactory : BaseMonsterFactory<WaypointsFactorySettings>
{
    private readonly WaypointsFactorySettings m_settings;

    public WaypointsMonsterFactory(ObjectPool<Monster> pool, WaypointsFactorySettings settings) : base(pool)
    {
        m_settings = settings;
    }

    public override Monster Create()
    {
        Monster newMonster = m_pool.Get();
        newMonster.name = "WaypointsMonster";
        newMonster.transform.position = m_settings.Waypoints[0];
        newMonster.Init(new WaypointMovement(m_settings.Waypoints, m_settings.Speed));

        return newMonster;
    }
}
