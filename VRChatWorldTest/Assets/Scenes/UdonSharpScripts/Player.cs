
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Player : UdonSharpBehaviour
{
    private int playerID;

    void Start()
    {
        
    }

    public Vector3 GetPosition()
    {
        return Networking.LocalPlayer.GetPosition();
    }

    public override void Interact() {

    }
}
