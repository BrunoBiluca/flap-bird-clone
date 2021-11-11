using Assets.Scripts;
using Assets.UnityFoundation.Code.Common;
using UnityEngine.UI;

public class ShooterModeUI : Singleton<ShooterModeUI>
{
    private Button shootButton;

    public void Awake()
    {
        shootButton = transform.Find("shoot_button").GetComponent<Button>();
        shootButton.onClick.AddListener(() => {
            ((ShooterBirdMode)GameplayController.Instance).BirdShoot();
        });
    }
}
