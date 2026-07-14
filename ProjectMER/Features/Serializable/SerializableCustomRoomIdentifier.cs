using AdminToys;
using LabApi.Features.Wrappers;
using Mirror;
using ProjectMER.Features.Extensions;
using ProjectMER.Features.Interfaces;
using ProjectMER.Features.Objects;
using UnityEngine;
using PrimitiveObjectToy = AdminToys.PrimitiveObjectToy;

namespace ProjectMER.Features.Serializable;

public class SerializableCustomRoomIdentifier : SerializableObject, IIndicatorDefinition
{
    public string RoomName { get; set; } = "Unnamed";

    public override GameObject SpawnOrUpdateObject(Room? room = null, GameObject? instance = null)
    {
        GameObject gameObject = instance ?? new GameObject("CustomRoomIdentifier");

        Vector3 position = room.GetAbsolutePosition(Position);
        Quaternion rotation = room.GetAbsoluteRotation(Rotation);

        _prevIndex = Index;

        gameObject.transform.SetPositionAndRotation(position, rotation);
        gameObject.transform.localScale = Scale;
        gameObject.layer = LayerMask.NameToLayer("InvisibleCollider");

        CustomRoomIdentifierObject roomIdentifier = gameObject.GetComponent<CustomRoomIdentifierObject>() ?? gameObject.AddComponent<CustomRoomIdentifierObject>();
        roomIdentifier.RoomName = RoomName;

        return gameObject;
    }

    public GameObject SpawnOrUpdateIndicator(Room room, GameObject? instance = null)
    {
        PrimitiveObjectToy root;
        Vector3 position = room.GetAbsolutePosition(Position);
        Quaternion rotation = room.GetAbsoluteRotation(Rotation);

        if (instance == null)
        {
            root = UnityEngine.Object.Instantiate(PrefabManager.PrimitiveObject);
            root.NetworkPrimitiveFlags = PrimitiveFlags.Visible;
            root.NetworkMaterialColor = new Color(0f, 0f, 1f, 1f);
            NetworkServer.Spawn(root.gameObject);
        }
        else
        {
            root = instance.GetComponent<PrimitiveObjectToy>();
        }

        root.NetworkPrimitiveType = PrimitiveType.Cube;
        root.transform.SetPositionAndRotation(position, rotation);
        root.transform.localScale = Scale;

        return root.gameObject;
    }
}
