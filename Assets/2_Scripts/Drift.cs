using UnityEngine;
using UnityEngine.Audio;

public class Drift : MonoBehaviour
{
    [SerializeField] public float accleration = 20f;    //전진, 후진 가속도
    [SerializeField] public float steering = 3f;        //조항 속도
    [SerializeField] public float maxSpeed = 10f;       //최대 속도 제한
    [SerializeField] public float driftFactor = 0.95f;  //낮을수록 더 미끄러짐

    public float driftThreshold = 1.5f;

    public ParticleSystem smokeLeft;
    public ParticleSystem smokeRight;
    public TrailRenderer LeftTrail;
    public TrailRenderer RightTrail;

    private Rigidbody2D rb;
    private AudioSource audioSource;

     void Start()
     {
       rb = GetComponent<Rigidbody2D>();
       audioSource = GetComponent<AudioSource>();
     }

    void FixedUpdate()
    {
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * Input.GetAxis("Vertical") * accleration);
        }

        //float turnAmount = Input.GetAxis("Horizontal") * steering * speed * Time.fixedDeltaTime;
        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed / maxSpeed, 0.4f, 1f);
        rb.MoveRotation(rb.rotation - turnAmount);

        ApplyDrift();
    }

    void ApplyDrift()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);

    }

    void Update()
    {
        float sidewayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);

        bool isDrifting = Mathf.Abs(sidewayVelocity) > driftThreshold && rb.linearVelocity.magnitude > 2f;
        if (isDrifting)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            if (!smokeLeft.isPlaying) smokeLeft.Play();
            if (!smokeRight.isPlaying) smokeRight.Play();
        }
        else
        {
            if (!audioSource.isPlaying) audioSource.Stop();
            if (!smokeLeft.isPlaying) smokeLeft.Stop();
            if (!smokeRight.isPlaying) smokeRight.Stop();
        }

        audioSource.volume = Mathf.Lerp(audioSource.volume, isDrifting ? 1f : 0f, Time.deltaTime * 5f);
        LeftTrail.emitting = isDrifting;
        RightTrail.emitting = isDrifting;
    }
}
