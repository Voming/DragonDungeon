using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    // 이동
    float speed = 17f;
    float rotatespeed = 1f;
    float jumpPower = 5f;
    Rigidbody rigidbody;
    Vector3 movement;
   
    float horizontalMove;
    float verticalMove;
    bool isJumping;

    // 체력
    public Slider playerHp;
    public Text playerHptxt;
    static int hp = 10000;  //100으로 나눌거임

    // 모션
    public Animator anim;

    //공격력
    static int power = 50;

    //레벨
    static int level = 1;
    public Text leveltxt;

    //효과음
    public AudioClip backgrd;
    public AudioClip battle;
    public AudioClip attack;

    AudioSource audio;

    //드래곤 hp 
    public GameObject dragonHp;
    public GameObject dragonHptxt;
    public GameObject dragonHp2;
    public GameObject dragonHptxt2;
    public GameObject dragonHp3;
    public GameObject dragonHptxt3;

    public GameObject dragon2;
    public GameObject dragon3;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        leveltxt.text = "Level : 1";

        playerHp.value = hp;

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //게임 오버
        if (hp <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        //상태바 위치 지정
        playerHp.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.9f, 0));
        playerHp.value = hp;

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        // 모션 변경
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("moveFWD", true);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("moveBWD", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("moveR", true);
        }
        else
        {
            anim.SetBool("moveFWD", false);
            anim.SetBool("moveBWD", false);
            anim.SetBool("moveR", false);
            
        }

        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("jump", true);
        } 
        else 
            anim.SetBool("jump", false);
      
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("attack", true);
            this.audio.PlayOneShot(this.attack);
        }
        else 
            anim.SetBool("attack", false);

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("defend", true);
        }
        else
            anim.SetBool("defend", false);
       
        //레벨
        leveltxt.text = "Level : " + level.ToString();

        if (level == 1)
        {
            power = 50;
            //상태 택스트 적용
            int hptxt = Mathf.FloorToInt(hp / 100);
            playerHptxt.text = hptxt.ToString() + "/100";
        }
        else if (level == 2)
        {
            power = 70;
            //상태 택스트 적용
            int hptxt = Mathf.FloorToInt(hp / 100);
            playerHptxt.text = hptxt.ToString() + "/200";
            playerHp.maxValue = 20000;
        }
        else if (level == 3)
        {
            power = 100;
            //상태 택스트 적용
            int hptxt = Mathf.FloorToInt(hp / 100);
            playerHptxt.text = hptxt.ToString() + "/300";
            playerHp.maxValue = 30000;
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "dragon")
        {
            hp -= 1000;
        }
        //1번 드래곤
        if (collision.gameObject.tag == "enter")
        {
            dragonHp.SetActive(true);
            dragonHptxt.SetActive(true);
            audio.clip = battle;
            audio.Play();
        }
        else if (collision.gameObject.tag == "exit")
        {
            dragonHp.SetActive(false);
            dragonHptxt.SetActive(false);
            audio.clip = backgrd;
            dragon2.SetActive(true);
            audio.Play();
        }
        //2번 드래곤
        if (collision.gameObject.tag == "enter2")
        {
            dragonHp2.SetActive(true);
            dragonHptxt2.SetActive(true);
            audio.clip = battle;
            audio.Play();
        }
        //3번 드래곤
        if (collision.gameObject.tag == "enter3")
        {
            dragonHp2.SetActive(false);
            dragonHptxt2.SetActive(false);
            dragonHp3.SetActive(true);
            dragonHptxt3.SetActive(true);
            dragon3.SetActive(true);
        }
    }

    public int getHp()
    {
        return hp;
    }
    public int getPower()
    {
        return power;
    }
    public int getLevel()
    {
        return level;
    }

    public void setHp(int dhp)
    {
        hp = dhp;
    }
    public void setPower(int dpower)
    {
        power = dpower;
    }
    public void setLevel(int dlevel)
    {
        level = dlevel;
    }




    //불에 맞으면 데미지
    public void onDamaged()
    {
        hp -= 5;
    }
    public void onDamaged2()
    {
        hp -= 50;
    }
    public void onDamaged3()
    {
        hp -= 100;
    }

    private void FixedUpdate()
    {
        Run();
        Jump();
    }

    void Run()
    {
        movement.Set(horizontalMove, 0, verticalMove);
        movement = transform.rotation * movement.normalized * speed * Time.deltaTime;  //transform.rotation으로 이동방향과 보는 방향 맞추기

        // 바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        if (!(horizontalMove == 0 && verticalMove == 0))
        {
            // 이동과 회전을 함께 처리
            transform.position += movement * speed * Time.deltaTime;
            // 회전하는 부분. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotatespeed);
        }
       
    }

    void Jump()
    {
        if (!isJumping)
            return;

        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        isJumping = false;
    }
}

