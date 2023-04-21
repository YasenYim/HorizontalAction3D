using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    Transform grabTrans;     // ����ץȡ�����壬û������Ϊnull
    public Transform grabPos;       // ץȡ����ĵ�
    public float rayLength2 = 1f;


    public void Grab()
    {
        if(grabTrans == null)
        {
            // ����ȥץ����
            Vector3 rayStart = transform.position + new Vector3(0, 0.7f, 0);
            Vector3 rayEnd = rayStart + transform.right;
            Debug.DrawLine(rayStart, rayEnd, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(rayStart, transform.right, out hit, rayLength2, LayerMask.GetMask("GrabBox")))
            {
                Transform box = hit.transform; // hit.transformָ��������
                box.parent = grabPos;         
                box.localPosition = Vector3.zero;  // ���ӱ�ץ���Ժ����������ץȡ�ĵ�Ҫ������Ծ�ֹ��״̬
                box.localRotation = Quaternion.identity;
                box.GetComponent<Rigidbody>().isKinematic = true;   // �ĳɶ���ѧ����
                grabTrans = box;
            }
        }
        else
        {
            // �������߼�
            grabTrans.parent = null;     // ��������ĸ������ǿգ��ص���һ������
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
