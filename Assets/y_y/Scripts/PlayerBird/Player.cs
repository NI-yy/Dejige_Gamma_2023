using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using KoitanLib;
using UnityEngine.UI;

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
    [Header("2�i�W�����v")] public bool beAbleToDoubleJump;

    //public GameObject canvasGame;

    private GameObject gameManagerObj;
    //private GameManager gameManagerScript;
    //private SoundManager soundManagerScript;
    private PlayerHP playerHPControllerScript;

    public GroundCheck ground;
    public GroundCheck head;
    public GameObject PlayerHPController;
    public GameObject UI_ColorOrb;
    public GameObject bullet;
    public GameObject _TwoPlayerManager;

    public GameObject particleSystem_electric;
    public GameObject particleSystem_electricBall;
    public GameObject particleSystem_fire;
    public GameObject particleSystem_fire_2;
    public GameObject particleSystem_leaf;
    public GameObject particleSystem_water;
    public GameObject particleSystem_soil;


    //honebone�ǉ�
    bool lookRight;
    public GameObject projector;
    public PlayerProjectorData projectorData;


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

    //�G����̍U������֌W
    private string enemyTag = "Enemy";
    private string bulletTag = "bullet";
    private float initialForce;

    //2�i�W�����v�֌W
    private bool afterFirstJump = false; //1��W�����v�����ォ�ǂ����B���ꂪtrue�̎��̂�2�i�W�����v�\

    //�U���F�֌W
    private TwoPlayerManager twoPlayerManagerScript;

    private bool attackFlag = true; //1��U������ƃN�[���^�C���L




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
        playerHPControllerScript = PlayerHPController.GetComponent<PlayerHP>();

        twoPlayerManagerScript = _TwoPlayerManager.GetComponent<TwoPlayerManager>();
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

        if (KoitanInput.GetDown(ButtonCode.B) || Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void ResetKeyDown()
    {
        spaceKeyDown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            playerHPControllerScript.ReduceHP();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(bulletTag))
        {
            playerHPControllerScript.ReduceHP();
        }
    }


    //honebone : �������o�����e�ɂ������đ��UGameOver�ɂȂ�̂ł�������R�����g�A�E�g���Ă܂�
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag(bulletTag))
    //    {
    //        playerHPControllerScript.ReduceHP();
    //    }
    //}





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
            lookRight = true;

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
            lookRight = false;


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
            lookRight = true;

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
            lookRight = false;


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
        //2�i�W�����v�̂��߂ɕϐ��̃X�R�[�v�ύX_yy
        //float initialForce = Mathf.Sqrt(gravity * jumpHeightMin * 2) * mass;
        initialForce = Mathf.Sqrt(gravity * jumpHeightMin * 2) * mass;

        //�n�ʏ�ŃX�y�[�X�L�[���������ꂽ�Ƃ��A������ɗ͂������邱�ƂŃW�����v����.�����Ɏ��Ԍv�����n�܂�
        if (spaceKeyDown)
        {
            //Debug.Log("jump");
            rb.AddForce(new Vector2(0, 1) * initialForce, ForceMode2D.Impulse);
            jumpingTimeCount = 0f;
            //anim.SetBool("jumping", true);
            //soundManagerScript.PlayOneShot(0);
            afterFirstJump = true;
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
        else if ((spaceKeyDown || KoitanInput.GetDown(ButtonCode.A)) && afterFirstJump && beAbleToDoubleJump)
        {
            //2�i�W�����v
            //Debug.Log("jump");
            rb.AddForce(new Vector2(0, 1) * initialForce, ForceMode2D.Impulse);
            jumpingTimeCount = 0f;
            //anim.SetBool("jumping", true);
            //soundManagerScript.PlayOneShot(0);
            afterFirstJump = false;
        }

        rb.AddForce(new Vector2(0, -1) * gravity);
    }

    private void Attack()
    {
        if (attackFlag)
        {
            attackFlag = false;
            Color color_wand = UI_ColorOrb.GetComponent<Image>().color;
            //string wandColorString = twoPlayerManagerScript.GetWandColor().ToStr();
            TwoPlayerManager.WandColor wandColor = twoPlayerManagerScript.GetWandColor();
            Debug.Log("��̐F: " + wandColor);
            //GameObject currentBullet = Instantiate(bullet, this.transform.position + new Vector3(0f, 4.0f, 0f), Quaternion.identity);
            //currentBullet.GetComponent<SpriteRenderer>().color = color_wand;
            //currentBullet.GetComponent<bulletController>().SetBulletColor(wandColorString);

            Vector3 dir = Vector3.right;
            if (!lookRight) { dir = Vector3.left; }


            StartCoroutine(ParticleAttack(wandColor, dir, color_wand));
        }
    }

    private IEnumerator ParticleAttack(TwoPlayerManager.WandColor wandColor, Vector3 dir, Color color_wand)
    {
        anim.SetBool("attack", true);
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, dir);
        if (wandColor == TwoPlayerManager.WandColor.Red)
        {
            anim.SetBool("redAttack", true);
            yield return new WaitForSeconds(0.2f);

            if (lookRight)
            {
                var p = Instantiate(particleSystem_fire, transform.position + new Vector3(0f, 4.0f, 0f), quaternion);
                Destroy(p, 1.0f);
            }
            else
            {
                var p = Instantiate(particleSystem_fire, transform.position + new Vector3(0f, 4.0f, 0f), Quaternion.Euler(0, 180, 0));
                Destroy(p, 1.0f);
            }

            yield return new WaitForSeconds(0.8f); //1�ڂ̃A�j���[�V�����I���҂�


            anim.SetBool("toAttackFire2", true);
            yield return new WaitForSeconds(0.1f);
            if (lookRight)
            {
                var p = Instantiate(particleSystem_fire_2, transform.position + new Vector3(0f, 4.0f, 0f), quaternion);
                Destroy(p, 1.0f);
            }
            else
            {
                var p = Instantiate(particleSystem_fire_2, transform.position + new Vector3(0f, 4.0f, 0f), Quaternion.Euler(0, 180, 0));
                Destroy(p, 1.0f);
            }

            StartCoroutine(ResetAnimFlag("redAttack"));
            StartCoroutine(ResetAnimFlag("toAttackFire2"));
        }
        else if (wandColor == TwoPlayerManager.WandColor.Green)
        {
            anim.SetBool("greenAttack", true);
            yield return new WaitForSeconds(0.3f);

            var p = Instantiate(particleSystem_leaf, transform.position + new Vector3(0f, 4.0f, 0f), Quaternion.identity);
            Destroy(p, 0.7f);
            
            StartCoroutine(ResetAnimFlag("attack"));
            StartCoroutine(ResetAnimFlag("greenAttack"));
        }
        else if (wandColor == TwoPlayerManager.WandColor.Blue)
        {
            anim.SetBool("blueAttack", true);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ResetAnimFlag("attack"));
            StartCoroutine(ResetAnimFlag("blueAttack"));
            if (lookRight)
            {
                var p = Instantiate(particleSystem_water, transform.position + new Vector3(7f, 6.0f, 0f), Quaternion.identity);
                p.GetComponent<AttackWaterController>().isRight = true;
                p.GetComponent<AttackWaterController>().ActiveBulletCollider();
            }
            else
            {
                var p = Instantiate(particleSystem_water, transform.position + new Vector3(-7f, 6.0f, 0f), Quaternion.Euler(0, 180, 0));
                p.GetComponent<AttackWaterController>().isRight = false;
                p.GetComponent<AttackWaterController>().ActiveBulletCollider();
            }
            
        }
        else if (wandColor == TwoPlayerManager.WandColor.Orange)
        {
            anim.SetBool("orangeAttack", true);
            yield return new WaitForSeconds(1f);

            if (lookRight)
            {
                var p = Instantiate(particleSystem_soil, transform.position + new Vector3(5f, 7.0f, 0f), Quaternion.identity);
                Destroy(p, 1.0f);
            }
            else
            {
                var p = Instantiate(particleSystem_soil, transform.position + new Vector3(-5f, 7.0f, 0f), Quaternion.Euler(0, 180, 0));
                Destroy(p, 1.0f);
            }

            
            StartCoroutine(ResetAnimFlag("attack"));
            StartCoroutine(ResetAnimFlag("orangeAttack"));
        }
        else if(wandColor == TwoPlayerManager.WandColor.Yellow)
        {
            anim.SetBool("yellowAttack", true);
            yield return new WaitForSeconds(0.3f);

            var p = Instantiate(particleSystem_electric, transform.position + new Vector3(0f, 4.0f, 0f), Quaternion.identity);

            var p_ball = Instantiate(projector, transform.position + new Vector3(0f, 4.0f, 0f), Quaternion.identity);
            p_ball.GetComponent<PlayerProjector>().Init(projectorData, dir, color_wand);

            Destroy(p, 0.7f);
            
            StartCoroutine(ResetAnimFlag("yellowAttack"));
        }

        StartCoroutine(ResetAnimFlag("attack"));

        yield return new WaitForSeconds(2.0f);
        attackFlag = true;
    }

    //void ResetFlag(string flagName)
    //{
    //    anim.SetBool(flagName, false);
    //}

    private IEnumerator ResetAnimFlag(string flagName)
    {
        yield return new WaitForSeconds(1f);

        anim.SetBool(flagName, false);
    }
}