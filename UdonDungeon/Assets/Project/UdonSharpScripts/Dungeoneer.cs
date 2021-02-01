
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Dungeoneer : UdonSharpBehaviour
{
    [UdonSynced] public int playerID = -1;
    [UdonSynced] public string displayName;
    [UdonSynced] public float currentHP = 5;
    [UdonSynced] public float maxHP = 5;
    [UdonSynced] public float deathTimer = 0;
    [UdonSynced] public bool isDead = false;
    [UdonSynced] public bool needsRespawn = false;

    void Update()
    {
        if (isDead && Networking.IsOwner(gameObject))
        {
            deathTimer -= Time.deltaTime;
        }

        VRCPlayerApi player = Networking.LocalPlayer;
        if (player == null || player.playerId != playerID)
        {
            return;
        }

        if (needsRespawn)
        {
            Transform target = GameObject.Find("VRCWorld").gameObject.transform;
            player.TeleportTo(target.position, target.rotation);
            this.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "EndRespawn");
            return;
        }

        if (isDead)
        {
            player.Immobilize(true);
            if (deathTimer <= 0)
            {
                this.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "BeginRespawn");
            }
        } else
        {
            player.Immobilize(false);
        }
    }

    public void TakeDamage()
    {
        currentHP--;
        if (currentHP <= 0)
        {
            this.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "BeginDeath");
        }
    }

    public void BeginDeath()
    {
        isDead = true;
        deathTimer = 10;
    }

    public void BeginRespawn()
    {
        deathTimer = 0;
        isDead = false;
        currentHP = maxHP;
        needsRespawn = true;
    }

    public void EndRespawn()
    {
        needsRespawn = false;
    }
}
