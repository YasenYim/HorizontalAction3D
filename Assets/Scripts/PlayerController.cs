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
        float h = Input.GetAxis("Horizontal");   // ����ű��õ�һ��-1~1��hֵ�����뵽��ɫ�ű������cc���ƶ�
        bool jump = Input.GetButton("Jump");
        character.Move(h,jump);
        if(Input.GetButtonDown("Fire1"))
        {
            character.Grab();
        }
    }
}
