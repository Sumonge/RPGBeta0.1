using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStats;

    // û�л�ȡ��ֱ�ӽ������������
    private void Awake()
    {
        EnsureAnimReferenced();
    }

    // 2. ��װһ��������ȷ�� anim ��Ϊ��
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
        // 3. 使� (���) ǰ �ٴμ��齱���ѱ���ã�����ֹ LoadData �� Awake ֮ǰ����ļ�����
        EnsureAnimReferenced();

        if(activationStats==false)
        {
            activationStats = true;
            AudioManager.instance.PlaySFX(25, null);
        }

        if (anim != null)
        {
            anim.SetBool("active", true);
        }
        else
        {
            Debug.LogError($"���� {gameObject.name} ���Ҳ��� Animator �����");
        }
    }
}