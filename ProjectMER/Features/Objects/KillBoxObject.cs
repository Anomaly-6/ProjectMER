using LabApi.Features.Wrappers;
using UnityEngine;

namespace ProjectMER.Features.Objects;

public class KillBoxObject : MonoBehaviour
{
    public string DeathReason = string.Empty;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Player? player = Player.Get(other.gameObject);

        if (player is { IsAlive: true })
            player.Kill(DeathReason);
    }
}
