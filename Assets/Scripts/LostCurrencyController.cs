
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    // Start is called before the first frame update
    public int currency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()!=null)
        {
            PlayerManager.instance.currency+=currency;
            Destroy(this.gameObject);
        }
    }

}
