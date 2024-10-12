using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] GameObject startButton;
    [SerializeField] TMP_Text codeRoom; // Добавлено для отображения кода комнаты
    [SerializeField] Button copyButton; // Кнопка для копирования кода комнаты
    
    // Start is called before the first frame update
    void Start()
    {
        // Проверяем, является ли игрок мастером
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }

        // Проверяем, есть ли сохраненный победитель
        if (PlayerPrefs.HasKey("Winner") && PhotonNetwork.IsMasterClient)
        {
            string winner = PlayerPrefs.GetString("Winner");
            photonView.RPC("ShowMessage", RpcTarget.All, "The last match was won: " + winner);
            PlayerPrefs.DeleteAll();
        }

        // Отображаем код комнаты, если комната создана
        if (PhotonNetwork.InRoom)
        {
            codeRoom.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
            // Активируем кнопку копирования, если комната создана
            copyButton.gameObject.SetActive(true); 
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    void Log(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Log(otherPlayer.NickName + " left the room");
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Log(newPlayer.NickName + " joined the room");
    }

    [PunRPC]
    public void ShowMessage(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public void Send()
    {
        if (string.IsNullOrWhiteSpace(InputText.text)) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            InputText.text = string.Empty;
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
 // Метод для копирования кода комнаты в буфер обмена
  public void CopyRoomCode()
  {
    // Получаем код комнаты из текста
    string roomCode = codeRoom.text.Substring(12); 

    // Копируем код в буфер обмена
    GUIUtility.systemCopyBuffer = roomCode;

    // Выводим сообщение о том, что код скопирован
    Debug.Log("Room code copied to clipboard: " + roomCode);
  }

}


