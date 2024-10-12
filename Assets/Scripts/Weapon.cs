using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class Weapon : MonoBehaviourPunCallbacks
{
[SerializeField] AudioSource shoot;
[SerializeField] AudioClip bulletSound, noBulletSound, reload;
//Объект Particle System, который будет оставлять след от пуль
[SerializeField] protected GameObject particle;
//Камера(понадобится для определения центра экрана)
[SerializeField] protected GameObject cam;
//Тип стрельбы
protected bool auto = false;
//Время задержки между выстрелами и таймер, который считает время
protected float cooldown = 0;
protected  float timer = 0;
//Сколько патронов в обойме
protected int ammoCurrent;
//Сколько патронов помещается в обойму
protected int ammoMax;
//Сколько патронов в запасе
protected int ammoBackPack;
//Переменная для отображения текста на UI
[SerializeField] TMP_Text ammoText;
    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
    if(photonView.IsMine)
    {
        //Запускаем таймер
    timer += Time.deltaTime;
    //Если удерживаем клавишу мыши, то вызываем метод Shoot
    if (Input.GetMouseButton(0)) 
    {
        Shoot();
    }
    AmmoTextUpdate();
    //если игрок нажмет кнопку R
if (Input.GetKeyDown(KeyCode.R))
{
    //если у нас кол-во патронов в обойме НЕ максимальное ИЛИ, если в запасе патронов больше нуля, то
    if(ammoCurrent != ammoMax || ammoBackPack != 0)
    {
        //активируем метод перезарядки с задержкой
        //время задержки можно установить самостоятельно
        Invoke("Reload", 1);
        shoot.PlayOneShot(reload);
    }
}
}  
}
//способность оружия стрелять. 
public void Shoot()
{
    if (Input.GetMouseButtonDown(0) || auto)
    {
        if (timer > cooldown)
        {
            if(ammoCurrent > 0)
            {
            OnShoot();
            timer = 0;
            ammoCurrent = ammoCurrent - 1;
            shoot.PlayOneShot(bulletSound);   
            shoot.pitch = Random.Range(1f, 1.5f);
            }
            else
            {
                shoot.PlayOneShot(noBulletSound);
            }
        }
    }
}
//Что происходит при выстреле, эту функцию сможет менять дочерний класс так она определенна модификатором protected virtual
protected virtual void OnShoot()
{
}
private void AmmoTextUpdate()
{
    ammoText.text = ammoCurrent + " / " + ammoBackPack;
}
private void Reload()
{
    //создаем временную переменную, которая высчитывает сколько патронов нам нужно добавить
    int ammoNeed = ammoMax - ammoCurrent; 
    //если кол-во патронов в запасе больше или равно кол-ву, которое нам нужно добавить то,
    if (ammoBackPack >= ammoNeed) 
    {
        //из кол-ва патронов в запасе вычитаем кол-во, которое добавляем в обойму
        ammoBackPack -= ammoNeed;
        //в обойму добавляем нужное количество патронов
        ammoCurrent += ammoNeed;
    }
    //иначе(если в запасе меньше патронов, чем нам нужно)
    else 
    {
        //добавляем в обойму столько патронов, сколько осталось в запасе
        ammoCurrent += ammoBackPack;
        //обнуляем кол-во патронов в запасе
        ammoBackPack = 0;
    }
}
    }

