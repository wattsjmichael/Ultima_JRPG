using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharStats[] playerStats;

    public bool gameMenuOpen,
        dialogActive,
        fadingBetweenAreas,
        shopActive,
        enhanceMenuOpen,
        battleActive;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;
    public int currentStones;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || enhanceMenuOpen || battleActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     AddItem("Iron Sword");
        //     AddItem("Full Health Potion");

        //     RemoveItem("Small HP Potion");
        //     RemoveItem("Excalibur");
        // }

        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     SaveData();
        // }
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     LoadData();
        // }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;
        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }
        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    i = referenceItems.Length;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " does not exist!");
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;
                i = itemsHeld.Length;
            }
        }

        if (foundItem)
        {
            numberOfItems[itemPosition]--;
            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        //save character info
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }

            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_Level",
                playerStats[i].playerLevel
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_currentExp",
                playerStats[i].currentExp
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_CurrentHP",
                playerStats[i].currentHP
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_MaxHP",
                playerStats[i].maxHP
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_CurrentMP",
                playerStats[i].currentMP
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_MaxMP",
                playerStats[i].maxMP
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_Strength",
                playerStats[i].strength
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_defense",
                playerStats[i].defense
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_weaponPower",
                playerStats[i].weaponPower
            );
            PlayerPrefs.SetInt(
                "Player_" + playerStats[i].charName + "_armorPower",
                playerStats[i].armorPower
            );
            PlayerPrefs.SetString(
                "Player_" + playerStats[i].charName + "_equippedWeapon",
                playerStats[i].equippedWeapon
            );
            PlayerPrefs.SetString(
                "Player_" + playerStats[i].charName + "_equippedArmor",
                playerStats[i].equippedArmor
            );
            //save current gold
        }

        //store inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
        PlayerPrefs.SetInt("Current_Gold", currentGold);
        //save current stones
        PlayerPrefs.SetInt("Current_Stones", currentStones);
        Debug.LogError(currentGold);
        Debug.LogError(currentStones);
    }

    public void LoadData()
    {
        PlayerController.instance.transform.position = new Vector3(
            PlayerPrefs.GetFloat("Player_Position_x"),
            PlayerPrefs.GetFloat("Player_Position_y"),
            PlayerPrefs.GetFloat("Player_Position_z")
        );

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_Level"
            );
            playerStats[i].currentExp = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_currentExp"
            );
            playerStats[i].currentHP = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_CurrentHP"
            );
            playerStats[i].maxHP = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_MaxHP"
            );
            playerStats[i].currentMP = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_CurrentMP"
            );
            playerStats[i].maxMP = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_MaxMP"
            );
            playerStats[i].strength = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_Strength"
            );
            playerStats[i].defense = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_defense"
            );
            playerStats[i].weaponPower = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_weaponPower"
            );
            playerStats[i].armorPower = PlayerPrefs.GetInt(
                "Player_" + playerStats[i].charName + "_armorPower"
            );
            playerStats[i].equippedWeapon = PlayerPrefs.GetString(
                "Player_" + playerStats[i].charName + "_equippedWeapon"
            );
            playerStats[i].equippedArmor = PlayerPrefs.GetString(
                "Player_" + playerStats[i].charName + "_equippedArmor"
            );
        }
        GameManager.instance.currentGold = PlayerPrefs.GetInt("Current_Gold");
        Debug.LogError(PlayerPrefs.GetInt("Current_Gold").ToString() + "g");
        Debug.LogError(PlayerPrefs.GetInt("Current_Stones").ToString() + "s");

        //load current stones
        GameManager.instance.currentStones = PlayerPrefs.GetInt("Current_Stones");

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }

        //load current gold
    }
}
