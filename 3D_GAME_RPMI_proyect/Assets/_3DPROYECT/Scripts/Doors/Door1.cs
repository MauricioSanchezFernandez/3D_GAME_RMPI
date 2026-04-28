using UnityEngine;

public class Door1 : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.Play("Open_BigDoor1_Anim");
    }

}
