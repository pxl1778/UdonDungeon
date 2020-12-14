
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class TestSword : UdonSharpBehaviour
{
    public Text debugText;
    private Enemy targetEnemy;
    private Vector3 enterPosition;
    private Vector3 lastFramePosition;
    private Vector3 swipeDirection;
    private float timeSinceEntered;
    private float cooldownTime;
    [SerializeField]
    private float attackCooldown = 0.5f;
    [SerializeField]
    private float maxAngleDifference = 50.0f;
    [SerializeField]
    private float maxSwipeDuration = 0.5f;
    void Start()
    {
        GameObject.Find("DungeonManager");
    }

    public override void OnPickup()
    {

    }

    private void Update()
    {
        if(targetEnemy != null)
        {
            timeSinceEntered += Time.deltaTime;
        }
        if(cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        lastFramePosition = this.gameObject.transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (other.isTrigger == false && enemy != null)
        {
            targetEnemy = enemy;
            timeSinceEntered = 0;
            enterPosition = this.gameObject.transform.position;
            swipeDirection = (enterPosition - lastFramePosition).normalized;
            if(enterPosition == lastFramePosition)
            {
                debugText.text += "\nontriggerenter: enterposition == lastframeposition";
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //debugText.text += "\nontriggerenter: " + other.gameObject.name;
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (other.isTrigger == false && enemy != null && enemy == targetEnemy)
        {
            Vector3 exitPosition = this.gameObject.transform.position;
            Vector3 exitDirection = (exitPosition - enterPosition).normalized;
            //debugText.text += "\nontriggerenter: correct enemy exit angle: " + Vector3.Angle(exitDirection, swipeDirection);
            if (Vector3.Angle(exitDirection, swipeDirection) <= maxAngleDifference && timeSinceEntered <= maxSwipeDuration && attackCooldown <= 0)
            {
                debugText.text += "\nHit Enemy: " + other.gameObject.name;
                if (Networking.IsOwner(Networking.LocalPlayer, enemy.gameObject))
                {
                    enemy.TakeDamage();
                }
                else
                {
                    enemy.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "TakeDamage");
                }
                targetEnemy = null;
                cooldownTime = attackCooldown;
            }
        }
    }
}
