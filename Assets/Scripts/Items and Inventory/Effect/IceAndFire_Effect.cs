using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Date/Item effect/Ice and Fire")]

public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVecelocity;
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player=PlayerManager.instance.player;

        bool thirdAttack = player.GetComponent<Player>().primaryAttack.comboCounter == 2;
        //仅在攻击到敌人生效，如果能在空击情况下攻击那就更好了
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position,player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVecelocity*player.facingDir,0);

            Destroy(newIceAndFire,10);
        }


    }
}
