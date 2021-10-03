using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
    // Start is called before the first frame update
    Animator myAnimator;
    [SerializeField] float speed;
    [SerializeField] BoxCollider2D pies;
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite fallingSprite;
    [SerializeField] Rigidbody2D myBody;
    [SerializeField] float jumpSpeed;

    SpriteRenderer myRenderer;
    BoxCollider2D myCollider;
    int jump;
    float dash;
    bool pressZ;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        dash = 0;
        pressZ = true;
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Saltar();
        Falling();
        Fire();
        Dash();
    }

    void Fire()
    {
        if (Input.GetKey(KeyCode.X))
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    void Mover()
    {
        float mov = Input.GetAxis("Horizontal");
        if (mov != 0)
        {
            myAnimator.SetBool("running", true);
            transform.localScale = new Vector2(Mathf.Sign(mov), 1);
            transform.Translate(new Vector2(mov * speed * Time.deltaTime, 0));
        }
        else
        {
            myAnimator.SetBool("running", false);
            
        }
    }
    void Saltar()
    {
        
        /*
        si no usaramos animaciones:
        if (isGrounded)
        {
            myRenderer.sprite = idleSprite;
        }
        else
        {
            myRenderer.sprite = fallingSprite;
        }*/

        if (isGrounded() && !myAnimator.GetBool("jumping"))
        {
            myAnimator.SetBool("falling", false);
            myAnimator.SetBool("jumping", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myAnimator.SetTrigger("takeof");
                myAnimator.SetBool("jumping", true);
                if(myAnimator.GetBool("dash")){
                    myBody.AddForce(new Vector2(0, jumpSpeed+ jumpSpeed/2), ForceMode2D.Impulse);
                }
                else{
                    myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                }
                jump = 1;
            }
            
        }
        if(myAnimator.GetBool("jumping") && !isGrounded()){
            if(myAnimator.GetBool("jumping") && jump == 1 && Input.GetKeyDown(KeyCode.Space)){
                myAnimator.SetTrigger("takeof");
                myAnimator.SetBool("jumping", true);
                myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                jump = 0;
            }
        }
    }

    bool isGrounded()
    {
        RaycastHit2D myRaycast = Physics2D.Raycast(myCollider.bounds.center, Vector2.down, myCollider.bounds.extents.y + 0.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(myCollider.bounds.center, new Vector2(0, (myCollider.bounds.extents.y + 0.2f) * -1), Color.cyan);
        return myRaycast.collider != null;
        // return pies.IsTouchingLayers(LayerMask.GetMask("Ground"));

    }

    void AfterTakeOfEvent()
    {
        myAnimator.SetBool("jumping", false);
        myAnimator.SetBool("falling", true);
    }

    void Falling()
    {
        if (myBody.velocity.y < 0 && !myAnimator.GetBool("jumping"))
        {
            myAnimator.SetBool("falling", true);
        }
    }

    void Dash(){

        if (Input.GetKey(KeyCode.Z) && dash <= 0.3 && pressZ == true){            
            myAnimator.SetBool("dash", true);
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
            dash = dash + 0.5f * Time.deltaTime;
            Debug.Log(dash);
            if(dash >= 0.3f){
                pressZ = false;
                dash = 1;
            }
        }
        else{
            if(dash > 0){
            dash = dash - 1 * Time.deltaTime;
            Debug.Log(dash);
            myAnimator.SetBool("dash", false);
            }
            if(dash <= 0){
                pressZ = true;
            }
        }     
        
    }
}
