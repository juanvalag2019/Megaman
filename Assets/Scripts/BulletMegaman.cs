using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMegaman : MonoBehaviour
{
    Animator myAnimator;
    public bool direction;
    private float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (direction)
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        else
            transform.Translate(new Vector2(-speed * Time.deltaTime, 0));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            myAnimator.SetBool("explode", true);
        }
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}