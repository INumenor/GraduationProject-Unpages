using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenAreaDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (IsLocalPlayer(other))
        {
            GameService.Instance.playerAction.isOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsLocalPlayer(other))
        {
            GameService.Instance.playerAction.isOpen = false;
        }
    }

    // Triggers also fire for the other player's character, which must not change our own state.
    private bool IsLocalPlayer(Collider other)
    {
        if (!other.CompareTag("Player") || GameService.Instance == null || GameService.Instance.playerAction == null) return false;
        Fusion.NetworkObject networkObject = other.GetComponentInParent<Fusion.NetworkObject>();
        return networkObject != null && networkObject.HasStateAuthority;
    }
}
