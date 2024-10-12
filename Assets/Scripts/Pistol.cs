using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Start()
    {
        //задержки между выстрелами нет
        cooldown = 0;
        //Стрельба не автоматическая. Нужно каждый раз нажимать на кнопку мыши для выстрела
        auto = false;
        ammoCurrent = 10;
        ammoMax = 10;
        ammoBackPack = 30;

    }
    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition);
        RaycastHit hit;
        //Продолжаем писать после строки RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if(hit.collider.CompareTag("enemy"))
            {
                //Число 10 можешь поменять на своё. Это урон, который наносит одна пуля
                //hit.collider.gameObject.GetComponent<Enemy>().ChangeHealth(10);
                hit.collider.gameObject.GetComponent<Enemy>().GetDamage(10);
            }
            else if (hit.collider.CompareTag("Player"))
            {
               hit.collider.gameObject.GetComponent<PlayerController>().GetDamage(10);
            }
            Destroy(gameBullet, 1);
        }
    }
}