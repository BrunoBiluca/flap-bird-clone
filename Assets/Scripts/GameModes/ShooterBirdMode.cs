using Assets.Scripts;
using Assets.UnityFoundation.CameraScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterBirdMode : GameplayController
{
    private ShooterBirdScript bird;

    public void BirdShoot()
    {
        bird.Shoot();
    }

    public override void FlapTheBird()
    {
        bird.FlapTheBird();
    }

    public override void PlayerDiedShowScore(BirdScript bird)
    {
        var medalIndex = GameController.Instance.EvaluatePlayerScore(score);
        StageUI.Instance.PlayerDied(score, medalIndex);
    }

    public override void SetScore()
    {
        score++;

        if(score % 5 == 0)
            InvokeOnIncreaseLevel();

        StageUI.Instance.SetScore(Score);
    }

    public override void StartGame()
    {
        bird = FindObjectOfType<ShooterBirdScript>();
        CameraFollower.Instance.Setup(bird);

        Time.timeScale = 1f;
        score = 0;

        bird.IBeleveICanFly();
        EnemySpawner.Instance.StartSpawning();
    }

    public void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            BirdShoot();
        }
    }
}
