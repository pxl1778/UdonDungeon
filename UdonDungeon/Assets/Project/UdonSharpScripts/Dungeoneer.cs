
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Dungeoneer : UdonSharpBehaviour
{
    [UdonSynced] public int playerID = -1;
    [UdonSynced] public string displayName;
    [UdonSynced] public float currentHP = 100;
    [UdonSynced] public float maxHP = 100;

    public void TakeDamage()
    {
        currentHP--;
    }
}
