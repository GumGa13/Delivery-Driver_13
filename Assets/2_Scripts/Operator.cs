using JetBrains.Annotations;
using NUnit.Framework.Internal;
using UnityEngine;

public class Operator : MonoBehaviour
{
    [Header("ü��")]
    [SerializeField]
    int health = 10;

    [Header("���� ����")]
    [SerializeField]
    int mathscore = 10;
    [Header("���� ����")]
    [SerializeField]
    int englishscore = 10;

    [Header("����")]
    [SerializeField]
    int level = 5;

    private void Start()
    {
        if (health > 70)
        {
            Debug.Log("���� �ǰ�");
        }
        else if (health > 30)
        {
            Debug.Log("�ణ �ǰ�");
        }
        else if (health > 0)
        {
            Debug.Log("������!");
        }
        else
        {
            Debug.Log("����");
        }



        if(mathscore >= 60 && englishscore >= 60)
        {
            float ��� = (mathscore + englishscore) / 2;
            if (��� >= 90)
            {
                Debug.Log("����� �հ�");
            }
            else
            {
                Debug.Log("�Ϲݹ� �հ�");
            }
        }
        else
        {
            Debug.Log("�� �� ��");
        }


        bool hasSpecialItem = true;
        bool isInBattle = true;

        if (level >= 5 && hasSpecialItem && isInBattle)
        {
            Debug.Log("������ �� �� ����");
        }
        else
        {
            Debug.Log("����̳� ������");
        }
    }

}
