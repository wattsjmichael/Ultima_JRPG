using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonSelect : MonoBehaviour
{
    public string itemName;
    
    public Text itemNameText;


    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void Press()
    {
        BattleManager.instance.itemMenu.SetActive(false); // Hide the item menu
        BattleManager.instance.OpenItemTargetMenu(itemName); // Open the item target menu
    }
}