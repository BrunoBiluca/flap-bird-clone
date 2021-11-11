using Assets.Scripts;
using Assets.UnityFoundation.CameraScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlapBirdMode : GameplayController
{

    [SerializeField] private float pipeMinDistance;
    [SerializeField] private float pipeDistance;
    [SerializeField] private float minOpeningSize;
    [SerializeField] private float openingSize;

    private List<BirdScript> birds;

    private int currentBirdToFlap;

    public void Start()
    {
        PipeCollector.Instance.Setup(
            pipeDistance,
            openingSize,
            restartPositions: true
        );
    }

    public override void FlapTheBird()
    {
        if(birds.Count == 0) return;

        if(currentBirdToFlap >= birds.Count)
            currentBirdToFlap = 0;

        birds[currentBirdToFlap++].FlapTheBird();
    }

    public override void SetScore()
    {
        score++;

        if(score % 4 == 0)
        {
            var newDistance = pipeDistance - 0.2f * score / 4;
            if(newDistance < pipeMinDistance)
                newDistance = pipeMinDistance;

            var newOpeningSize = openingSize - 0.2f * score / 4;
            if(newOpeningSize < minOpeningSize)
                newOpeningSize = minOpeningSize;

            PipeCollector.Instance.Setup(
                newDistance,
                newOpeningSize,
                restartPositions: false
            );
        }

        StageUI.Instance.SetScore(score);
    }

    public override void PlayerDiedShowScore(BirdScript bird)
    {
        birds.Remove(bird);
        if(birds.Count > 0)
        {
            CameraFollower.Instance.Setup(birds[0]);
            return;
        }

        var medalIndex = GameController.Instance.EvaluatePlayerScore(score);

        StageUI.Instance.PlayerDied(score, medalIndex);
    }

    public override void StartGame()
    {
        birds = FindObjectsOfType<BirdScript>().ToList();
        CameraFollower.Instance.Setup(birds[0]);

        Time.timeScale = 1f;
        score = 0;
        currentBirdToFlap = 0;

        foreach(var bird in birds)
        {
            bird.IBeleveICanFly();
        }
    }

}
