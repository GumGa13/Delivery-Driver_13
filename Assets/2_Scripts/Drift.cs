using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] public float accleration = 20f;    
    [SerializeField] public float steering = 3f;        
    [SerializeField] public float maxSpeed = 10f;       
    [SerializeField] public float driftFactor = 0.95f;  

    public float driftThreshold = 1.5f;

    public ParticleSystem smokeLeft;
    public ParticleSystem smokeRight;
    public TrailRenderer LeftTrail;
    public TrailRenderer RightTrail;

    private Rigidbody2D rb;
    [SerializeField] public AudioSource driftAudioSource;
    [SerializeField] public AudioSource offRoadAudioSource;

    [SerializeField] private RoadChecker roadChecker;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       roadChecker = GetComponent<RoadChecker>();

        if (driftAudioSource == null || offRoadAudioSource == null)
       {
           AudioSource[] sources = GetComponents<AudioSource>();
           if (sources.Length > 1)
           {
               driftAudioSource = sources[0];
               offRoadAudioSource = sources[1];
           }
       }
    }

    void FixedUpdate()
    {
        if (roadChecker != null && !roadChecker.isOnRoad) return; //도로 위가 아니면 조작 안됨.

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
        float forwardVelocity = Vector2.Dot(rb.linearVelocity, transform.up);
        bool isDrifting = Mathf.Abs(sidewayVelocity) > driftThreshold && rb.linearVelocity.magnitude > 2f;
        bool isOffRoad = roadChecker != null && !roadChecker.isOnRoad;

        if (isDrifting)
        {
            if (!smokeLeft.isPlaying) smokeLeft.Play();
            if (!smokeRight.isPlaying) smokeRight.Play();

            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;

            var leftMain = smokeLeft.main;
            var rightMain = smokeRight.main;

            leftMain.startRotation = (angle + 180f) * Mathf.Deg2Rad;
            rightMain.startRotation = (angle + 180f) * Mathf.Deg2Rad;
            LeftTrail.emitting = true;
            RightTrail.emitting = true;
        }
        else
        {
            if (smokeLeft.isPlaying) smokeLeft.Stop();
            if (smokeRight.isPlaying) smokeRight.Stop();
            LeftTrail.emitting = false;
            RightTrail.emitting = false;
        }

        if (isDrifting)
        {
            if (!driftAudioSource.isPlaying) driftAudioSource.Play();
        }
        else
        {
            if (driftAudioSource.isPlaying) driftAudioSource.Stop();
        }

        if (isOffRoad)
        {
            if (!offRoadAudioSource.isPlaying) offRoadAudioSource.Play();
        }
        else
        {
            if (offRoadAudioSource.isPlaying) offRoadAudioSource.Stop();
        }
    }
}
