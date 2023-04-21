using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterController cc;
    protected Animator animator;
    Vector3 pendingVelocity;   // ��һ֡�ƶ�������
    public float speed = 6;
    public float airSpeed = 3;
    public float dropDate = 2;
    public float jumpSpeed = 10;
    [Range(0.25f,0.29f)]
    public float rayLength = 0.3f;
    bool isGround = false;
    public GameObject deathParticle;    // ����������Ч
    

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // �ƶ�����
    public void Move(float input,bool jump)  // ֻ��Ҫ��������
    {
        pendingVelocity.x = input * speed;
        if(Mathf.Abs(input) > 0.1)
        {
            // ������ת
            Quaternion right = Quaternion.LookRotation(Vector3.forward);
            Quaternion left = Quaternion.LookRotation(Vector3.back);

            if(input > 0)
            {
                // ����ת
                transform.rotation = Quaternion.Slerp(transform.rotation,right,0.05f);
            }
            else
            {
                // ����ת
                transform.rotation = Quaternion.Slerp(transform.rotation, left, 0.05f);
            }
        }

        // ������Ծ�߼�
        if(jump && isGround)
        {
            // ������Ծ
            pendingVelocity.y = jumpSpeed;
            pendingVelocity.x = input * airSpeed;
            isGround = false;
      
        }
        // ��������
        if(!isGround)
        {
            pendingVelocity.y += Physics.gravity.y  * dropDate * Time.deltaTime;
            pendingVelocity.x = input * airSpeed;
        }
        else
        {
            pendingVelocity.y = 0;
            pendingVelocity.x = input * speed;   
        }

        cc.Move(pendingVelocity * Time.deltaTime);

        // ���¶���
        UpdateAnim();

        // ��̤�������
        AttackCheck();
    }
    private void FixedUpdate()
    {
        isGround = false;
        Ray ray = new Ray(transform.position + new Vector3(0, 0.2f, 0), Vector3.down);
        Vector3 rayStart = transform.position + new Vector3(0, 0.2f, 0);
        Vector3 rayEnd = rayStart + new Vector3(0, -rayLength, 0);
        //Debug.DrawLine(rayStart, rayEnd,Color.blue);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,rayLength,LayerMask.GetMask("Default")))
        {
            isGround = true;
        }
    }
    void AttackCheck()
    {
        Ray ray = new Ray(transform.position,Vector3.down); // �ӽŵ׷���
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,0.1f,LayerMask.GetMask("Cha")))
        {
            // ��Ҫ��������
            if(hit.transform == transform) { return; }
            // �����е����壬��hit.transform,hitָ������ײ������Ϣ����ײ��λ�ã���ײ��ĳ���hit.transform��ָ������������
            Character cha = hit.transform.GetComponent<Character>();
            cha.TakeDamage();
            Bounce();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
    public void TakeDamage()
    {
        Debug.Log(gameObject.name + "TakeDamage");
        Destroy(gameObject);
        GameObject go = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(go, 2);   // 2���ɾ������
    }
    void Bounce()
    {
        pendingVelocity.y = jumpSpeed / 2; 
    }
    void UpdateAnim()
    {
        animator.SetFloat("Forward", cc.velocity.magnitude / speed);
        animator.SetBool("IsGround", isGround);
    }
  
}
