using System.Collections;
using TMPro;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [Header("회전")]
    [SerializeField] float turnspeed = 200f;

    [Header("속도")]
    [SerializeField] float movespeed = 15f;  // 기본 주행 속도
    private float currentSpeed;

    [Header("느리게")]
    [SerializeField] float slowSpeedRatio = 0.5f;
    float slowSpeed;

    [Header("빠르게")]
    [SerializeField] float boostSpeedRatio = 1.5f;
    float boostSpeed;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI speedChangeText;
    [SerializeField] private TextMeshProUGUI dashHintText;
    private Coroutine speedTextCoroutine;

    private bool canDash = false;
    private float dashSpeed = 45f;
    private float dashDuration = 0.5f;
    private bool isDashing = false;

    void Start()
    {
        currentSpeed = movespeed;
        slowSpeed = movespeed * slowSpeedRatio;
        boostSpeed = movespeed * boostSpeedRatio;

        if (dashHintText != null)
            dashHintText.gameObject.SetActive(false);
    }

    void Update()
    {
        float moveAmount = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        float turnAmount = Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime;

        transform.Rotate(0, 0, -turnAmount);

        if (!isDashing)
            transform.Translate(0, moveAmount, 0);

        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스 바 누름.");
            StartCoroutine(Dash());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boost"))
        {
            canDash = true;
            Debug.Log("부스트 획득! 돌진 가능 : " + canDash);

            if (dashHintText != null)
            {
                dashHintText.text = "Press! [Space Bar]";
                dashHintText.gameObject.SetActive(true);
            }

            StartCoroutine(RemoveBoostObjectAfterDelay(other.gameObject));
        }
    }

    IEnumerator RemoveBoostObjectAfterDelay(GameObject boostObject)
    {
        yield return new WaitForSeconds(1f);
        Destroy(boostObject);
        Debug.Log("부스트 아이템 삭제.");
    }

    IEnumerator Dash()
    {
        Debug.Log("돌진!!!");
        isDashing = true;
        canDash = false;
        dashHintText?.gameObject.SetActive(false);

        float prevSpeed = currentSpeed;
        currentSpeed = dashSpeed;

        ShowSpeedChange(currentSpeed - prevSpeed); // 속도 증가 텍스트 표시

        Vector3 dashDirection = transform.up;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentSpeed = movespeed;
        isDashing = false;
        Debug.Log("돌진 중지.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float prevSpeed = currentSpeed;
        currentSpeed = slowSpeed;
        float delta = currentSpeed - prevSpeed;
        ShowSpeedChange(delta);
    }

    void ShowSpeedChange(float delta)
    {
        if (Mathf.Approximately(delta, 0f))
            return;

        if (speedTextCoroutine != null)
            StopCoroutine(speedTextCoroutine);

        speedTextCoroutine = StartCoroutine(DisplaySpeedChange(delta));
    }

    IEnumerator DisplaySpeedChange(float delta)
    {
        speedChangeText.text = $"Speed!  {(delta > 0 ? "+" : "")}{delta:F2}";
        speedChangeText.color = delta > 0 ? Color.green : Color.red;
        speedChangeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        speedChangeText.gameObject.SetActive(false);
    }
}