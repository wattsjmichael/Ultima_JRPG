using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject[] windows;
    private CharStats[] playerStats;
    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Text goldText;
    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;
    public ItemButton[] itemButtons;
    public string selectedItem; 
    public Item activeItem; 
    public Text itemName, itemDescription, useButtonText;
    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public GameObject[] statusButtons;
    public Image statusImage;
    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;

    public static GameMenu instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && !Shop.instance.shopMenu.activeInHierarchy)
        {
           if (menu.activeInHierarchy)
           {
            //menu.SetActive(false);
            //GameManager.instance.gameMenuOpen = false;
            CloseMenu();
           }
           else
            {
        //          if (Input.GetKey(KeyCode.K))
        // {
        //     CharStats.instance.AddExp(1000);
        // }
                menu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);
                nameText[i].text = playerStats[i].charName;
                Debug.Log(playerStats[i].charName);
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "LVL: " + playerStats[i].playerLevel;
                expText[i].text = "" + playerStats[i].currentExp + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentExp;
                charImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();
       for(int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        menu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();
        StatusChar(0);
        for(int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    public void StatusChar(int selected)
    {
        
        statusName.text = playerStats[selected].charName;
        statusHP.text =  playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text =  playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text =  playerStats[selected].defense.ToString();
        //statusWpnEqpd.text = "Weapon: " + playerStats[selected].equippedWeapon;
        //statusWpnPwr.text = "Power: " + playerStats[selected].weaponPower;
        //statusArmrEqpd.text = "Armor: " + playerStats[selected].equippedArmor;
        //statusArmrPwr.text = "Power: " + playerStats[selected].armorPower;
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentExp).ToString() + " EXP to Level Up";
        statusImage.sprite = playerStats[selected].charImage;
        if (playerStats[selected].equippedWeapon != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWeapon;
        }
        statusWpnPwr.text = playerStats[selected].weaponPower.ToString();
        if (playerStats[selected].equippedArmor != "")
        {
            statusArmrEqpd.text = playerStats[selected].equippedArmor;
        }
        statusArmrPwr.text = playerStats[selected].armorPower.ToString();
    }

    public void ShowItems()
    {   
        GameManager.instance.SortItems();
        
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;
            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }
        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
      
        }
    }
    
    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        Debug.Log(selectChar);
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }
}

