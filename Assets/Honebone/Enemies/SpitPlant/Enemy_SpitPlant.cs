using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpitPlant : Enemy
{
    [SerializeField]
    float range = 10;//���m�͈�
    [SerializeField]
    float attackDelayTime;
    [SerializeField]
    float attackIntervalTime;
    [SerializeField]
    EnemyProjectorData projectorData;
    Vector3 origin;
    Vector3 direction;

    bool interval;
    void Start()
    {
        Init();
        origin = transform.position;
        direction = GetPlayerDir();
    }

    void Update()
    {
        origin = transform.position;//origin�Ɏ��g�̍��W����
        direction = GetPlayerDir();//direction�Ɏ��g����v���C���[�Ɍ������P�ʃx�N�g������
        RaycastHit2D hit2D = Physics2D.Raycast(origin, direction, range);//Raycast���Č��m����Object��hit2D�ɑ��
        Debug.DrawRay(origin, direction * range, Color.red);//Ray�Ɠ����n�_�A�����A�����̐Ԃ�����1�t���[���`��

        //�U�����莞�Ԃ��o���Ă��� ���� Raycst�Ń^�O��"Player"�ł���Object�����m����
        if (!interval && hit2D.CheckRaycastHit("Player"))
        {
            interval = true;
            StartCoroutine(Attack());
        }

        SetSpriteFlip();
    }

    IEnumerator Attack()
    {
        Signal();
        yield return new WaitForSeconds(attackDelayTime);
        StartFireProjectile(projectorData, new Vector3());
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackIntervalTime);
        interval = false;
    }
}
