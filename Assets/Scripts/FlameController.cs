using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    public GameObject Gplayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == Gplayer)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.onDamaged();
            }
        }
    }
}
