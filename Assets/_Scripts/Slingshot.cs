using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    //поля, устанавливаемые в инспекторе Unity
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //поля. устанавливаемыее динамически
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
        //Игрок нажал кнопку мыши, когда указатель над рогаткой 
        AimingMode = true;
        //Создать снаряд 
        projectile = Instantiate(prefabProjectile) as GameObject;
        //Поместить в точку LaunchPoint
        projectile.transform.position = LaunchPos;
        //Сделать его кинематическим
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    void Update()
    {
        //Если рогатка не в режими прицеливания не выполнять этот код
        if (!AimingMode) return;

        //Получить текущие экранные координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //Найти разность координат между LaunchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - LaunchPos;
        //Ограничть mouseDelta радиусом коллайдера обьекта Slingshot
        float maxMagnitube = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitube) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitube;
        }

        //Передвинуть снаряд в новую позицию
        Vector3 projPos = LaunchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            //Кнопка мыши отпущена 
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
