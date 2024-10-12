using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
//создаем переменную
private int previousPlayerCount;
int checkPlayers = 0;
[SerializeField] List<Transform> spawns = new List<Transform>();int randSpawn;
[SerializeField] List<Transform> spawnsWalk = new List<Transform>();
[SerializeField] List<Transform> spawnsTurret = new List<Transform>();
//Ссылка на текст
[SerializeField] public TMP_Text playersText;
//Массив, в котором будут хранится все игроки
GameObject[] players;
//Список, в котором будут хранится живые игроки
List<string> activePlayers = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
    //получаем рандомное число
    randSpawn = Random.Range(0, spawns.Count);
    PhotonNetwork.Instantiate("Player", spawns[randSpawn].position, spawns[randSpawn].rotation);
    Invoke("SpawnEnemy", 5f);
    //Эту строку пишем в методе Start()
    //При помощи функции Photon получаем количество игроков на сервере
    previousPlayerCount = PhotonNetwork.PlayerList.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length < previousPlayerCount)
        {
            ChangePlayersList();
        }
        previousPlayerCount = PhotonNetwork.PlayerList.Length;
    }
    public void SpawnEnemy()
{
    if (PhotonNetwork.IsMasterClient)
    {
        for (int i = 0; i < spawnsWalk.Count; i++)
        {
            PhotonNetwork.Instantiate("WalkEnemy", spawnsWalk[i].position, spawnsWalk[i].rotation);
            Invoke("SpawnEnemy", 15f);
        }
        for (int i = 0; i < spawnsTurret.Count; i++)
        {
            PhotonNetwork.Instantiate("Turret", spawnsTurret[i].position, spawnsTurret[i].rotation);
        }
    }
}
[PunRPC]
public void PlayerList()
{
    players = GameObject.FindGameObjectsWithTag("Player");
    activePlayers.Clear();
    foreach(GameObject player in players)
{
    //если игрок жив, то
    if(player.GetComponent<PlayerController>().dead == false)
    {
        //добавляем его сетевое имя в список активных игроков
        activePlayers.Add(player.GetComponent<PhotonView>().Owner.NickName);             
    }
    playersText.text = "Игроки в игре : " + activePlayers.Count.ToString();
}
//Если у нас остался 1 игрок, то..
if (activePlayers.Count <= 1 && checkPlayers > 0)
{ 
    PlayerPrefs.SetString("Winner", activePlayers[0]);           
    //Ищем всех врагов на карте и кладем их в массив
    var enemies = GameObject.FindGameObjectsWithTag("enemy");
    //Перебираем всех врагов в массиве
    foreach (GameObject enemy in enemies)
    {
        //Всем врагам вычитаем 100HP(если у твоих врагов больше HP, то измени кол-во отнимаемых HP)
        enemy.GetComponent<Enemy>().ChangeHealth(100);
    }
    Invoke("EndGame", 5f);
}
checkPlayers++;
}
public void ChangePlayersList()
{
    photonView.RPC("PlayerList", RpcTarget.All);   
}
void EndGame()
{        
    //переходим в Lobby
    PhotonNetwork.LoadLevel("Lobby");        
}
//Метод для кнопки
public void ExitGame()
{
    PhotonNetwork.LeaveRoom();
}
//Метод фотон, который срабатывает при выходе
public override void OnLeftRoom()
{
    //запускаем сцену с Меню игры
    SceneManager.LoadScene(0);
    //обновляем список игроков в матче
    ChangePlayersList();
}
}
