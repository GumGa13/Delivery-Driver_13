using JetBrains.Annotations;
using NUnit.Framework.Internal;
using UnityEngine;

public class Operator : MonoBehaviour
{
    [Header("체력")]
    [SerializeField]
    int health = 10;

    [Header("수학 점수")]
    [SerializeField]
    int mathscore = 10;
    [Header("영어 점수")]
    [SerializeField]
    int englishscore = 10;

    [Header("레벨")]
    [SerializeField]
    int level = 5;

    private void Start()
    {
        if (health > 70)
        {
            Debug.Log("아임 건강");
        }
        else if (health > 30)
        {
            Debug.Log("약간 건강");
        }
        else if (health > 0)
        {
            Debug.Log("데인졀!");
        }
        else
        {
            Debug.Log("망사");
        }



        if(mathscore >= 60 && englishscore >= 60)
        {
            float 평균 = (mathscore + englishscore) / 2;
            if (평균 >= 90)
            {
                Debug.Log("우수수 합격");
            }
            else
            {
                Debug.Log("일반반 합격");
            }
        }
        else
        {
            Debug.Log("불 합 격");
        }


        bool hasSpecialItem = true;
        bool isInBattle = true;

        if (level >= 5 && hasSpecialItem && isInBattle)
        {
            Debug.Log("아이템 쓸 수 있음");
        }
        else
        {
            Debug.Log("어른템이나 쓰시지");
        }
    }

}
