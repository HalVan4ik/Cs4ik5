using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class MenuManager : MonoBehaviourPunCallbacks
{
[SerializeField] TMP_Text logText;
[SerializeField] TMP_InputField inputField;
[SerializeField] TMP_InputField roomName;
string roomCode;
    // Start is called before the first frame update
    void Start()
    {
    //присваиваем игроку них с рандомным числом
    PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
    //Отображаем ник игрока в поле Log
    Log("Player Name: " + PhotonNetwork.NickName);
    //Настройки игры
    PhotonNetwork.AutomaticallySyncScene = true; //Автопереключение сцены
    PhotonNetwork.GameVersion = "1"; //Версия игры
    PhotonNetwork.ConnectUsingSettings(); //Подключается к серверу Photon
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Log(string message) 
    { 
        logText.text += "\n"; 
        logText.text += message; 
    }
    public void CreateRoom()
{
    PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });

}
public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
public override void OnConnectedToMaster()
{
    Log("Connected to the server");
}

public void ChangeName()
{
    //Считываем то, что написал игрок в поле InputField
    PhotonNetwork.NickName = inputField.text;
    //Выводим в поле игрока его новый никнейм
    Log("New Player name: " + PhotonNetwork.NickName);
}
 public void JoinRoomByCode()
  {
    string roomCode = roomName.text;

    // Проверка, пустой ли код комнаты
    if (string.IsNullOrWhiteSpace(roomCode))
    {
      Debug.LogWarning("Введите код комнаты!");
      return;
    }

    // Попытка присоединения к комнате по коду
    PhotonNetwork.JoinRoom(roomCode);
  }

  // Обработчик события, когда игрок присоединился к комнате
  public override void OnJoinedRoom()
  {
    // Отображаем код комнаты
    roomCode = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
    Log("Joined the lobby");
    PhotonNetwork.LoadLevel("Lobby");
    var roomName = PhotonNetwork.CurrentRoom.Name;
  }

  // Обработчик события, когда не удалось присоединиться к комнате
  public override void OnJoinRoomFailed(short returnCode, string message)
  {
    // Вывод сообщения об ошибке
    Debug.LogError("Не удалось присоединиться к комнате: " + message);
    // ... (обработка ошибки)
  }
}
    

