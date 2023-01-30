using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer : MonoBehaviour
{
    private bool canOpen;

    public string[] itemsForSale = new string[40];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !EnhanceMenu.instance.enhanceMenu.activeInHierarchy)
        {
           
            EnhanceMenu.instance.OpenEnhance();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

     void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = false;
        }
    }


}
