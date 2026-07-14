using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace ProjectMER.Features.Objects;

public class CustomZoneIdentifierObject : MonoBehaviour
{
    public static readonly List<CustomZoneIdentifierObject> Instances = [];

    public string ZoneName = "Unnamed";

    public bool Contains(Player player) =>
        player != null && _playersInside.Contains(player);

    public static bool IsInCustomZone(Player player, string zoneName) =>
        player != null && !string.IsNullOrWhiteSpace(zoneName) && Instances.Any(zone => zone.IsNamed(zoneName) && zone.Contains(player));

    public static CustomZoneIdentifierObject? GetCustomZone(Player player) => 
        player == null ? null : Instances.FirstOrDefault(zone => zone.Contains(player));

    public static string? GetCustomZoneName(Player player) => 
        GetCustomZone(player)?.ZoneName;

    public static List<CustomZoneIdentifierObject> GetCustomZones(Player player) => 
        player == null ? [] : [.. Instances.Where(zone => zone.Contains(player))];

    public static List<CustomZoneIdentifierObject> GetCustomZones(string zoneName) => 
        string.IsNullOrWhiteSpace(zoneName) ? [] : [.. Instances.Where(zone => zone.IsNamed(zoneName))];

    private void Awake() => Instances.Add(this);

    public bool Contains(Vector3 worldPosition)
    {
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);

        return Mathf.Abs(localPosition.x) <= 0.5f && Mathf.Abs(localPosition.y) <= 0.5f && Mathf.Abs(localPosition.z) <= 0.5f;
    }

    private void OnDestroy()
    {
        Instances.Remove(this);
        _playersInside.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Player.Get(other.gameObject) is Player player)
            _playersInside.Add(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (Player.Get(other.gameObject) is Player player)
            _playersInside.Remove(player);
    }

    private bool IsNamed(string zoneName) => string.Equals(ZoneName, zoneName, StringComparison.OrdinalIgnoreCase);

    private readonly HashSet<Player> _playersInside = [];
}
