
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class Enemy : UdonSharpBehaviour
{
    [SerializeField]
    public int maxHealth = 10;
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private float attackCooldown = 3;
    [SerializeField]
    private float maxAttackCooldown = 3;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float personalSpace = 3;
    [SerializeField]
    private float minPlayerDistance = 1000;
    [SerializeField]
    public Text targetText;
    [SerializeField]
    public Text debugText;
    private bool playersNearby = false;
    private Dungeoneer targetDungeoneer;
    public DungeoneerManager dungeoneerManager;
    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    [UdonSynced(UdonSyncMode.None)]
    public int targetID = -1;
    [UdonSynced(UdonSyncMode.None)]
    public int health = 10;

    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        rb = this.GetComponent<Rigidbody>();
        health = maxHealth;
        debugText.text += "\nStart";
    }

    void Update()
    {
        if(targetDungeoneer == null)
        {//searching for target
            if(!playersNearby) { return; }
            //debugText.text += "\nPlayersNearby == true";
            VRCPlayerApi localPlayer = Networking.LocalPlayer;
            if (Vector3.Distance(localPlayer.GetPosition(), gameObject.transform.position) < minPlayerDistance)
            {
                debugText.text += "\nClose Enough to Raycast";
                Vector3 direction = (gameObject.transform.position - localPlayer.GetPosition()).normalized;
                RaycastHit hit;
                Debug.DrawRay(localPlayer.GetPosition(), direction, Color.blue, 0.5f);
                if (Physics.Raycast(localPlayer.GetPosition(), direction, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        Networking.SetOwner(localPlayer, this.gameObject);
                        debugText.text += "\nSet new Target";
                        targetDungeoneer = dungeoneerManager.getDungeoneerForID(localPlayer.playerId);
                        targetID = localPlayer.playerId;
                        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f);
                        targetText.text = localPlayer.displayName;
                        //debugText.text += "\nGrabbed Dungeoneer: " + targetDungeoneer.displayName;
                        //SendCustomEvent("_onDeserialization");
                        //this.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "UpdateRanking");
                    }
                }
                else
                {
                    debugText.text += "\nTarget Not hit";
                }
            }
        }
        else
        {//chasing target
            if(targetDungeoneer.playerID == Networking.LocalPlayer.playerId)
            {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0)
                {
                    attackCooldown = maxAttackCooldown;
                    //Target.CombatSetCurrentHitpoints(Target.CombatGetCurrentHitpoints() - damage);
                    targetDungeoneer.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "TakeDamage");
                }
                float distance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), gameObject.transform.position);
                this.gameObject.transform.LookAt(Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips));
                Quaternion rot = gameObject.transform.rotation;
                rot.x = 0;
                rot.z = 0;
                this.gameObject.transform.rotation = rot;
                //this.gameObject.transform.rotation
                if (distance > personalSpace)
                {
                    Vector3 direction = (Networking.LocalPlayer.GetPosition() - gameObject.transform.position).normalized;
                    //Vector3 goalPosition = gameObject.transform.position + (direction * speed * Time.deltaTime);
                    //float goalDistance = Vector3.Distance(goalPosition, gameObject.transform.position);
                    //this.gameObject.transform.position = goalPosition;
                    //goalPosition -= direction * (personalSpace - goalDistance);
                    rb.velocity = (direction * speed * Time.deltaTime * Mathf.Clamp(distance - personalSpace, 0, 1.0f));
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        debugText.text += "\nOnPlayerTriggerEnter";
        playersNearby = true;
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        debugText.text += "\nOnPlayerTriggerExit";
        if (targetDungeoneer != null && player.playerId == targetDungeoneer.playerID)
        {
            ResetAggro();
        }
    }

    private void ResetAggro()
    {
        debugText.text += "\nResetAggro";
        playersNearby = false;
        meshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f);
        targetDungeoneer = null;
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        targetID = -1;
        targetText.text = "none";
        rb.velocity = Vector3.zero;
    }

    public override void OnDeserialization()
    {
        //debugText.text += "\nOn Deserialization Called.";
        if(targetID != -1)
        {
            //debugText.text += "\nOn Deserialization targetID != -1";
            meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f);
            targetDungeoneer = dungeoneerManager.getDungeoneerForID(targetID);
            targetText.text = targetDungeoneer.displayName;
        }
        else
        {
            //debugText.text += "\nOn Deserialization  targetID == -1";
            meshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f);
            targetText.text = "none";
            targetDungeoneer = null;
        }
        //debugText.text += "\nOnDeserialization: health = " + health;
        //healthText.text = health + "/" + maxHealth;
    }

    public void TakeDamage()
    {
        health = health - 1;
        debugText.text += "\nTakeDamage: health = " + health;
        //healthText.text = health + "/" + maxHealth;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    //public override void OnPlayerJoined(VRCPlayerApi player) {

    //}

    //public override void OnPlayerLeft(VRCPlayerApi player)
    //{
           
    //}
}
