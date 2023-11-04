using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using KoitanLib;

public class Player : MonoBehaviour
{
    [Header("�f�b�h�]�[��")] public float deadZone;

    [Header("�d��")] public float gravity;

    [Header("�n��ō��X�s�[�h")] public float SpeedGroundMax;
    [Header("�n������x")] public float accelerationGround;
    [Header("�󒆍ō��X�s�[�h")] public float SpeedAirMax;
    [Header("�󒆉����x")] public float accelerationAir;
    [Header("�W�����v�Œፂ�x")] public float jumpHeightMin;
    [Header("�W�����v�ō����x")] public float jumpHeightMax;
    [Header("�W�����v�ő厞��")] public float jumpTimeMax;

    //public GameObject canvasGame;

    private GameObject gameManagerObj;
    //private GameManager gameManagerScript;
    //private SoundManager soundManagerScript;

    public GroundCheck ground;
    public GroundCheck head;


    private Animator anim = null;
    private Rigidbody2D rb = null;


    private bool isOnGround = false;
    private bool isOnHead = false;

    private float horizontalKeyRaw;
    private float verticalKeyRaw;
    private bool spaceKey;
    private bool spaceKeyDown;
    private Vector3 moveKeyVec;


    private Vector3 playerScale;
    private Vector3 initialPosition;
    private float jumpingTimeCount;
    public float mass;
    [SerializeField]
    GameObject bulletPrefab;
    public GameObject[] gameObjects;







    /////////////////////////�@�@�@�C�x���g�֐��@�@�@////////////////////////////


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        playerScale = transform.localScale;
        initialPosition = transform.position;
        mass = rb.mass;

        //gameManagerObj = GameObject.Find("GameManager");
        //gameManagerScript = gameManagerObj.GetComponent<GameManager>();
        //soundManagerScript = gameManagerObj.GetComponent<SoundManager>();
    }


    void Update()
    {
        GetKeysInput();
        isOnGround = ground.IsOnGround();
        isOnHead = head.IsOnGround();
    }

    private void FixedUpdate()
    {
        if (isOnGround)
        {
            anim.SetBool("onGround", true);

            ManageXMoveGround();
            ManageYMoveGround();

        }
        else
        {
            anim.SetBool("onGround", false);
            //anim.SetBool("jumping", false);
            anim.SetBool("running", false);

            ManageXMoveAir();
            ManageYMoveAir();

        }
        ResetKeyDown();


    }





    /////////////////////////�@�@�@�V�X�e���n�@�@�@////////////////////////////

    private void GetKeysInput()
    {
        horizontalKeyRaw = KoitanInput.GetStick(StickCode.LeftStick).x;
        verticalKeyRaw = KoitanInput.GetStick(StickCode.LeftStick).y;

        moveKeyVec = new Vector3(verticalKeyRaw, horizontalKeyRaw);


        if (Input.GetKey(KeyCode.Space) || KoitanInput.Get(ButtonCode.A))
        {
            spaceKey = true;
        }
        else
        {
            spaceKey = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) || KoitanInput.GetDown(ButtonCode.A))
        {
            spaceKeyDown = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void ResetKeyDown()
    {
        spaceKeyDown = false;
    }







    /////////////////////////�@�@�@�����@�@�@////////////////////////////


    private void ManageXMoveGround()
    {
        Vector3 newScale = playerScale;

        //�����C�W����Max�X�s�[�h��AddForce�ƒނ荇���l
        float deceleration = accelerationGround * mass / SpeedGroundMax;

        if (horizontalKeyRaw > deadZone)
        {
            anim.SetBool("running", true);
            transform.localScale = newScale;

            //x�����ɉ����x*���ʂ̗͂�������
            rb.AddForce(new Vector2(1, 0) * accelerationGround * mass);

            //x�����̑��x�ɏ]���Ė��C������
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else if (horizontalKeyRaw < -deadZone)
        {
            anim.SetBool("running", true);

            //���E�̌�����ς���
            newScale.x = -newScale.x;
            transform.localScale = newScale;


            //-x�����ɉ����x*���ʂ̗͂�������
            rb.AddForce(new Vector2(-1, 0) * accelerationGround * mass);

            //x�����̑��x�ɏ]���Ė��C������
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else
        {
            anim.SetBool("running", false);


            //x�����̑��x�ɏ]���Ė��C������
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration * 10);

            //���������̃X�s�[�h�̐�Βl���ɏ��iSpeedMax��1/100�j�ɂȂ�����O�ɂ���
            if (Mathf.Abs(rb.velocity.x) < SpeedGroundMax * 0.01f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

        }

    }

    private void ManageXMoveAir()
    {

        Vector3 newScale = playerScale;
        float mass = rb.mass;

        //��C��R�W����Max�X�s�[�h��AddForce�ƒނ荇���l
        float deceleration = accelerationAir * mass / SpeedAirMax;

        if (horizontalKeyRaw > deadZone)
        {
            transform.localScale = newScale;

            //x�����ɉ����x*���ʂ̗͂�������
            rb.AddForce(new Vector2(1, 0) * accelerationAir * mass);

            //x�����̑��x�ɏ]���ċ�C��R������
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else if (horizontalKeyRaw < -deadZone)
        {
            anim.SetBool("running", true);

            //���E�̌�����ς���
            newScale.x = -newScale.x;
            transform.localScale = newScale;


            //-x�����ɉ����x*���ʂ̗͂�������
            rb.AddForce(new Vector2(-1, 0) * accelerationAir * mass);

            //x�����̑��x�ɏ]���ċ�C��R������
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else
        {
            anim.SetBool("running", false);


            //x�����̑��x�ɏ]���Ė��C������
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);

            //���������̃X�s�[�h�̐�Βl���ɏ��iSpeedMax��1/100�j�ɂȂ�����O�ɂ���
            if (Mathf.Abs(rb.velocity.x) < SpeedAirMax * 0.01f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void ManageYMoveGround()
    {
        float initialForce = Mathf.Sqrt(gravity * jumpHeightMin * 2) * mass;

        if (spaceKeyDown)
        {
            //Debug.Log("jump");
            rb.AddForce(new Vector2(0, 1) * initialForce, ForceMode2D.Impulse);
            jumpingTimeCount = 0f;
            //anim.SetBool("jumping", true);
            //soundManagerScript.PlayOneShot(0);
        }

        rb.AddForce(new Vector2(0, -1) * gravity);


    }

    private void ManageYMoveAir()
    {
        float jumpingForce = (1 - 1 / jumpHeightMax) * gravity * mass * 1.5f;

        if (spaceKey && jumpingTimeCount < jumpTimeMax && !isOnHead)
        {
            rb.AddForce(new Vector2(0, 1) * jumpingForce);
            jumpingTimeCount += Time.deltaTime;
        }

        rb.AddForce(new Vector2(0, -1) * gravity);
    }

    private void Attack()
    {
        Vector3 pos = transform.position + new Vector3(0.5f, -0.5f, 0);
        Instantiate(bulletPrefab, pos, Quaternion.identity);
    }
}