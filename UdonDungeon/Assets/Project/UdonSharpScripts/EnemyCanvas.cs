
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class EnemyCanvas : UdonSharpBehaviour
{
    private Canvas canvas;

    void Start()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
    }

    void Update()
    {
        canvas.transform.LookAt(Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head), Vector3.up);
    }
}
