using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour
{
    [Header("ȸ��")]
    [SerializeField] float turnspeed = 1f;

    [Header("�ӵ�")]
    [SerializeField] float movespeed = 15f;  // �⺻ ���� �ӵ�

    [Header("������")]
    [SerializeField] float slowSpeedRatio = 0.5f;
    float slowSpeed;

    [Header("������")]
    [SerializeField] float boostSpeedRatio = 1.5f;
    float boostSpeed;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI speedChangeText;
    [SerializeField] private TextMeshProUGUI dashHintText; // ���� ��Ʈ�� �ؽ�Ʈ
    private Coroutine speedTextCoroutine;

    private bool canDash = false; // ���� ���� ����
    private float dashSpeed = 30f;  // ���� �ӵ� (�⺻ ���� �ӵ����� ������ ����)
    private float dashDuration = 0.5f; // ���� ���� �ð�
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
        // �̵�
        float moveAmount = Input.GetAxis("Vertical") * movespeed * Time.deltaTime;
        float turnAmount = Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime;

        transform.Rotate(0, 0, -turnAmount);

        if (!isDashing)
            transform.Translate(0, moveAmount, 0);

        // ���� �õ�
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
        canDash = false; // ���� �� �ٽ� ������ �� ������ ����

        dashHintText?.gameObject.SetActive(false); // ���� �� �ȳ� �ؽ�Ʈ ����

        Debug.Log("Dash speed set to: " + dashSpeed);

        float elapsed = 0f;
        Vector3 dashDirection = transform.up; // ���� �ٶ󺸴� �������� ����

        // 0.5�� ���� ��� �̵�
        while (elapsed < dashDuration)
        {
            // ���� �ӵ� ����, �̵� ������ transform.up�� ���
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ���� �� �⺻ �ӵ��� movespeed�� ����
        isDashing = false;
        Debug.Log("Dash ended.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float prevSpeed = movespeed;
        movespeed = slowSpeed;  // �浹 �� �ӵ� ����
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