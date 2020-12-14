
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class EnemyHurtButton : UdonSharpBehaviour
{
    public Enemy enemy;
    public Text debugText;

    void Start()
    {
        
    }

    public override void Interact()
    {
        if(enemy != null)
        {
            //Networking.SetOwner(Networking.LocalPlayer, enemy.gameObject);
            //enemy.TakeDamage();
            if(Networking.IsOwner(Networking.LocalPlayer, enemy.gameObject))
            {
                enemy.TakeDamage();
            }
            else
            {
                enemy.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "TakeDamage");
            }
        }
    }
}
