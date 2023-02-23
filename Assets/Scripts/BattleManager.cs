using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public string gameOverScene;

    public bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentActiveBattler;

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;

    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;

    public Text[] playerNames,
        playerHP,
        playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;

    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 35;

    [Header("Item Menu")]
    public GameObject itemMenu;
    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName;
    public Text itemDescription;
    public Text useButtonText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            BattleStart(new string[] { "Goblin" });
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                    //UIManager.instance.playerHUD.SetHUD(activeBattlers[currentTurn]);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);
                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!battleActive)
        {
            battleActive = true;
            GameManager.instance.battleActive = true;

            transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                transform.position.z
            );
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (
                            playerPrefabs[j].charName
                            == GameManager.instance.playerStats[i].charName
                        )
                        {
                            BattleChar newPlayer = Instantiate(
                                playerPrefabs[j],
                                playerPositions[i].position,
                                playerPositions[i].rotation
                            );
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].wpnPwr = thePlayer.weaponPower;
                            activeBattlers[i].armrPwr = thePlayer.armorPower;
                        }
                    }
                }
            }
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(
                                enemyPrefabs[j],
                                enemyPositions[i].position,
                                enemyPositions[i].rotation
                            );
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);

            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }
        Debug.Log(activeBattlers[currentTurn].charName + "'s turn");
        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            Debug.Log(activeBattlers[i].currentHP);
            if (activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if (activeBattlers[i].currentHP == 0)
            {
                //handle dead battler
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                Debug.Log("Victory");
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in defeat
                Debug.Log("Defeat");
                StartCoroutine(GameOverCo());
            }

            // battleScene.SetActive(false);
            // GameManager.instance.battleActive = false;
            // battleActive = false;
        }
        else
        {
            while (activeBattlers[currentTurn].currentHP == 0) //skip dead battlers
            {
                currentTurn += Random.Range(1, activeBattlers.Count);
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0; //loop back to start
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1.5f);
        EnemyAttack();
        yield return new WaitForSeconds(1.5f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0) //
            {
                players.Add(i); //add player to list
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)]; //random target

        //activeBattlers[selectedTarget].currentHP -= 30; //sample damage

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(
                    movesList[i].effect,
                    activeBattlers[selectedTarget].transform.position,
                    activeBattlers[selectedTarget].transform.rotation
                );
                movePower = movesList[i].movePower;
            }
        }
        Instantiate(
            enemyAttackEffect,
            activeBattlers[currentTurn].transform.position,
            activeBattlers[currentTurn].transform.rotation
        );
        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPwr;
        float defPwr = activeBattlers[target].defense + activeBattlers[target].armrPwr;

        // select a random number between 0 and 1
        float critChance = Random.Range(0f, 1f);
        float critMultiplier = 2.0f;
        if (critChance <= .05f) //5% chance of critical hit
        {
            float critCalc =
                (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f) * critMultiplier;

            int critDamToGive = Mathf.RoundToInt(critCalc);
            Debug.Log(
                activeBattlers[currentTurn].charName
                    + " is dealing "
                    + critDamToGive
                    + " CRITICAL damage to "
                    + activeBattlers[target].charName
            );

            activeBattlers[target].currentHP -= critDamToGive; //critical hit
            Instantiate(
                    theDamageNumber,
                    activeBattlers[target].transform.position,
                    activeBattlers[target].transform.rotation
                )
                .SetDamage(critDamToGive);

            UpdateUIStats();
        }
        else
        {
            float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);

            int damageToGive = Mathf.RoundToInt(damageCalc);
            Debug.Log(
                activeBattlers[currentTurn].charName
                    + " is dealing "
                    + damageToGive
                    + " damage to "
                    + activeBattlers[target].charName
            );

            activeBattlers[target].currentHP -= damageToGive;
            Instantiate(
                    theDamageNumber,
                    activeBattlers[target].transform.position,
                    activeBattlers[target].transform.rotation
                )
                .SetDamage(damageToGive);

            UpdateUIStats(); //update UI
        }
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerNames[i].gameObject.SetActive(true);
                    playerNames[i].text = playerData.charName;
                    playerHP[i].text =
                        Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text =
                        Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP; //clamp to prevent negative values
                    GameManager.instance.playerStats[i].currentHP = playerData.currentHP; //update player stats
                    GameManager.instance.playerStats[i].currentMP = playerData.currentMP; //update player stats
                }
                else
                {
                    playerNames[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerNames[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName) //find move in list
            {
                Instantiate(
                    movesList[i].effect,
                    activeBattlers[selectedTarget].transform.position,
                    activeBattlers[selectedTarget].transform.rotation
                );
                movePower = movesList[i].movePower;
            }
        }

        // Instantiate(
        //     theDamageNumber,
        //     activeBattlers[selectedTarget].transform.position,
        //     activeBattlers[selectedTarget].transform.rotation
        // ).SetDamage(movePower); //damage number

        DealDamage(selectedTarget, movePower); //deal damage

        Instantiate(
            enemyAttackEffect,
            activeBattlers[currentTurn].transform.position,
            activeBattlers[currentTurn].transform.rotation
        ); //attack effect

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        //open menu
        targetMenu.SetActive(true);
        //populate menu
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer) //if not player
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenItemTargetMenu(string moveName)
    {
        //open menu
        targetMenu.SetActive(true);
        //populate menu
        List<int> Players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer) //if player
            {
                Players.Add(i);
            }
        }

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (Players.Count > i)
            {
                itemButtons[i].gameObject.SetActive(true);
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu() //open magic menu
    {
        magicMenu.SetActive(true);
        for (int i = 0; i < magicButtons.Length; i++) //populate menu
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i) //if move is available
            {
                magicButtons[i].gameObject.SetActive(true); //activate button
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i]; //set spell name
                magicButtons[i].spellNameText.text = magicButtons[i].spellName; //set spell name text

                for (int j = 0; j < movesList.Length; j++) //find spell in list
                {
                    if (movesList[j].moveName == magicButtons[i].spellName) //if spell name matches
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost; //set spell cost
                        magicButtons[i].spellCostText.text = magicButtons[i].spellCost.ToString(); //set spell cost text
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee() //flee
    {
        int fleeSuccess = Random.Range(0, 100);
        if (fleeSuccess < chanceToFlee)
        {
            //end battle
            battleActive = false;
            battleScene.SetActive(false);
            StartCoroutine(EndBattleCo()); //end battle coroutine
        }
        else
        {
            NextTurn();
            battleNotice.theText.text = "Couldn't escape!";
            battleNotice.Activate();
        }
    }

    public void OpenItemMenu()
    {
        GameManager.instance.SortItems();
        itemMenu.SetActive(true);
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
                itemButtons[i].buttonImage.sprite = GameManager.instance
                    .GetItemDetails(GameManager.instance.itemsHeld[i])
                    .itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item selectedItem)
    {
        activeItem = selectedItem;
        if (selectedItem.isItem)
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

    public void UseItem()
    {
        activeItem.Use(currentActiveBattler);

        //Select Current Active Battler
        Debug.LogError(currentActiveBattler + " is the current active battler");
        GameManager.instance.SortItems();
        UpdateUIStats();

        battleNotice.theText.text = "Used " + activeItem.itemName + "!";
        battleNotice.Activate();
        CloseItemMenu();
        NextTurn();
    }

    public void CloseItemMenu()
    {
        itemMenu.SetActive(false);
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName) //if active battler name matches player name
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP; //set current hp to active battler hp
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP; //set current mp to active battler mp
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }
        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        GameManager.instance.battleActive = false;

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().bgmToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }
}
