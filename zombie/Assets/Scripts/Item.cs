using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Item : MonoBehaviour
{
    public TextMeshProUGUI money_text;
    public Button RevolverButton, M4A1Button, AK47Button, ScarButton, SniperButton, P90Button;
    public GameObject Successfully_text, Insufficient_text, Alredy_text;
    private int money;

    private void Start()
    {
        money = PlayerPrefs.GetInt("moneys");
        money_text.text = money.ToString() + " $";
        RevolverButton.onClick.AddListener(() => Buy(50, "Revolver"));
        M4A1Button.onClick.AddListener(() => Buy(200, "M4A1"));
        AK47Button.onClick.AddListener(() => Buy(500, "AK-47"));
        ScarButton.onClick.AddListener(() => Buy(1000, "Scar"));
        SniperButton.onClick.AddListener(() => Buy(1500, "Sniper"));
        P90Button.onClick.AddListener(() => Buy(2000, "P90"));
    }

    public void Buy(int price, string name)
    {
        if (money >= price && PlayerPrefs.GetInt(name) != price)
        {
            PlayerPrefs.SetInt(name, price);
            money -= price;
            SetMoney();
            StartCoroutine(ShowSuccessfully(Successfully_text));
        }
        else if (PlayerPrefs.GetInt(name) == price)
            StartCoroutine(ShowSuccessfully(Alredy_text));
        else if (money < price)
            StartCoroutine(ShowSuccessfully(Insufficient_text));
    }

    private IEnumerator ShowSuccessfully(GameObject text)
    {
        Insufficient_text.SetActive(false);
        Alredy_text.SetActive(false);
        Successfully_text.SetActive(false);
        text.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        text.SetActive(false);
    }

    public void Set_1000_money()
    {
        PlayerPrefs.DeleteAll();
        money = 3000;
        SetMoney();
    }

    private void SetMoney()
    {
        PlayerPrefs.SetInt("moneys", money);
        money_text.text = money.ToString() + " $";
    }

}
