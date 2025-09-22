using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private MovementMode m_movementMode;

    [SerializeField] private LinearFactorySettings linearSettings;
    [SerializeField] private AcceleratedFactorySettings acceleratedSettings;
    [SerializeField] private CircularFactorySettings circularSettings;
    [SerializeField] private WaypointsFactorySettings waypointsSettings;

    [Space]
    [SerializeField] private float m_interval = 3;
    [SerializeField] private Monster m_monsterPrefab;
    [SerializeField] private FlyingShield m_flyingShieldPrefab;

    private ObjectPool<Monster> m_monsterPool;
    private IMonsterFactory m_monsterFactory;

    private WaitForSeconds m_spawnDelay;

    private void Awake()
    {
        m_monsterPool = new ObjectPool<Monster>(CreateEntity, OnGet, OnRelease, OnDestroyEntity);
        m_spawnDelay = new WaitForSeconds(m_interval);

        SetFactory();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnMonster();
            yield return m_spawnDelay;
        }
    }

    private void SpawnMonster()
    {
        Monster monster = m_monsterFactory.Create();
        if (m_flyingShieldPrefab != null)
            Instantiate(m_flyingShieldPrefab, monster.transform);
    }

    private void SetFactory()
    {
        switch (m_movementMode)
        {
            case MovementMode.Linear:
                {
                    m_monsterFactory = new LinearMonsterFactory(m_monsterPool, transform.position, linearSettings);
                    break;
                }
            case MovementMode.Accelerated:
                {
                    m_monsterFactory = new AcceleratedMonsterFactory(m_monsterPool, transform.position, acceleratedSettings);
                    break;
                }
            case MovementMode.Circular:
                {
                    m_monsterFactory = new CircularMonsterFactory(m_monsterPool, circularSettings);
                    break;
                }
            case MovementMode.Waypoints:
                {
                    m_monsterFactory = new WaypointsMonsterFactory(m_monsterPool, waypointsSettings);
                    break;
                }
        }
    }

    private Monster CreateEntity()
    {
        return Instantiate(m_monsterPrefab);
    }

    private void OnGet(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }

    private void OnRelease(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyEntity(Monster monster)
    {
        Destroy(monster.gameObject);
    }
}
