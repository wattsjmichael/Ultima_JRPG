using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceMenu_mw : MonoBehaviour
{
    public static EnhanceMenu instance;
    public GameObject enhanceMenu;
    public int baseSuccessRate = 50;
    public int enhancementLevel;
    public int enhancementAttempts;
    public int currentSuccessRate;

    private GameObject spriteObject;
    private int stonesAvailable = 1000;

    public Button enhanceButton;
    public Slider successBar;
    public Text successRateText;
    public Text stoneCountText;

    [Header("Success Window")]
    public GameObject successWindow;
    public Text successWindowLevelText;
    public Text ifSuccessText;

    // Use this for initialization
    void Start()
    {
        currentSuccessRate = baseSuccessRate;
        successRateText.text = "Enhancement Success " + currentSuccessRate + "%";
        stoneCountText.text = "Stones: " + stonesAvailable;
        //disable sussess window
        successWindow.SetActive(false);
        successBar.value = 0;
    }

    void Update()
    {
        successBar.value = 0;
        if (enhancementLevel >= 10)
        {
            Debug.Log("Max enhancement level reached");
            enhanceButton.GetComponentInChildren<Text>().text = "Max Level";
            enhanceButton.interactable = false;
        }
        else
        {
            enhanceButton.GetComponentInChildren<Text>().text = "Enhance";
        }
    }

    public void AttemptEnhancement()
    {
        //disable sussess window
        successWindow.SetActive(false);
        int stonesRequired = 1;

        if (enhancementLevel == 1)
        {
            stonesRequired = 2;
        }
        else if (enhancementLevel == 2)
        {
            stonesRequired = 5;
        }
        else if (enhancementLevel >= 3)
        {
            stonesRequired = 7;
        }

        if (stonesAvailable < stonesRequired)
        {
            Debug.Log(stonesRequired);
            Debug.Log("Not enough stones to perform enhancement");
            return;
        }

        stonesAvailable -= stonesRequired;
        stoneCountText.text = "Stones: " + stonesAvailable;
        StartCoroutine(RollForEnhancement());
    }

    IEnumerator RollForEnhancement()
    {
        float time = 0f;
        while (time < 3f)
        {
            enhanceButton.GetComponentInChildren<Text>().text = "Enhancing...";
            successBar.value = 0;
            //enhanceButton.interactable = false;
            time += Time.deltaTime;
            successBar.value = (time / 3) * 100;
            yield return null;
        }

        int randomValue = Random.Range(0, 100);
        successBar.value = randomValue;
        if (enhancementLevel >= 10)
        {
            Debug.Log("Max enhancement level reached");
            yield break;
        }
        if (randomValue < currentSuccessRate)
        {
            successWindow.SetActive(true);
            successWindowLevelText.text = "Level " + (enhancementLevel + 1);
            ifSuccessText.text = "Success!";
            enhancementLevel++;

            currentSuccessRate = baseSuccessRate - (5 * enhancementLevel);
            successRateText.text = "Enhancement Success " + currentSuccessRate + "%";
            enhancementAttempts = 0;
            Debug.Log("Success to +" + enhancementLevel);
        }
        else
        {
            enhancementAttempts++;
            if (enhancementLevel == 0)
            {
                successWindow.SetActive(true);
                successWindowLevelText.text = "Still at Level " + (enhancementLevel);
                ifSuccessText.text = "Failed!!!";
                Debug.Log("Cannot downgrade further");
                yield break;
            }
            else if (enhancementLevel >= 3)
            {
            successWindow.SetActive(true);
            successWindowLevelText.text = "Item Downgraded to Level " + (enhancementLevel - 1);
            ifSuccessText.text = "Failed!!!";
                enhancementLevel--;
                currentSuccessRate =
                    baseSuccessRate
                    - (5 * enhancementLevel)
                    + (Random.Range(5 * enhancementAttempts, 2 * enhancementAttempts));
                Debug.Log("Downgraded to +" + enhancementLevel);
                successRateText.text = "Enhancement Success " + currentSuccessRate + "%";
                Debug.Log(enhancementAttempts);
            }
            else
            {

            successWindow.SetActive(true);
            successWindowLevelText.text = "Still at Level " + (enhancementLevel);
            ifSuccessText.text = "Failed!!!";

                enhancementAttempts++;
                currentSuccessRate =
                    baseSuccessRate
                    - (5 * enhancementLevel)
                    + (Random.Range(5 * enhancementAttempts, 2 * enhancementAttempts));
                Debug.Log("Downgraded to +" + enhancementLevel);
                successRateText.text = "Enhancement Success " + currentSuccessRate + "%";
                Debug.Log(enhancementAttempts);
            }
        }
        
        //enhanceButton.interactable = true;
    }

    public void OpenEnhance()
    {
        enhanceMenu.SetActive(true);
        GameManager.instance.enhanceMenuOpen = true;
        stoneCountText.text = GameManager.instance.currentStones.ToString() + " Stones";
    }
}
