using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
       public Button[] options;
    private int selectedOption = 0;

    private void Update()
    {
        float vertical = Input.GetAxis("Vertical");

        if (vertical > 0)
        {
            selectedOption--;
            if (selectedOption < 0)
            {
                selectedOption = options.Length - 1;
            }
        }
        else if (vertical < 0)
        {
            selectedOption++;
            if (selectedOption >= options.Length)
            {
                selectedOption = 0;
            }
        }

        if (Input.GetButtonDown("Submit"))
        {
            options[selectedOption].onClick.Invoke();
        }

        for (int i = 0; i < options.Length; i++)
        {
            if (i == selectedOption)
            {
                options[i].Select();
            }
            else
            {
                options[i].OnDeselect(null);
            }
        }
    }
}