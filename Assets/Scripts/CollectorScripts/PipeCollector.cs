using Assets.UnityFoundation.Code.Common;
using UnityEngine;

public class PipeCollector : Singleton<PipeCollector> {

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float pipeMin = -7f;
    [SerializeField] private float pipeMax = 9f;

    private GameObject[] pipeHolders;
    private float distance = 10f;
    private float lastPipeX;
    private float openingSize = 5f;

    public PipeCollector Setup(
        float distance,
        float openingSize,
        bool restartPositions
    ) {
        this.distance = distance;
        this.openingSize = openingSize;

        if(restartPositions)
            InitializeObjects();

        return this;
    }

    private void InitializeObjects() {
        pipeHolders = GameObject.FindGameObjectsWithTag("PipeHolder");

        lastPipeX = spawnPoint.position.x;
        for(int i = 0; i < pipeHolders.Length; i++) {
            UpdatePipeHolderPosition(pipeHolders[i], lastPipeX + distance * i);
        }

        lastPipeX = pipeHolders[pipeHolders.Length - 1].transform.position.x;
    }

    void OnTriggerEnter2D(Collider2D target) {
        if(target.tag == "PipeHolder") {
            lastPipeX += distance;
            UpdatePipeHolderPosition(target.gameObject, lastPipeX);
        }
    }

    private void UpdatePipeHolderPosition(GameObject pipeHolder, float posX) {
        pipeHolder.transform.position = new Vector3(
            posX,
            pipeHolder.transform.position.y,
            0f
        );

        var meanPipeHolder = (pipeMax + pipeMin) / 2f;

        var upperPipe = pipeHolder.transform.Find("upper_pipe");
        upperPipe.localPosition = new Vector3(
            0f,
            Random.Range(
                meanPipeHolder + (openingSize / 2f),
                pipeMax - 6 / openingSize
            ) + 3f,
            0f
        );

        var underPipe = pipeHolder.transform.Find("under_pipe");
        underPipe.localPosition = new Vector3(
            0f,
            Random.Range(
                pipeMin + 6 / openingSize, 
                meanPipeHolder - (openingSize / 2f)
            ) - 3f,
            0f
        );
    }

}
