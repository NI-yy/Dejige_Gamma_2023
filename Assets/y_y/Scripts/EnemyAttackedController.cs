using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackedController : MonoBehaviour
{
    //yyÒW_vC[ÌU(ñ)Ş½Á½êÁĊ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireAttack"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("OrangeAttack"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("WaterAttack"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("LeafAttack"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("ElectricAttack"))
        {
            Destroy(this.gameObject);
        }
    }
}
