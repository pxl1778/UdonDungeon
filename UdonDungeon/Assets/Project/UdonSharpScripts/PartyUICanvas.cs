using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class PartyUICanvas : UdonSharpBehaviour
{
    Canvas canvas;
    VRCPlayerApi localPlayer;

    [UdonSynced] string hpString;

    public DungeoneerManager dungeoneerManager;
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
                Dungeoneer[] dungeoneers = dungeoneerManager.dungeoneers;
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

    public override void OnDeserialization()
    {
        hpText.text = hpString;
    }

}