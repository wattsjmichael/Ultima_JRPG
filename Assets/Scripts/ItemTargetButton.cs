﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTargetButton : MonoBehaviour
{

     public string itemName;
    public int activeItemTarget;
    public Text targetName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void Press()
    {
        BattleManager.instance.UseItem();
    }
}
