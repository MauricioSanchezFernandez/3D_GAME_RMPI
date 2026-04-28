using UnityEngine;

public class Door2 : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.Play("Open_BigDoor2_Anim");
    }

}
