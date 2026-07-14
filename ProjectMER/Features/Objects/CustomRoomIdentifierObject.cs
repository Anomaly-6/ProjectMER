using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace ProjectMER.Features.Objects;

public class CustomRoomIdentifierObject : MonoBehaviour
{
    public static readonly List<CustomRoomIdentifierObject> Instances = [];

    public string RoomName = "Unnamed";

    public bool Contains(Player player) => player != null && _playersInside.Contains(player);

    public static bool IsInCustomRoom(Player player, string roomName) => 
        player != null && !string.IsNullOrWhiteSpace(roomName) && Instances.Any(room => room.IsNamed(roomName) && room.Contains(player));

    public static CustomRoomIdentifierObject? GetCustomRoom(Player player) =>
        player == null ? null : Instances.FirstOrDefault(room => room.Contains(player));

    public static string? GetCustomRoomName(Player player) =>
        GetCustomRoom(player)?.RoomName;

    public static List<CustomRoomIdentifierObject> GetCustomRooms(Player player) => 
        player == null ? [] : [.. Instances.Where(room => room.Contains(player))];

    public static List<CustomRoomIdentifierObject> GetCustomRooms(string roomName) => 
        string.IsNullOrWhiteSpace(roomName) ? [] : [.. Instances.Where(room => room.IsNamed(roomName))];

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

    private bool IsNamed(string roomName) => string.Equals(RoomName, roomName, StringComparison.OrdinalIgnoreCase);

    private readonly HashSet<Player> _playersInside = [];
}
