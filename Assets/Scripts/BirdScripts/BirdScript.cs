using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.UnityFoundation.CameraScripts;

public class BirdScript : MonoBehaviour, IFollowable
{

    [SerializeField] private float forwardSpeed = 3f;
    [SerializeField] private float bounceSpeed = 4f;

    [SerializeField]
    private AudioClip flapClip, pointClip, dieClip;

    private bool didFlap;
    public bool isAlive;

    protected Rigidbody2D myRigidBody;
    protected Animator anim;
    protected AudioSource audioSource;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

        OnAwake();
    }

    protected virtual void OnAwake() { }

    void FixedUpdate()
    {
        if(!isAlive) return;

        Vector3 temp = transform.position;
        temp.x += forwardSpeed * Time.deltaTime;
        transform.position = temp;

        if(didFlap)
        {
            didFlap = false;
            myRigidBody.velocity = new Vector2(0, bounceSpeed);
            anim.SetTrigger("Flap");
            audioSource.PlayOneShot(flapClip);
        }

        if(myRigidBody.velocity.y >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            float angle = 0;
            angle = Mathf.Lerp(0, -90, -myRigidBody.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void IBeleveICanFly()
    {
        isAlive = true;
        myRigidBody.constraints = RigidbodyConstraints2D.None;
    }

    public void FlapTheBird()
    {
        didFlap = true;
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if(
            target.gameObject.tag != "Ground"
            && target.gameObject.tag != "Pipe"
        )
            return;

        if(!isAlive) return;

        isAlive = false;
        anim.SetTrigger("Died");
        audioSource.PlayOneShot(dieClip);
        GameplayController.Instance.PlayerDiedShowScore(this);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "PipeHolder")
        {
            GameplayController.Instance.SetScore();
            audioSource.PlayOneShot(pointClip);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool StopFollow()
    {
        return !isAlive;
    }

    public Vector3 GetPositionOffset()
    {
        return new Vector3(
            Camera.main.transform.position.x - transform.position.x - 1f,
            0,
            0
        );
    }
}
