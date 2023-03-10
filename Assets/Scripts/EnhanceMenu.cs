using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceMenu : MonoBehaviour
{
    public static EnhanceMenu instance;
    public GameObject enhanceMenu;
    public int baseSuccessRate = 50;
    public int enhancementLevel;
    public int enhancementAttempts;
    public int currentSuccessRate;

    public GameObject[] levelSprites;
    public GameObject failurePrefab;

    private GameObject spriteObject;
    private int stonesAvailable;

    public Button enhanceButton;
    public Slider successBar;
    public Text successRateText;
    public Text stoneCountText;
    public Sprite[] placeholderNums;
    public Image startNumImg;
    public Image finishNumImg;
    private int startNum;
    private int finishNum;
    private bool enhancementSuccess;

    // Use this for initialization
    void Start()
    {
        spriteObject = Instantiate(levelSprites[0], transform.position, Quaternion.identity);
        currentSuccessRate = baseSuccessRate;
        successRateText.text = currentSuccessRate + "%";
        stoneCountText.text = "Stones: " + stonesAvailable;
        enhanceButton.onClick.AddListener(AttemptEnhancement);
        // Init enhance Num Img
        startNum = 0;
        finishNum = 1;
        startNumImg.sprite = placeholderNums[startNum];
        finishNumImg.sprite = placeholderNums[finishNum];
    }

    public void AttemptEnhancement()
    {
        int stonesRequired = 1;
        if (enhancementLevel == 1)
        {
            stonesRequired = 2;
        }
        else if (enhancementLevel == 2)
        {
            stonesRequired = 5;
        }
        else if (enhancementLevel == 3)
        {
            stonesRequired = 7;
        }

        if (stonesAvailable < stonesRequired)
        {
            Debug.Log("Not enough stones to perform enhancement");
            return;
        }

        stonesAvailable -= stonesRequired;
        stoneCountText.text = "Stones: " + stonesAvailable;
        StartCoroutine(RollForEnhancement());
    }

    IEnumerator RollForEnhancement()
    {
        Debug.Log(currentSuccessRate);
        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            successBar.value = (time / 3) * 100;
            yield return null;
        }

        int randomValue = Random.Range(0, 100);
        successBar.value = randomValue;
        if (randomValue < currentSuccessRate)
        {
            Debug.Log("Success to +" + (enhancementLevel + 1));
            Destroy(spriteObject);
            spriteObject = Instantiate(
                levelSprites[enhancementLevel + 1],
                transform.position,
                Quaternion.identity
            );
            enhancementLevel++;


            if (enhancementLevel >= 10)
            {
                Debug.Log("Max enhancement level reached");
                yield break;
            }
            currentSuccessRate = baseSuccessRate - (5 * enhancementLevel);
            successRateText.text = currentSuccessRate + "%";
        }
        else
        {
            Destroy(spriteObject);
            spriteObject = Instantiate(failurePrefab, transform.position, Quaternion.identity);
            if (enhancementLevel <= 3)
            {
                Debug.Log("Failed! didnt enhance to " + enhancementLevel);
                spriteObject = Instantiate(
                    levelSprites[enhancementLevel],
                    transform.position,
                    Quaternion.identity
                    
                );
                currentSuccessRate = baseSuccessRate + (5);
                successRateText.text = currentSuccessRate + "%";
                Debug.Log(currentSuccessRate);
               
            }

            else if (enhancementLevel > 3)
            {
                enhancementLevel--;
                Debug.Log("Failed! Downgraded to +" + enhancementLevel);
                spriteObject = Instantiate(
                    levelSprites[enhancementLevel],
                    transform.position,
                    Quaternion.identity
                );
                currentSuccessRate = baseSuccessRate + (5);
                successRateText.text = currentSuccessRate + "%";
                Debug.Log(currentSuccessRate);
            }
        }
        startNum = enhancementLevel;
        finishNum = enhancementLevel + 1;
        // Debug.Log(enhancementLevel);
        startNumImg.sprite = placeholderNums[startNum];
        finishNumImg.sprite = placeholderNums[finishNum];
        successBar.value = 0f;
    }

    public void OpenEnhance()
    {
        enhanceMenu.SetActive(true);
        GameManager.instance.enhanceMenuOpen = true;
        stoneCountText.text = GameManager.instance.currentStones.ToString() + " Stones";
    }
}
