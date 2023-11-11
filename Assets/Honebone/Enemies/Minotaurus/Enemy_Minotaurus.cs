using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Minotaurus : Enemy
{
    [SerializeField]
    float engageRange;
    [SerializeField]
    float disengageRange;

    [SerializeField]
    float attackRange;
    [SerializeField]
    float attackDelayTime;
    [SerializeField]
    float attackIntervalTime;

    [SerializeField]
    GameObject shockWave;
    [SerializeField, Header("shockWave�����߂ɐ�������鋗��")]
    int shockWaveStartDistance;
    [SerializeField, Header("shockWave����������鐔=�U������")]
    int shockWaveAmount;
    [SerializeField,Header("����shockWave�����������܂ł̊Ԋu")]
    float shockCycleTime;
    [SerializeField, Header("shockWave�Ԃ̋���")]
    float shockWaveGap;
    [SerializeField, Header("shockWave�𐶐����邽�߂�groundCheck�𐶐����鍂��")]
    float groundCheckHeight;
    [SerializeField, Header("shockWave�𐶐����邽�߂�groundCheck�̒���")]
    float groundCheckRange;

    //[SerializeField]
    //float minHeight;

    Vector3 origin;
    Vector3 direction;

    bool attacking;
    bool interval;
    bool engaged;

    //RaycastHit2D groundHit;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        origin = transform.position;
        direction = GetPlayerDir();
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;
        direction = GetPlayerDir();
        RaycastHit2D hit2D = Physics2D.Raycast(origin, direction, engageRange);
        if (engaged) { Debug.DrawRay(origin, direction * disengageRange, Color.blue); }
        else { Debug.DrawRay(origin, direction * engageRange, Color.yellow); }
        Debug.DrawRay(origin, direction * attackRange, Color.red);

        //groundHit = Physics2D.Raycast(origin, Vector2.down, minHeight);
        //Debug.DrawRay(origin, Vector2.down * minHeight, Color.gray);


        if (hit2D.CheckRaycastHit("Player"))
        {
            if (!engaged)//���J�n
            {
                engaged = true;
            }
        }
        if (!interval && engaged && GetPlayerDistance() <= attackRange)//�U��
        {
            attacking = true;
            interval = true;
            StartCoroutine(Attack());
        }
        if (engaged && GetPlayerDistance() > disengageRange) { engaged = false; }//���I��



        SetSpriteFlip();
    }
    private void FixedUpdate()
    {
        if (engaged && !attacking && GetPlayerDistance() > attackRange)//�ړ�
        {
            transform.Translate(GetPlayerDir_Horizontal() * enemyStatus.moveSpeed * 0.01f);
        }
        rb.velocity = Vector2.zero;
    }

    IEnumerator Attack()
    {
        Signal();
        yield return new WaitForSeconds(attackDelayTime);
        StartCoroutine(ShockWave());
        attacking = false;
        yield return new WaitForSeconds(attackIntervalTime);
        interval = false;
    }
    IEnumerator ShockWave()
    {
        Vector3 raycastPos = transform.position;
        Vector3 attackPos = new Vector3();
        raycastPos.x += shockWaveStartDistance * GetPlayerDir_Horizontal().x;
        float shockWaveMove = shockWaveGap * GetPlayerDir_Horizontal().x;
        for (int i = 0; i < shockWaveAmount; i++)
        {
            RaycastHit2D[] ground = Physics2D.RaycastAll(raycastPos, Vector2.down, groundCheckRange);
            //Debug.DrawRay(raycastPos, Vector2.down * groundCheckRange, Color.gray, 1f);
            bool groundFound = false;
            foreach (RaycastHit2D hit in ground)
            {
                if (hit.CheckRaycastHit("Ground"))
                {
                    attackPos = hit.point;
                    attackPos.y += 3.75f;
                    Instantiate(shockWave, attackPos, Quaternion.identity);//�U���̐���

                    raycastPos.x += shockWaveMove;
                    raycastPos.y = hit.point.y + groundCheckHeight;//����groundCheck�̏ꏊ��ݒ�
                    groundFound = true;
                    break;
                }
            }
            if (!groundFound) { break; }

            
            yield return new WaitForSeconds(shockCycleTime);
            
        }
    }
}
