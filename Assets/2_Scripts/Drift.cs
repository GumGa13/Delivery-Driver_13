using UnityEngine;
using UnityEngine.Audio;

public class Drift : MonoBehaviour
{
    [SerializeField] float accleration = 20f;    //전진, 후진 가속도
    [SerializeField] float steering = 3f;        //조항 속도
    [SerializeField] float maxSpeed = 10f;       //최대 속도 제한
    [SerializeField] float driftFactor = 0.95f;  //낮을수록 더 미끄러짐

    [SerializeField] float slowAccelerationRatio = 0.5f;
    [SerializeField] float boostAccelerationRatio = 1.5f;

    [SerializeField] ParticleSystem smokeLeft;
    [SerializeField] ParticleSystem smokeRight;
    [SerializeField] TrailRenderer LeftTrail;
    [SerializeField] TrailRenderer RightTrail;

    Rigidbody2D rb;
    AudioSource audioSource;

      void Start()
      {
        rb = GetComponent<Rigidbody2D>();
        audioSource = rb.GetComponent<AudioSource>();

        defaultAcceleration = accleration;
        slowAccelerationRatio = accleration * slowAccelerationRatio;
        boostAccelerationRatio = accleration * boostAccelerationRatio;
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

        //Drift
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
     }

    private void Update()
    {
        float sidewayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);

        bool isDrifting = rb.linearVelocity.magnitude > 2f && Mathf.Abs(sidewayVelocity) > 1f;
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

        LeftTrail.emitting = isDrifting;
        RightTrail.emitting = isDrifting;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        accleration = boostAcceleration;

    }
}
