using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DragonController3 : MonoBehaviour
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
        dragonHp.value = 1000;
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
        dragonHptxt.text = dragonHp.value.ToString() + "/1000";

        //���
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Flame Attack") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fly Flame Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Scream1"))
        {
            particle.SetActive(true);
            Debug.Log("scream");
        }
        else { particle.SetActive(false); }

        //�巡�� �׾�����
        if (dragonHp.value == 0)
        {
            anim.SetBool("die", true);

            Invoke(nameof(WaitForDie), 5);
        }
        else
        {
            levelup.SetActive(false);
            anim.SetBool("die", false);
        }

    
    }

    void WaitForDie()
    {
        //���� Ŭ����
        SceneManager.LoadScene("GameClear");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "swrod")
        {
            dragonHp.value -= player.getPower();
        }
    }
}
