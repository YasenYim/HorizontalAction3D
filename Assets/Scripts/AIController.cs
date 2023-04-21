using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Character enemy;
    public float changeActTime = 2;  // 改变行动的时间间隔
    public float tryJumpTime = 0.5f;    // 每隔0.5秒尝试跳跃
    public float jumpChance = 30;       // 每次有30%的概率跳跃
    float inputX;         // 模拟横向输入-1~1
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
            // 改变inputX
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
