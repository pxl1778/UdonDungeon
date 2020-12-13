using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class PartyUICanvas : UdonSharpBehaviour
{
    Canvas canvas;
    VRCPlayerApi localPlayer;

    //only the owner will touch these
    [UdonSynced] int playerCount;
    [UdonSynced] string hpString;

    public Dungeoneer[] dungeoneers;
    public GameObject dungeoneerManager;
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
            canvas.transform.position = playerPos + forward + new Vector3(0, -0.25f, 0);

            if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
            {
                hpString = "";
                for (int i = 0; i < dungeoneers.Length; i++)
                {
                    if (dungeoneers[i].playerID != -1)
                    {
                        hpString = hpString + i + " " + dungeoneers[i].displayName + ": " + dungeoneers[i].currentHP + "/" + dungeoneers[i].maxHP + "\n";
                    }
                }
                hpText.text = hpString;
            }
        }
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

    public override void OnDeserialization()
    {
        hpText.text = hpString;
    }

    public Dungeoneer getDungeoneerForID(int ID)
    {
        for (int i = 0; i < dungeoneers.Length; i++)
        {
            if (dungeoneers[i].playerID == ID)
            {
                return dungeoneers[i];
            }
        }

        return null;
    }
}