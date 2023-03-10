using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    private bool justStarted;

    public string[] dialogLines;
    public int currentLine;
    public static DialogManager instance;

    private string questToMark;
    private bool markQuestComplete;
    
    private bool shouldMarkQuest;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //dialogText.text = dialogLines[currentLine];
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (!justStarted)
                {
                currentLine++;
                
                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                        GameManager.instance.dialogActive = false;

                        if (shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            QuestManager.instance.MarkQuestComplete(questToMark);
                        }
                    }
                    else
                    {
                        CheckIfName();
                        dialogText.text = dialogLines[currentLine];
                    }
                } else
                {
                    justStarted = false;
                }
            }

            }
        }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;
        currentLine = 0;
        CheckIfName();
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        justStarted = true;
        nameBox.SetActive(isPerson);
        GameManager.instance.dialogActive = true;

    }

    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameBox.SetActive(true);
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
            dialogText.text = dialogLines[currentLine];
        }
    }

    public void ShouldActivateQuestAtEnd(string questToMark, bool markComplete)
    {
        this.questToMark = questToMark;
        this.markQuestComplete = markComplete;
        shouldMarkQuest = true;

    }
}
    

