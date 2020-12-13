
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PartyUICanvas : UdonSharpBehaviour
{
    const float MAX_HP = 100;

    Canvas canvas;
    VRCPlayerApi localPlayer;

    //only the owner will touch this
    VRCPlayerApi[] players;

    [UdonSynced(UdonSyncMode.None)]
    string hpString;

    public Text debugText;
    public Text hpText;

    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        localPlayer = Networking.LocalPlayer;
    }

    void Update()
    {
        if (localPlayer != null && canvas != null)
        {
            canvas.transform.LookAt(localPlayer.GetBonePosition(HumanBodyBones.Head), Vector3.up);

            Vector3 playerPos = localPlayer.GetBonePosition(HumanBodyBones.Hips);
            Quaternion playerRotation = localPlayer.GetRotation();
            var forward = playerRotation * Vector3.forward;
            canvas.transform.position = playerPos + forward;

            if (Networking.GetOwner(gameObject).playerId == localPlayer.playerId)
            {
                //hpString = MAX_HP + " / " + MAX_HP;
                if (players != null)
                {
                    hpString = "";
                    foreach (VRCPlayerApi player in players)
                    {
                        hpString = hpString + player.displayName + ": " + player.CombatGetCurrentHitpoints() + "/" + MAX_HP + "\n";
                    }
                    hpText.text = hpString;
                }
            }
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.GetOwner(gameObject).playerId == Networking.LocalPlayer.playerId)
        {
            if (players == null)
            {
                players = new VRCPlayerApi[1];
                players[0] = player;
            }
            else
            {
                VRCPlayerApi[] tempPlayers = new VRCPlayerApi[players.Length + 1];
                for (int i = 0; i < players.Length; i++)
                {
                    tempPlayers[i] = players[i];
                }
                tempPlayers[players.Length - 1] = player;
                players = tempPlayers;
            }
        }
    }

    public override void OnDeserialization()
    {
        hpText.text = hpString;
    }
}