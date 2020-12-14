
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class EnemyCanvas : UdonSharpBehaviour
{
    private Canvas canvas;
    private Enemy enemy;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text targetText;
    [SerializeField]
    private Image healthForegroundPanel;
    [SerializeField]
    private Image healthBackgroundPanel;

    void Start()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
        enemy = this.gameObject.GetComponentInParent<Enemy>();
        if(enemy != null)
        {
            healthText.text = enemy.health + "/" + enemy.health;
        }
    }

    void Update()
    {
        canvas.transform.LookAt(Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head), Vector3.up);
        if(enemy != null)
        {
            healthText.text = enemy.health + "/" + enemy.maxHealth;
            healthForegroundPanel.rectTransform.sizeDelta = new Vector2(healthBackgroundPanel.rectTransform.sizeDelta.x * ((float)enemy.health / (float)enemy.maxHealth), healthForegroundPanel.rectTransform.sizeDelta.y);
        }
    }
}
