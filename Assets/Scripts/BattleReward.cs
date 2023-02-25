using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleReward : MonoBehaviour
{

    public static BattleReward instance;
    public Text xpText, itemText, goldText;
    public GameObject rewardScreen;

    public string[] rewardsItems;
    public int xpEarned;

    public int goldEarned;

    public bool markQuestComplete;
    public string questToMark;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(100, new string[] { "Potion", "Iron Sword", "Potion" }, 650);
        }
    }

    public void OpenRewardScreen(int xp, string[] rewards, int gold)
    {
        goldEarned = gold; 
        xpEarned = xp; // xp earned
        rewardsItems = rewards; // array of items

        xpText.text = "Everyone earned: " + xpEarned + " xp!"; // xp text
        itemText.text = ""; // item text

        //earn gold
         GameManager.instance.currentGold += goldEarned; // add gold to current gold
         goldText.text = goldEarned + "g"; // gold text


        for (int i = 0; i < rewardsItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";

        }

        rewardScreen.SetActive(true);
    }

    public void CloseReqwardScreen()
    {
        for (int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            
            if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                Debug.Log("Player " + i + " earned " + xpEarned + " xp!");
                GameManager.instance.playerStats[i].AddExp(xpEarned);
                Debug.Log("Player " + i + " now has " + CharStats.instance.currentExp + " xp!");
                
            }
            
        }

        for (int i = 0; i < rewardsItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardsItems[i]);
        }
        

        rewardScreen.SetActive(false);
        GameManager.instance.battleActive = false;

        if(markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
