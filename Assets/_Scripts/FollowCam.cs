using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;//������ �� ������������ ��� ������
    //��� point of interest ������������ ������
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    [Header("Set Denamycally")]
    public float CamZ;//�������� ���������� z ������
    
    void Awake()
    {
        CamZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        //������������ ������ if �� ������� �������� ������
        // if (POI == null) return;//����� ���� ��� ������������� �������

        //�������� ������� ������������� �������
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        //���� ��� ������������� �������, ������� �:[ 0, 0, 0 ]
        if (POI == null)
        {
            destination = Vector3.zero;
        } else
        {
            // �������� ������� ������������� �������
            destination = POI.transform.position;
            // ���� ������������ ������ - ������, ���������, ��� �� �����������
            if (POI.tag == "Projectile")
            {
                // ���� �� ����� �� ����� (�� ���� �� ���������)
                if (POI.GetComponent<Rigidbody>().IsSleeping() )
                {
                    //������� �������� ��������� ���� ������ ������
                    POI = null;
                    //� ��������� �����
                    return;
                }
            }
        }
        
        
        //���������� � � � ����������� ���������
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //���������� ����� ����� ������� ������������� ������ � destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //������������� ���������� �������� destination.z ������ camZ ����� ���������� ������ ��������
        destination.z = CamZ;
        //��������� ������ � ������� destination
        transform.position = destination;
        //
        Camera.main.orthographicSize = destination.y + 10;
    }
}
