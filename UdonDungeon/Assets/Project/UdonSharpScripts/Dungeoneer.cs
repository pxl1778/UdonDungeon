
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

    void Update()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        if (player == null)
        {
            return;
        }

        if (isDead)
        {
            player.Immobilize(true);

            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                deathTimer = 0;
                isDead = false;
                currentHP = maxHP;
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
}
