﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public Text spellNameText;
    public Text spellCostText;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void Press()
    {
        if (
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP
            >= spellCost
        )
        {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -=
                spellCost;
        }
        else
        {

            
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.battleNotice.theText.text = "Not enough MP!";
            BattleManager.instance.battleNotice.Activate();
        }
    }
}
