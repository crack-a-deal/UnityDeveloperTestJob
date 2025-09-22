using UnityEngine;

[System.Serializable]
public class CircularFactorySettings : BaseFactorySettings
{
    [SerializeField] private float m_speed = 10;
    [SerializeField] private float m_radius = 5;
    [SerializeField] private GameObject m_tower;

    public float Speed => m_speed;
    public Vector3 Tower => m_tower.transform.position;
    public float Radius => m_radius;
}
