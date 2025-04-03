using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
 
public class Driver : MonoBehaviour
{
    [Header("ȸ��")]
    [SerializeField]
    float turnspeed = 1f;

    [Header("�ӵ�")]
    [SerializeField]
    float movespeed = 0.02f;

    [Header("������")]
    [SerializeField]
    float slowSpeedRatio = 0.5f;
    float slowSpeed;

    [Header("������")]
    [SerializeField]
    float boostSpeedRatio = 1.5f;
    float boostSpeed;

    void Start()
    {
        SayHello("����");

        int result = Multiply(3, 4);
        //Debug.Log("result = " + result);


        int currentHealth = 100;

        currentHealth = ApplyDemage(currentHealth, 30);
        //Debug.Log("currentHealth = " + currentHealth);



        slowSpeed = movespeed * slowSpeedRatio;
        boostSpeed = movespeed * boostSpeedRatio; 
    }
    void SayHello(string name)
    {
        //Debug.Log("Hello, " + name);
    }

    int Multiply(int a, int b)
    {
        return a * b;
    }

    int ApplyDemage(int health, int demage)
    {
        return health - demage;
    }

    void Update()
    {

        float turnAmount = Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * movespeed * Time.deltaTime;

        transform.Rotate(0, 0, -turnAmount);
        transform.Translate(0, moveAmount, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Boost"))
        {
            movespeed = boostSpeed;
            Debug.Log("����!!!!!!!!!!!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        movespeed = slowSpeed;
    }
}