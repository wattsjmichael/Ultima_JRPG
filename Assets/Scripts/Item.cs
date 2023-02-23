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

        ;
        if (BattleManager.instance.battleActive)
        {
            charToUseOn = BattleManager.instance.currentActiveBattler;
            
            
           
            
        }

        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];
        
        
        if (isItem)
        {
            if (selectedChar.currentHP != selectedChar.maxHP)
            {
                if (effectHP)
                {
                    selectedChar.currentHP += amountToChange;
                    if (selectedChar.currentHP > selectedChar.maxHP)
                    {
                        selectedChar.currentHP = selectedChar.maxHP;
                    }
 
                    if (BattleManager.instance.battleActive)
                    {
                        charToUseOn = BattleManager.instance.currentActiveBattler;
                        BattleManager.instance.activeBattlers[charToUseOn].currentHP += amountToChange;
                        if (BattleManager.instance.activeBattlers[charToUseOn].currentHP > selectedChar.maxHP)
                        {
                            BattleManager.instance.activeBattlers[charToUseOn].currentHP = selectedChar.maxHP;
                        }
                    }
                }
 
                GameManager.instance.RemoveItem(itemName);
            }
 
            if (selectedChar.currentMP != selectedChar.maxMP)
            {
                if (effectMP)
                {
                    selectedChar.currentMP += amountToChange;
                    if (selectedChar.currentMP > selectedChar.maxMP)
                    {
                        selectedChar.currentMP = selectedChar.maxMP;
                    }
 
                    if (BattleManager.instance.battleActive)
                    {
                        charToUseOn = BattleManager.instance.currentActiveBattler;
                        BattleManager.instance.activeBattlers[charToUseOn].currentMP += amountToChange;
                        if (BattleManager.instance.activeBattlers[charToUseOn].currentMP > selectedChar.maxMP)
                        {
                            BattleManager.instance.activeBattlers[charToUseOn].currentMP = selectedChar.maxMP;
                        }
                    }
 
                    GameManager.instance.RemoveItem(itemName);
                }
            }
 
            if (effectStr)
            {
                selectedChar.strength += amountToChange;
 
                GameManager.instance.RemoveItem(itemName);
            }
        }
 
        if (isWeapon)
        {
            if (selectedChar.equippedWeapon != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWeapon);
            }
 
            selectedChar.equippedWeapon = itemName;
            selectedChar.weaponPower = weaponStrength;
 
            GameManager.instance.RemoveItem(itemName);
        }
 
        if (isArmor)
        {
            if (selectedChar.equippedArmor != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor);
            }
 
            selectedChar.equippedArmor = itemName;
            selectedChar.armorPower = armorStrength;
 
            GameManager.instance.RemoveItem(itemName);
        }
    }


    
  

}