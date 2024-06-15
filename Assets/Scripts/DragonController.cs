 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    public Slider dragonHp;
    public Text dragonHptxt;

    //플레이어 바라보도록
    public Transform target;
    Vector3 lookDir;

    // 모션
    public Animator anim;

    //불 쏘기
    public GameObject particle;

    PlayerController player;

    public GameObject levelup;


    // Start is called before the first frame update
    void Start()
    {
        dragonHp.value = 500;
        anim = GetComponent<Animator>();
        player = GameObject.Find("FemaleCharacterPBR").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 방향은 노멀라이즈 사용
        lookDir = (target.position - transform.position).normalized;

        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);

        transform.rotation = Quaternion.Lerp(from, to, Time.fixedDeltaTime);

        //hp값
        dragonHptxt.text = dragonHp.value.ToString() + "/500";

        //모션
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fireball Shoot") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fly Fireball Shoot") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Basic Attack"))
        {
            particle.SetActive(true);
            Debug.Log("scream");
        }
        else { particle.SetActive(false); }

        //드래곤 죽었을때
        if (dragonHp.value == 0)
        {
            player.setLevel(2);
            player.setHp(20000);
            anim.SetBool("die", true);
            levelup.SetActive(true);
            Invoke(nameof(WaitForDie), 5);
        }
        else
        {
            anim.SetBool("die", false);
            levelup.SetActive(false);
        }
    }
    void WaitForDie()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "swrod")
        {
            dragonHp.value -= player.getPower();
        }
    }
}
