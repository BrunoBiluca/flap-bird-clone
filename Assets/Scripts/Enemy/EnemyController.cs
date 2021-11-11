using Assets.Scripts;
using Assets.UnityFoundation.Code.MonoBehaviourUtils;
using Assets.UnityFoundation.Systems.HealthSystem;

public class EnemyController : CustomDestroyMonoBehaviour
{

    protected override void OnAwake()
    {
        GetComponent<HealthSystem>().OnDied += (sender, args) => {
            GameplayController.Instance.SetScore();
        };
    }

    public void OnEnable()
    {
        destroyBehaviour.Destroy(10f);
    }
}
