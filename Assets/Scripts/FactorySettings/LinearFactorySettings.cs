using UnityEngine;

[System.Serializable]
public class LinearFactorySettings : BaseFactorySettings
{
    [SerializeField] private float m_speed = 10;
    [SerializeField] private Transform m_target;

    public float Speed => m_speed;
    public Vector3 TargetPosition => m_target.position;
}
