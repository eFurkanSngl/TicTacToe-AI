using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Random = UnityEngine.Random;


public class TicTacToe : MonoBehaviour
{
    // UIdaki butonların Text compoenentlerini tutan dizi
    public Text[] buttonList;
    // Oyunu ve AI ın sembollerini temsil eden string
    private string _playerSide = "X";
    private string _aiSide = "O";
    // Oyunun bitip bitmediğini kontrol eden boolen
    private bool gameOver = false;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text winnerText;
    [SerializeField] private Button PlayAgainButton;
   
    

    // oyun başladığında cağrılan method
    private void Start()
    {
        SetGameControllerRefenceOnButtons();
        PlayAgainButton.onClick.AddListener(RestartGame);

    }
    private void RestartGame()
    {
        gameOver = false;
        gameOverPanel.SetActive(false);

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
            buttonList[i].GetComponentInParent<Button>().interactable = true;
        }
    }

    

    // Butona Tıklama olayı dinleyicisi ekleyen metod
   private void SetGameControllerRefenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            // Her butona tıklandığında Metodu cağıracak
            buttonList[i].GetComponentInParent<Button>().onClick.AddListener(() => PlayerClick());
        }
    }

    
    private void PlayerClick()   // oyuncu bir butona tıkladığında cağırılan metod
    {
        // Eğer oyun bittiyse metodu sonlandır
        if (gameOver) return;

        Button clickedButton =
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // Tıklanan butonu al 


        int clickedButtonIndex = System.Array.IndexOf(buttonList, clickedButton.GetComponentInChildren<Text>());
        // burada IndexOf bir liste ve değer istiyor
        // Tıklanan butonun dizideki indexi bullma metodu


        if (buttonList[clickedButtonIndex].text =="")  // Eğer buton boşsa üzerinde X veya O yoksa 
        {
            buttonList[clickedButtonIndex].text = _playerSide;
            // Butona oyununcun sembolünü yaz

            clickedButton.interactable = false;
            // butonu etkileşime kapa

            CheckForWinner();
            // Kazanan var mı diye bak

            if (!gameOver)
            {
                
                AITurn();
            }
        }
    }

    IEnumerator AITurnWithDelay()
    {
        Debug.Log("Gecikme başladı");
        yield return new WaitForSeconds(3f);
        Debug.Log("AI hamle yapıyor");
        AITurn();
    }
    private void AITurn()
    {
        List<int> availableMoves = new List<int>();

        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].text == "")
            {
                availableMoves.Add(i);
            }
        }

        if (availableMoves.Count > 0)
        {
            int randomMove = availableMoves[Random.Range(0, availableMoves.Count)];
            buttonList[randomMove].text = _aiSide;
            buttonList[randomMove].GetComponentInParent<Button>().interactable = false;
            CheckForWinner();
        }
    }  
    

    private void CheckForWinner()
    {
        // Yatay Kontrol
        for (int i = 0; i < 9; i += 3)
        {
            if (buttonList[i].text != "" && buttonList[i].text == buttonList[i+1].text && buttonList[i].text == buttonList[i+2].text)
            {
                GameOver(buttonList[i].text);
                return;
            }
        }
    
        // Dikey Kontrol
        for (int i = 0; i < 3; i++)
        {
            if (buttonList[i].text != "" && buttonList[i].text == buttonList[i+3].text && buttonList[i].text == buttonList[i+6].text)
            {
                GameOver(buttonList[i].text);
                return;
            }
        }
    
        // Çapraz Kontrol
        if (buttonList[0].text != "" && buttonList[0].text == buttonList[4].text && buttonList[0].text == buttonList[8].text)
        {
            GameOver(buttonList[0].text);
            return;
        }

        if (buttonList[2].text != "" && buttonList[2].text == buttonList[4].text && buttonList[2].text == buttonList[6].text)
        {
            GameOver(buttonList[2].text);
            return;
        }

        // Beraberlik kontrolü
        if (IsBoardFull())
        {
            GameOver("tie");
        }
    }

    private void GameOver(string winner)
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
    
        if (winner == "tie")
        {
            winnerText.text = "Berabere";
        }
        else
        {
            winnerText.text = winner + " Kazandı Tebrikler...";
        }

        // Tüm butonları devre dışı bırak
        foreach (Text buttonText in buttonList)
        {
            buttonText.GetComponentInParent<Button>().interactable = false;
        }
    }


    private bool IsBoardFull()
    {
        foreach (Text button in buttonList)
        {
            if (button.text =="")
            {
                return false;
            }
        }

        return true;
    }
    
}