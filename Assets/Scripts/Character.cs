using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterController cc;
    protected Animator animator;
    Vector3 pendingVelocity;   // 这一帧移动的向量
    public float speed = 6;
    public float airSpeed = 3;
    public float dropDate = 2;
    public float jumpSpeed = 10;
    [Range(0.25f,0.29f)]
    public float rayLength = 0.3f;
    bool isGround = false;
    public GameObject deathParticle;    // 死亡粒子特效
    

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // 移动函数
    public void Move(float input,bool jump)  // 只需要横向输入
    {
        pendingVelocity.x = input * speed;
        if(Mathf.Abs(input) > 0.1)
        {
            // 考虑旋转
            Quaternion right = Quaternion.LookRotation(Vector3.forward);
            Quaternion left = Quaternion.LookRotation(Vector3.back);

            if(input > 0)
            {
                // 往右转
                transform.rotation = Quaternion.Slerp(transform.rotation,right,0.05f);
            }
            else
            {
                // 往左转
                transform.rotation = Quaternion.Slerp(transform.rotation, left, 0.05f);
            }
        }

        // 处理跳跃逻辑
        if(jump && isGround)
        {
            // 正常跳跃
            pendingVelocity.y = jumpSpeed;
            pendingVelocity.x = input * airSpeed;
            isGround = false;
      
        }
        // 考虑下落
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

        // 更新动画
        UpdateAnim();

        // 踩踏攻击检测
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
        Ray ray = new Ray(transform.position,Vector3.down); // 从脚底发射
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,0.1f,LayerMask.GetMask("Cha")))
        {
            // 不要打中自身
            if(hit.transform == transform) { return; }
            // 被打中的物体，是hit.transform,hit指的是碰撞检测的信息，碰撞的位置，碰撞面的朝向，hit.transform是指被碰到的物体
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
        Destroy(go, 2);   // 2秒后删除粒子
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
