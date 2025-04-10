using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour
{
    [Header("회전")]
    [SerializeField] float turnspeed = 1f;

    [Header("속도")]
    [SerializeField] float movespeed = 15f;  // 기본 주행 속도

    [Header("느리게")]
    [SerializeField] float slowSpeedRatio = 0.5f;
    float slowSpeed;

    [Header("빠르게")]
    [SerializeField] float boostSpeedRatio = 1.5f;
    float boostSpeed;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI speedChangeText;
    [SerializeField] private TextMeshProUGUI dashHintText; // 돌진 힌트용 텍스트
    private Coroutine speedTextCoroutine;

    private bool canDash = false; // 돌진 가능 여부
    private float dashSpeed = 30f;  // 돌진 속도 (기본 주행 속도보다 빠르게 설정)
    private float dashDuration = 0.5f; // 돌진 지속 시간
    private bool isDashing = false;

    void Start()
    {
        slowSpeed = movespeed * slowSpeedRatio;
        boostSpeed = movespeed * boostSpeedRatio;

        if (dashHintText != null)
            dashHintText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 이동
        float moveAmount = Input.GetAxis("Vertical") * movespeed * Time.deltaTime;
        float turnAmount = Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime;

        transform.Rotate(0, 0, -turnAmount);

        if (!isDashing)
            transform.Translate(0, moveAmount, 0);

        // 돌진 시도
        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed, attempting dash.");
            StartCoroutine(Dash());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boost"))
        {
            if (!canDash)
            {
                canDash = true;
                Debug.Log("Boost item collected! canDash set to: " + canDash);
                if (dashHintText != null)
                {
                    dashHintText.text = "Press!" + "[Space Bar]";
                }
                dashHintText?.gameObject.SetActive(true);
            }

            StartCoroutine(RemoveBoostObjectAfterDelay(other.gameObject));
        }
    }

    IEnumerator RemoveBoostObjectAfterDelay(GameObject boostObject)
    {
        yield return new WaitForSeconds(1f);
        Destroy(boostObject);
        Debug.Log("Boost item destroyed.");
    }

    IEnumerator Dash()
    {
        Debug.Log("Dash started!");

        isDashing = true;
        canDash = false; // 돌진 후 다시 돌진할 수 없도록 설정

        dashHintText?.gameObject.SetActive(false); // 돌진 후 안내 텍스트 숨김

        Debug.Log("Dash speed set to: " + dashSpeed);

        float elapsed = 0f;
        Vector3 dashDirection = transform.up; // 현재 바라보는 방향으로 돌진

        // 0.5초 동안 계속 이동
        while (elapsed < dashDuration)
        {
            // 돌진 속도 적용, 이동 방향은 transform.up을 사용
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 돌진 후 기본 속도인 movespeed로 복원
        isDashing = false;
        Debug.Log("Dash ended.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float prevSpeed = movespeed;
        movespeed = slowSpeed;  // 충돌 시 속도 감소
        float delta = movespeed - prevSpeed;
        ShowSpeedChange(delta);
    }

    void ShowSpeedChange(float delta)
    {
        if (speedTextCoroutine != null)
            StopCoroutine(speedTextCoroutine);
        speedTextCoroutine = StartCoroutine(DisplaySpeedChange(delta));
    }

    IEnumerator DisplaySpeedChange(float delta)
    {
        if (Mathf.Approximately(delta, 0f))
        {
            yield break;
        }

        speedChangeText.text = $"Speed!  {(delta > 0 ? "+" : "")}{delta:F2}";
        speedChangeText.color = delta > 0 ? Color.green : Color.red;
        speedChangeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        speedChangeText.gameObject.SetActive(false);
    }
}