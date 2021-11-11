using Assets.Scripts;
using Assets.UnityFoundation.CameraScripts;
using Assets.UnityFoundation.Code.Common;
using Assets.UnityFoundation.Code.Spline;
using Assets.UnityFoundation.Code.TimeUtils;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.Systems.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private ObjectPooling enemyPooling;
    [SerializeField] private MultipleObjectPooling enemyMovementPooling;

    [SerializeField] private float cooldownTime = 1f;
    private Timer cooldown;

    [StringInList(
        new string[] {
            "fixed",
            "up_down_arc",
            "zig_zag",
            "circle"
        }
    )]
    public List<string> enemyMovementTypes;

    protected override void OnAwake()
    {
        cooldown = new Timer(
            cooldownTime,
            InstantiateEnemy
        );
    }

    public void Start()
    {
        enemyPooling.InstantiateObjects();

        GameplayController.Instance.OnIncreaseLevel += (score) => {
            var newCooldownTime = cooldownTime - 0.2f * score / 5;
            cooldown.SetAmount(newCooldownTime).Start();
        };
    }

    public void StartSpawning()
    {
        cooldown.Start();
    }

    private void InstantiateEnemy()
    {
        var minPos = CameraUtils.GetMousePosition2D(new Vector2(0, 0));
        var maxPos = CameraUtils.GetMousePosition2D(
            new Vector2(Screen.width, Screen.height)
        );
        var posY = Random.Range(minPos.y, maxPos.y);

        var spawnPosition = new Vector3(maxPos.x, posY);

        var obj = enemyPooling.GetAvailableObject();
        if(!obj.IsPresent)
            return;

        var enemyObj = obj.Get();

        enemyObj.transform.position = spawnPosition;
        enemyObj.GetComponent<HealthSystem>().HealFull();

        var movementIdx = Random.Range(0, enemyMovementTypes.Count);

        if(movementIdx != 0)
        {
            var mov = enemyMovementTypes[movementIdx];
            enemyMovementPooling.GetAvailableObject(mov)
                .Some(obj => {
                    obj.transform.position = spawnPosition;
                    obj.Activate<SplineMonoBehaviour>(
                        sObj => enemyObj
                            .GetComponent<SplineFollower>()
                            .Setup(
                                sObj,
                                mov == "circle"
                                    ? SplineFollowBehaviour.loop
                                    : SplineFollowBehaviour.backAndForth
                            )
                    );
                });
        }

        enemyObj.Activate();
    }
}
