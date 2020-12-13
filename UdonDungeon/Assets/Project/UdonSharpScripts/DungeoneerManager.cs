
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DungeoneerManager : UdonSharpBehaviour
{
    public Dungeoneer[] dungeoneers;
    [UdonSynced] int playerCount;

    void Start()
    {
        dungeoneers = gameObject.GetComponentsInChildren<Dungeoneer>();
    }

    public Dungeoneer getDungeoneerForID(int ID)
    {
        if (dungeoneers != null)
        {
            for (int i = 0; i < dungeoneers.Length; i++)
            {
                if (dungeoneers[i].playerID == ID)
                {
                    return dungeoneers[i];
                }
            }
        }

        return null;
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            for (int i = 0; i < dungeoneers.Length; i++)
            {
                if (dungeoneers[i].playerID == -1)
                {
                    dungeoneers[i].displayName = player.displayName;
                    dungeoneers[i].playerID = player.playerId;
                    playerCount++;
                    break;
                }
            }
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        {
            for (int i = 0; i < dungeoneers.Length; i++)
            {
                if (dungeoneers[i].playerID == player.playerId)
                {
                    dungeoneers[i].displayName = null;
                    dungeoneers[i].playerID = -1;
                    playerCount--;
                    break;
                }
            }
        }
    }
}
