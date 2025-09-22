using UnityEngine;

public class CannonTower : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private ShootingMode m_mode = ShootingMode.Direct;
    [SerializeField] private Transform m_base;
    [SerializeField] private Transform m_barrel;
    [SerializeField, Range(1, 90)] private float m_barrelPitchLimit = 45f;
    [SerializeField] private float m_range = 4f;
    [SerializeField] private float m_rotationSpeed;

    [Header("Shooting")]
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private float m_shootInterval = 0.5f;
    [SerializeField] private LayerMask m_monsterLayer;
    [SerializeField, Range(0, 5)] private float aimTolerance = 1f;

    [SerializeField] private CannonProjectile m_projectilePrefab;
    [Space]


    [Header("Ballistic settings")]
    [SerializeField] float m_ballisticMaxTime = 5f;
    [SerializeField] float m_ballisticTimeStep = 0.02f;


    private Monster m_targetMonster;
    private float m_lastShotTime = -0.5f;

    private ITargetingStrategy m_targetingStrategy;

    private Vector3 resultVelocity;
    private Vector3 m_interceptPoint;

    private enum TowerState
    {
        Finding,
        Tracking,
        Shooting,
    }

    private TowerState m_currentState = TowerState.Finding;

    private void Awake()
    {
        m_targetingStrategy = new ClosestTargetStrategy(transform.position, m_range, m_monsterLayer);
    }

    private void Update()
    {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        switch (m_currentState)
        {
            case TowerState.Finding:
                {
                    FindingState();
                    break;
                }
            case TowerState.Tracking:
                {
                    TrackingState();
                    break;
                }
            case TowerState.Shooting:
                {
                    ShootingState();
                    break;
                }
        }
    }

    private void FindingState()
    {
        if (m_targetMonster != null)
        {
            if (Vector3.Distance(transform.position, m_targetMonster.transform.position) >= m_range)
            {
                m_targetMonster = null;
            }
        }

        if (m_targetMonster == null)
        {
            m_targetMonster = m_targetingStrategy.SelectTarget();
        }

        if (m_targetMonster != null && TryGetInterceptPosition())
        {
            m_currentState = TowerState.Tracking;
        }
    }


    private void TrackingState()
    {
        RotateBaseAndBarrel();

        m_currentState = TowerState.Finding;

        if (IsAimedAtTarget(resultVelocity))
        {
            m_currentState = TowerState.Shooting;
        }
    }

    private void ShootingState()
    {
        if (m_lastShotTime + m_shootInterval > Time.time)
        {
            m_currentState = TowerState.Finding;
            return;
        }

        CannonProjectile cannonProjectile = Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
        cannonProjectile.SetVelocity(resultVelocity);
        if (m_mode == ShootingMode.Balistic)
        {
            cannonProjectile.SetGravity(true);
        }

        m_lastShotTime = Time.time;
    }

    private void RotateBaseAndBarrel()
    {
        // Tower base rotation
        Vector3 flatDir = new Vector3(resultVelocity.x, 0f, resultVelocity.z);
        if (flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion desiredRot = Quaternion.LookRotation(flatDir.normalized, Vector3.up);
            m_base.rotation = Quaternion.RotateTowards(m_base.rotation, desiredRot, m_rotationSpeed * Time.deltaTime);
        }


        // Tower barrel rotation
        Vector3 localDir = m_base.InverseTransformDirection(resultVelocity);
        float pitchAngle = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        pitchAngle = Mathf.Clamp(pitchAngle, -m_barrelPitchLimit, m_barrelPitchLimit);

        Quaternion pitchRot = Quaternion.Euler(pitchAngle, 0f, 0f);

        m_barrel.localRotation = Quaternion.RotateTowards(m_barrel.localRotation, pitchRot, m_rotationSpeed * Time.deltaTime);
    }

    private bool IsAimedAtTarget(Vector3 interceptPosition)
    {
        float aimAngle = Quaternion.Angle(m_shootPoint.rotation, Quaternion.LookRotation(resultVelocity.normalized, Vector3.up));

        return aimAngle < aimTolerance;
    }

    private bool TryGetInterceptPosition()
    {
        if (m_mode == ShootingMode.Direct)
        {
            return CalculateIntercept(m_projectilePrefab.Speed, m_targetMonster, Vector3.zero, out resultVelocity);
        }
        else if (m_mode == ShootingMode.Balistic)
        {
            return CalculateIntercept(m_projectilePrefab.Speed, m_targetMonster, Physics.gravity, out resultVelocity);
        }

        return false;
    }

    private bool CalculateIntercept(float projectileSpeed, Monster target, Vector3 gravity, out Vector3 resultVelocity)
    {
        resultVelocity = Vector3.zero;

        float bestError = float.MaxValue;
        bool solutionFound = false;

        for (float t = 0.05f; t <= m_ballisticMaxTime; t += m_ballisticTimeStep)
        {
            Vector3 predictedTarget = target.GetPredictedPosition(t);

            if (!IsReachable(m_shootPoint.position, predictedTarget, projectileSpeed, t, m_range))
                continue;

            Vector3 requiredVelocity = CalculateRequiredVelocity(m_shootPoint.position, predictedTarget, gravity, t);

            float speedError = Mathf.Abs(requiredVelocity.magnitude - projectileSpeed);

            if (speedError < bestError)
            {
                bestError = speedError;
                m_interceptPoint = predictedTarget;
                resultVelocity = requiredVelocity.normalized * projectileSpeed;
                solutionFound = true;
            }
        }

        return solutionFound;
    }

    private bool IsReachable(Vector3 shooterPos, Vector3 targetPos, float projectileSpeed, float time, float attackRange)
    {
        Vector3 displacement = targetPos - shooterPos;
        float distance = displacement.magnitude;

        return distance <= projectileSpeed * time && distance <= attackRange;
    }

    private Vector3 CalculateRequiredVelocity(Vector3 shooterPos, Vector3 targetPos, Vector3 gravity, float time)
    {
        return (targetPos - shooterPos - 0.5f * gravity * time * time) / time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, m_range);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_shootPoint.position, m_shootPoint.position + m_shootPoint.forward * 20);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_interceptPoint, 0.5f);

        Gizmos.color = Color.red;
        if (m_targetMonster != null)
        {
            Gizmos.DrawSphere(m_targetMonster.transform.position, 1f);
        }
    }
}
