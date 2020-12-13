
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Dungeoneer : UdonSharpBehaviour
{
    public int playerID = -1;
    public string displayName;
    public float currentHP = 100;
    public float maxHP = 100;
}
