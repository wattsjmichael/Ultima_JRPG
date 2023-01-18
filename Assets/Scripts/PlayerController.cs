using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float dashSpeed;

    public bool canMove = true;

    public Animator playerAnim;

    public static PlayerController instance;

    public string areaTransitionName;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;


    

    // Start is called before the first frame update
    void Start()
    {
        if (instance  == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject); 
            }
        }

        DontDestroyOnLoad(gameObject);
    }   

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
             rb.velocity = new Vector2(
                        Input.GetAxisRaw("Horizontal"),
                        Input.GetAxisRaw("Vertical")
                    ).normalized * moveSpeed;
                    
                    
                    if (Input.GetKey(KeyCode.Space))
                    {
                        rb.velocity = new Vector2(
                        Input.GetAxisRaw("Horizontal"),
                        Input.GetAxisRaw("Vertical")
                    ).normalized * dashSpeed;
                    }
        } 
        else
        
        {
            rb.velocity = Vector2.zero;
        }


        playerAnim.SetFloat("moveX", rb.velocity.x);
        playerAnim.SetFloat("moveY", rb.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == 1 )
        {
            if (canMove)
            {
                playerAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                playerAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

          transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
            transform.position.z);
    }

        public void SetBounds (Vector3 botLeft, Vector3 topRight)
    {
       bottomLeftLimit = botLeft + new Vector3(.5f, .5f, 0f);
       topRightLimit = topRight + new Vector3(-.5f, -.5f, 0f);
    }
   
}
