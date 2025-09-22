using UnityEngine;

[System.Serializable]
public class AcceleratedFactorySettings : BaseFactorySettings
{
    [SerializeField] private float m_speed = 10;
    [SerializeField] private float m_acceleration = 3;
    [SerializeField] private Transform m_target;

    public float Speed => m_speed;
    public float Acceleration => m_acceleration;
    public Vector3 TargetPosition => m_target.position;
}