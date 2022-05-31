using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;//Ссылка на интересующий нас обьект
    //это point of interest интересующий обьект
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    [Header("Set Denamycally")]
    public float CamZ;//Желаемая координата z камеры
    
    void Awake()
    {
        CamZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        //Однострочная версия if не требует фигурных скобок
        // if (POI == null) return;//Выйти если нет интересующего обьекта

        //Получить позицию интересующего обьекта
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        //Если нет интересующего объекта, вернуть Р:[ 0, 0, 0 ]
        if (POI == null)
        {
            destination = Vector3.zero;
        } else
        {
            // Получить позицию интересующего объекта
            destination = POI.transform.position;
            // Если интересующий объект - снаряд, убедиться, что он остановился
            if (POI.tag == "Projectile")
            {
                // Если он стоит на месте (то есть не двигается)
                if (POI.GetComponent<Rigidbody>().IsSleeping() )
                {
                    //Вернуть исходные настройки поля зрения камеры
                    POI = null;
                    //в следующем кадре
                    return;
                }
            }
        }
        
        
        //ограничить Х и У минимальным значением
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //Определить точку между текущим местопожением камеры и destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //Принудительно установить значение destination.z равным camZ чтобы отодвинуть камеру подальше
        destination.z = CamZ;
        //Поместить камеру в позицию destination
        transform.position = destination;
        //
        Camera.main.orthographicSize = destination.y + 10;
    }
}
