using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;
    public bool activateOnEntry, activateOnStay, activateOnExit;
    

    private bool inArea;

    public float timeBetweenBattles;
    private float betweenBattleCounter;
    public bool deactivateAfterStarting; //if true, the battle starter will be deactivated after starting a battle

    public bool cantFlee;

    [Header("Quest Stuff")]
    public bool shouldCompleteQuest;
    public string questToComplete;

    // Start is called before the first frame update
    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(inArea && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
                StartCoroutine(StartBattleCo());
            }
           
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(activateOnEntry)
            {
                StartCoroutine(StartBattleCo());
                //BattleManager.instance.BattleStart(potentialBattles[Random.Range(0, potentialBattles.Length)]);
            }
            else
            {
                inArea = true;
            }
            inArea = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(activateOnExit)
            {
                StartCoroutine(StartBattleCo());
                //BattleManager.instance.BattleStart(potentialBattles[Random.Range(0, potentialBattles.Length)]);
            }
            else{
                inArea = false;
            }
            
        }
    }


    public IEnumerator StartBattleCo()
    {
        UIFade.instance.FadeToBlack();
        GameManager.instance.battleActive = true;
       
       
       
        int selectedBattle = Random.Range(0, potentialBattles.Length);
        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].expReward;
        BattleManager.instance.rewardGold = potentialBattles[selectedBattle].goldReward;
         yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cantFlee); // spawn enemies
        UIFade.instance.FadeFromBlack();

        if(deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = questToComplete;
    
    }
}



