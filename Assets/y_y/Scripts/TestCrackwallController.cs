using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrackwallController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(this.gameObject);
        }
    }
}
