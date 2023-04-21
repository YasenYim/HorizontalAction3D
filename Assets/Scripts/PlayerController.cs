using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter character;
    void Start()
    {
        character = GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");   // 输入脚本得到一个-1~1的h值，传入到角色脚本里，控制cc的移动
        bool jump = Input.GetButton("Jump");
        character.Move(h,jump);
        if(Input.GetButtonDown("Fire1"))
        {
            character.Grab();
        }
    }
}
