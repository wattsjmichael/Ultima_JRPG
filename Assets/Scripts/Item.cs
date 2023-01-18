using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isItem, isWeapon, isArmor;
    [Header("Item Details")]
    public string itemName;
    public string description;

    public int value;
    public Sprite itemSprite;
    [Header("Item Details")]
    public int amountToChange;
    public bool effectHP, effectMP, effectStr;
    
    public int weaponStrength, armorStrength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];
        if(isItem)
        {
            if (effectHP)
            {
                selectedChar.currentHP += amountToChange;
                Debug.LogError(selectedChar.currentHP);
                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }
            if (effectMP)
            {
                selectedChar.currentMP += amountToChange;
                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }
            if (effectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }
        if(isWeapon)
        {
            if(selectedChar.equippedWeapon != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWeapon);
            }
            selectedChar.equippedWeapon = itemName;
            selectedChar.weaponPower = weaponStrength;
        }
        if(isArmor)
        {
            if (selectedChar.equippedArmor != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor);
            }
            selectedChar.equippedArmor = itemName;
            selectedChar.armorPower = armorStrength;
        }
        GameManager.instance.RemoveItem(itemName);
    }
}
