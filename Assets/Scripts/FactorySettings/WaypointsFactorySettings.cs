using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WaypointsFactorySettings : BaseFactorySettings
{
    [SerializeField] private float m_speed = 10;
    [SerializeField] private List<GameObject> m_waypoints;

    public float Speed => m_speed;
    public List<Vector3> Waypoints => m_waypoints.Select(w => w.transform.position).ToList();
}