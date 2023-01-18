using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;
    public int playerLevel = 1;
    public int currentExp;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseExp = 1000;



    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int strength;
    public int defense;
    public int weaponPower;
    public int armorPower;
    public string equippedWeapon;
    public string equippedArmor;
    public Sprite charImage;

    public static CharStats instance;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseExp;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.08f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            AddExp(1000);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentExp += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentExp >= expToNextLevel[playerLevel])
            {
                currentExp -= expToNextLevel[playerLevel];

                playerLevel++;

                //Add Random Range for the Stat Points
                 strength += Random.Range(0, 5);
                 defense += Random.Range(0, 5);
                 maxHP += Mathf.FloorToInt(Random.Range(0, 5)*10.5f);
                 Debug.Log(maxHP);
                    maxMP += Mathf.FloorToInt(Random.Range(0, 5)*3.5f);
                    currentHP = maxHP;
                    currentMP = maxMP;
             
            }
        }

        if (playerLevel >= maxLevel)
        {
            currentExp = 0;
        }

       
    }
}
