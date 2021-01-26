
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Dungeoneer : UdonSharpBehaviour
{
    [UdonSynced] public int playerID = -1;
    [UdonSynced] public string displayName;
    [UdonSynced] public float currentHP = 5;
    [UdonSynced] public float maxHP = 5;

    public void TakeDamage()
    {
        currentHP--;
    }
}
