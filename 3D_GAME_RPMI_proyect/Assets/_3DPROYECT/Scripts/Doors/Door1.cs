using UnityEngine;

public class Door1 : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("isPlayer", true);
        }

        
        
    }

}
