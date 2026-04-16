using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStats;

    // 没有获取就直接将组件先抢过来
    private void Awake()
    {
        EnsureAnimReferenced();
    }

    // 2. 封装一个方法，确保 anim 不为空
    private void EnsureAnimReferenced()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        activationStats = true;

        // 3. 在使用前再次确保组件已被引用（防止 LoadData 在 Awake 之前触发的极端情况）
        EnsureAnimReferenced();

        if(activationStats==false)
        AudioManager.instance.PlaySFX(25, transform);

        if (anim != null)
        {
            anim.SetBool("active", true);
        }
        else
        {
            Debug.LogError($"物体 {gameObject.name} 上找不到 Animator 组件！");
        }
    }
}