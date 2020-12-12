
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class TriggerRedo : UdonSharpBehaviour
{
    public Text textField;

    void Start()
    {
        
    }

    void Update() {
        textField.text = Networking.LocalPlayer.isMaster.ToString();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player) {
        textField.text = string.Format("{0} Entered", player.displayName);
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player) {
        textField.text = string.Format("{0} Exited", player.displayName);
    }
}
