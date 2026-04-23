using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        if (anim != null)
            anim.SetTrigger("fadeOut");
    }

    public void FadeIn()
    {
        if (anim != null)
            anim.SetTrigger("fadeIn");
    }
}