using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] cars;
    public int currentCarIndex = 0;

    public TextMeshProUGUI tapToStartText;
    public GameObject carSelectionButtons;
    public GameObject settingsPanel;
    public GameObject stagesPanel;

    public AudioClip nextNPreviousClip;
    public AudioClip buttonClip;
    public AudioClip carPurchasedClip;

    public TextMeshProUGUI cashText;
    public TextMeshProUGUI NotEnoughcashText;
    public TextMeshProUGUI deductionMessageText;

    private static bool hasVisited = false;

    public CarBlueprint[] carBlueprints;
    public Button buyButton;
    public Button driveButton;

    void Start()
    {
        Time.timeScale = 1f;
        settingsPanel.SetActive(false);
        stagesPanel.SetActive(false);
        deductionMessageText.gameObject.SetActive(false);

        if (!PlayerPrefs.HasKey("CashValue"))
        {
            PlayerPrefs.SetInt("CashValue", 36000);
            PlayerPrefs.Save();
        }

        if (!hasVisited)
        {
            tapToStartText.gameObject.SetActive(true);
            carSelectionButtons.SetActive(false);
            hasVisited = true;
        }
        else
        {
            tapToStartText.gameObject.SetActive(false);
            carSelectionButtons.SetActive(true);
        }

        CarBlueprint();
        ShowCar(currentCarIndex);

        UpdateCashDisplay();
    }

    void Update()
    {
        UpdateUI();

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlaySFX(buttonClip);
            tapToStartText.gameObject.SetActive(false);
            carSelectionButtons.SetActive(true);
        }
    }

    void CarBlueprint()
    {
        for (int i = 0; i < carBlueprints.Length; i++)
        {
            if (carBlueprints[i].cost == 0)
            {
                carBlueprints[i].isUnlocked = true;
            }
            else
            {
                int cash = PlayerPrefs.GetInt(carBlueprints[i].carName, 0);
                carBlueprints[i].isUnlocked = cash == 0 ? false : true;
            }
        }
    }

    void ShowCar(int index)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(i == index);
        }
    }

    public void NextCar()
    {
        AudioManager.Instance.PlaySFX(nextNPreviousClip);

        currentCarIndex++;

        if (currentCarIndex >= cars.Length)
        {
            currentCarIndex = 0;
        }

        ShowCar(currentCarIndex);
    }

    public void PreviousCar()
    {
        AudioManager.Instance.PlaySFX(nextNPreviousClip);

        currentCarIndex--;

        if (currentCarIndex < 0)
        {
            currentCarIndex = cars.Length - 1;
        }

        ShowCar(currentCarIndex);
    }

    public void SelectCar()
    {
        PlayerPrefs.SetInt("CarIndexValue", currentCarIndex);
    }


    public void UnlockCar()
    {
        AudioManager.Instance.PlaySFX(carPurchasedClip);

        CarBlueprint currentCar = carBlueprints[currentCarIndex];

        PlayerPrefs.SetInt(currentCar.carName, 1);
        PlayerPrefs.SetInt("Selected Car", currentCarIndex);

        currentCar.isUnlocked = true;

        int newCashAmount = PlayerPrefs.GetInt("CashValue", 36000) - currentCar.cost;
        PlayerPrefs.SetInt("CashValue", newCashAmount);

        UpdateCashDisplay();

        ShowDeductionMessage(currentCar.cost);
    }

    private void UpdateCashDisplay()
    {
        int cash = PlayerPrefs.GetInt("CashValue", 36000);
        cashText.text = cash.ToString("N0");
    }

    private void ShowDeductionMessage(int amountDeducted)
    {
        if (deductionMessageText != null)
        {
            deductionMessageText.text = "-" + amountDeducted.ToString("N0");
            StartCoroutine(ShowDeductionMessageCoroutine());
        }
    }

    private IEnumerator ShowDeductionMessageCoroutine()
    {
        if (deductionMessageText != null)
        {
            deductionMessageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            deductionMessageText.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        CarBlueprint currentCar = carBlueprints[currentCarIndex];
        if (currentCar.isUnlocked)
        {
            buyButton.gameObject.SetActive(false);
            driveButton.gameObject.SetActive(true);
            NotEnoughcashText.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = currentCar.cost.ToString("N0");
            driveButton.gameObject.SetActive(false); 


            if (currentCar.cost <= PlayerPrefs.GetInt("CashValue", 0))
            {
                buyButton.interactable = true;
                NotEnoughcashText.gameObject.SetActive(false);
            }
            else
            {
                buyButton.interactable = false;
                NotEnoughcashText.gameObject.SetActive(true);

            }
        }
    }

    public void SettingsButton()
    {
        AudioManager.Instance.PlaySFX(buttonClip);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        AudioManager.Instance.PlaySFX(buttonClip);
        settingsPanel.SetActive(false);
    }

    public void PlayButton()
    {
        AudioManager.Instance.PlaySFX(buttonClip);
        stagesPanel.SetActive(true);
    }

    public void CloseStage()
    {
        AudioManager.Instance.PlaySFX(buttonClip);
        stagesPanel.SetActive(false);
    }
}
