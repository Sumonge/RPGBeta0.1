using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    // 修改点：将 Start 改为 Awake
    private void Awake()
    {
        anim = GetComponent<Animator>();

        // 安全检查：如果还是空，说明你这个脚本挂载的对象上真的没有 Animator 组件
        if (anim == null)
        {
            Debug.LogError($"{gameObject.name} 上找不到 Animator 组件！");
        }
    }

    public void FadeOut()
    {
        if (anim != null) // 加个保险
            anim.SetTrigger("fadeOut");
    }

    public void FadeIn()
    {
        if (anim != null) // 加个保险
            anim.SetTrigger("fadeIn");
    }
}