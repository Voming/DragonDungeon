using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonController2 : MonoBehaviour
{
    public Slider dragonHp;
    public Text dragonHptxt;

    //�÷��̾� �ٶ󺸵���
    public Transform target;
    Vector3 lookDir;

    // ���
    public Animator anim;

    //�� ���
    public GameObject particle;

    PlayerController player;

    public GameObject levelup;

    void Start()
    {
        dragonHp.value = 800;
        anim = GetComponent<Animator>();
        player = GameObject.Find("FemaleCharacterPBR").GetComponent<PlayerController>();
    }
     
    void Update()
    {
        // ������ ��ֶ����� ���
        lookDir = (target.position - transform.position).normalized;

        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);

        transform.rotation = Quaternion.Lerp(from, to, Time.fixedDeltaTime);

        //hp��
        dragonHptxt.text = dragonHp.value.ToString() + "/800";

        //���
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream1") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream3"))
        {
            particle.SetActive(true);
            Debug.Log("scream");
        }
        else { particle.SetActive(false); }

        //�巡�� �׾�����
        if (dragonHp.value == 0)
        {
            player.setLevel(3);
            player.setHp(30000);
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
