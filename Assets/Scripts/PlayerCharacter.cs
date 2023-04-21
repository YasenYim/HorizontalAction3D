using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    Transform grabTrans;     // 正在抓取的物体，没有物体为null
    public Transform grabPos;       // 抓取物体的点
    public float rayLength2 = 1f;


    public void Grab()
    {
        if(grabTrans == null)
        {
            // 尝试去抓箱子
            Vector3 rayStart = transform.position + new Vector3(0, 0.7f, 0);
            Vector3 rayEnd = rayStart + transform.right;
            Debug.DrawLine(rayStart, rayEnd, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(rayStart, transform.right, out hit, rayLength2, LayerMask.GetMask("GrabBox")))
            {
                Transform box = hit.transform; // hit.transform指的是箱子
                box.parent = grabPos;         
                box.localPosition = Vector3.zero;  // 箱子被抓到以后相对于我们抓取的点要处于相对静止的状态
                box.localRotation = Quaternion.identity;
                box.GetComponent<Rigidbody>().isKinematic = true;   // 改成动力学刚体
                grabTrans = box;
            }
        }
        else
        {
            // 扔箱子逻辑
            grabTrans.parent = null;     // 设置物体的父物体是空，回到第一级物体
            grabTrans.GetComponent<Rigidbody>().isKinematic = false;
            grabTrans.position = new Vector3(grabTrans.position.x, grabTrans.position.y, 0);
            grabTrans = null;
        }
        
    }
    private void Update()
    {
        if(grabTrans == null)
        {
            animator.SetBool("Grab", false);
        }
        else
        {
            animator.SetBool("Grab", true);
        }
    }

}
