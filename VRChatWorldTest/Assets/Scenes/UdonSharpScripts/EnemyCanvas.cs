
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class EnemyCanvas : UdonSharpBehaviour
{
    public Canvas canvas;

    void Start()
    {
        
    }

    void Update()
    {
        canvas.transform.LookAt(Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head), Vector3.up);
    }
}
