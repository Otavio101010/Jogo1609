using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class charactr : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private bool podePular = true;

    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        podePular = true;
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        jump();
        
        if(Input.GetKeyDown(KeyCode.R)){
			Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
		}
    }

    void move()
    {
        Vector3 moviment = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += moviment * Time.deltaTime * speed;

    }

    void jump()
    {
        if (Input.GetKey(KeyCode.Space) && podePular){
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            podePular = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Ground")){
            podePular=true;
        }
    }

}

