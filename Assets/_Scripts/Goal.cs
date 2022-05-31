using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //Статическое поле, доступное другому коду
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        //Когда в область действия попадает что нибудь 
        //проверить, является ли это снарядом
        if (other.gameObject.tag == "Projectile")
        {
            //Если это снаряд присвоить True
            Goal.goalMet = true;
            // Также изменить альфа канал цвета, чтобы увеличить непрозрачность
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
