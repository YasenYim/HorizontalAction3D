using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Character enemy;
    public float changeActTime = 2;  // �ı��ж���ʱ����
    public float tryJumpTime = 0.5f;    // ÿ��0.5�볢����Ծ
    public float jumpChance = 30;       // ÿ����30%�ĸ�����Ծ
    float inputX;         // ģ���������-1~1
    bool jump = false;
    void Start()
    {
        enemy = GetComponent<Character>();
        StartCoroutine(ChangeAct());
        StartCoroutine(EnemyJump());
    }

    void Update()
    {
        enemy.Move(inputX, jump);
        jump = false;
        
    }

    IEnumerator ChangeAct()
    {
        while(true)
        {
            // �ı�inputX
            inputX = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(changeActTime);
        }
    }

    IEnumerator EnemyJump()
    {
        while (true)
        {
            int r = Random.Range(0, 100);
            if(r < jumpChance)
            {
                jump = true;
            }
            yield return new WaitForSeconds(tryJumpTime);

        }
    }
}
