using UnityEngine.Pool;

public abstract class BaseMonsterFactory<T> : IMonsterFactory where T : BaseFactorySettings
{
    protected readonly ObjectPool<Monster> m_pool;

    public BaseMonsterFactory(ObjectPool<Monster> pool)
    {
        m_pool = pool;
    }

    public abstract Monster Create();
}