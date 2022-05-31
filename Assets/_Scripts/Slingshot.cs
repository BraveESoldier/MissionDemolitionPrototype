using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    //����, ��������������� � ���������� Unity
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //����. ���������������� �����������
    [Header("Set Dynamically")]
    public GameObject LaunchPoint;
    public Vector3 LaunchPos;
    public GameObject projectile;
    public bool AimingMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.LaunchPos;
        }
    }
    
    void Awake()
    {
        S = this;
        Transform LaunchPointTrans = transform.Find("LaunchPoint");
        LaunchPoint = LaunchPointTrans.gameObject;
        LaunchPoint.SetActive(false);
        LaunchPos = LaunchPointTrans.position;
    }

    void OnMouseEnter()
    {
        //print("SlingShot:OnMouseEnter()");
        LaunchPoint.SetActive(true);
    }
    
    void OnMouseExit()
    {
        //print("Slingshot:OnMpuseExit()");
        LaunchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        //����� ����� ������ ����, ����� ��������� ��� �������� 
        AimingMode = true;
        //������� ������ 
        projectile = Instantiate(prefabProjectile) as GameObject;
        //��������� � ����� LaunchPoint
        projectile.transform.position = LaunchPos;
        //������� ��� ��������������
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    void Update()
    {
        //���� ������� �� � ������ ������������ �� ��������� ���� ���
        if (!AimingMode) return;

        //�������� ������� �������� ���������� ��������� ����
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //����� �������� ��������� ����� LaunchPos � mousePos3D
        Vector3 mouseDelta = mousePos3D - LaunchPos;
        //��������� mouseDelta �������� ���������� ������� Slingshot
        float maxMagnitube = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitube) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitube;
        }

        //����������� ������ � ����� �������
        Vector3 projPos = LaunchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            //������ ���� �������� 
            AimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
