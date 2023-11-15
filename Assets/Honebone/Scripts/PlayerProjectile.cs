using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    PlayerProjectorData projectorData;
    Transform tf;
    //protected Player player;
    //Transform playerTF;
    //protected Vector3 playerPos;
    //Vector3 playerPosDiff;
    float projectileSpeed;
    //float followPlayerSpeed;

    bool disabled;
    public void Init(PlayerProjectorData data,Color color)
    {
        projectorData = data;
        //player = p;
        //playerTF = player.GetComponent<Transform>();
        //playerPos = playerTF.position;

        projectileSpeed = Random.Range(projectorData.projectileSpeed_min, projectorData.projectileSpeed_max);
        //followPlayerSpeed = data.followPlayerSpeed;

        tf = transform;

        GetComponent<SpriteRenderer>().color = color;

        StartCoroutine(CountDown());
    }
    void FixedUpdate()
    {
        //if (followPlayerSpeed > 0)//�ǔ��e
        //{
        //    if (projectorData.followCurrentPlayer)//���݂̃v���C���[�̈ʒu��ǔ�����ꍇ�́A�v���C���[�̈ʒu����ɍX�V
        //    {
        //        playerPos = playerTF.position;
        //    }
        //    playerPosDiff = (playerPos - tf.position);
        //    Vector2 dis = playerPosDiff;
        //    if (dis.magnitude < 0.5f) { followPlayerSpeed = 0; }//�v���C���[�̈ʒu�ɓ���������ǔ���~

        //    float rot = (Mathf.Atan2(playerPosDiff.y, playerPosDiff.x) * Mathf.Rad2Deg) - tf.localEulerAngles.z - 90;
        //    if (rot < -180) { rot += 360; }
        //    tf.Rotate(0, 0, Mathf.Clamp(rot, followPlayerSpeed * -0.5f, followPlayerSpeed * 0.5f));
        //}

        tf.Translate(Vector3.up * projectileSpeed / 33f);
    }

    private void OnTriggerEnter2D(Collider2D collision)//��_��������
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().HitAttack();
        }
    }

    void DestroyPJTL(bool expired)
    {
        disabled = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        AtTheEnd(expired);


        Destroy(gameObject, 1f);
    }
    /// <summary>expired:���Ԑ؂�ɂ��j��</summary>
    public virtual void AtTheEnd(bool expired) { }//���Ŏ��U��

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(projectorData.projectileDuration);
        if (!disabled) { DestroyPJTL(true); }
    }
}
