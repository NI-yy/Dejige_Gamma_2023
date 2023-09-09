using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using KoitanLib;

public class Player : MonoBehaviour
{
    [Header("デッドゾーン")] public float deadZone;

    [Header("重力")] public float gravity;

    [Header("地上最高スピード")] public float SpeedGroundMax;
    [Header("地上加速度")] public float accelerationGround;
    [Header("空中最高スピード")] public float SpeedAirMax;
    [Header("空中加速度")] public float accelerationAir;
    [Header("ジャンプ最低高度")] public float jumpHeightMin;
    [Header("ジャンプ最高高度")] public float jumpHeightMax;
    [Header("ジャンプ最大時間")] public float jumpTimeMax;

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







    /////////////////////////　　　イベント関数　　　////////////////////////////


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
            anim.SetBool("jumping", false);
            anim.SetBool("running", false);

            ManageXMoveAir();
            ManageYMoveAir();

        }
        ResetKeyDown();


    }





    /////////////////////////　　　システム系　　　////////////////////////////

    private void GetKeysInput()
    {
        horizontalKeyRaw = KoitanInput.GetStick(StickCode.LeftStick).x;
        Debug.Log(horizontalKeyRaw);
        verticalKeyRaw = KoitanInput.GetStick(StickCode.LeftStick).y;

        moveKeyVec = new Vector3(verticalKeyRaw, horizontalKeyRaw);


        if (Input.GetKey(KeyCode.Space) || KoitanInput.Get(ButtonCode.A) || KoitanInput.Get(ButtonCode.B))
        {
            spaceKey = true;
        }
        else
        {
            spaceKey = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) || KoitanInput.GetDown(ButtonCode.A) || KoitanInput.GetDown(ButtonCode.B))
        {
            spaceKeyDown = true;
        }
    }

    private void ResetKeyDown()
    {
        spaceKeyDown = false;
        //Debug.Log(("reset", Time.time));
    }







    /////////////////////////　　　動き　　　////////////////////////////


    private void ManageXMoveGround()
    {
        Vector3 newScale = playerScale;

        //動摩擦係数＝MaxスピードでAddForceと釣り合う値
        float deceleration = accelerationGround * mass / SpeedGroundMax;

        if (horizontalKeyRaw > deadZone)
        {
            anim.SetBool("running", true);
            transform.localScale = newScale;

            //x方向に加速度*質量の力を加える
            rb.AddForce(new Vector2(1, 0) * accelerationGround * mass);

            //x方向の速度に従って摩擦が働く
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else if (horizontalKeyRaw < -deadZone)
        {
            anim.SetBool("running", true);

            //左右の向きを変える
            newScale.x = -newScale.x;
            transform.localScale = newScale;


            //-x方向に加速度*質量の力を加える
            rb.AddForce(new Vector2(-1, 0) * accelerationGround * mass);

            //x方向の速度に従って摩擦が働く
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else
        {
            anim.SetBool("running", false);


            //x方向の速度に従って摩擦が働く
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration * 10);

            //ｘ軸方向のスピードの絶対値が極小（SpeedMaxの1/100）になったら０にする
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

        //空気抵抗係数＝MaxスピードでAddForceと釣り合う値
        float deceleration = accelerationAir * mass / SpeedAirMax;

        if (horizontalKeyRaw > deadZone)
        {
            transform.localScale = newScale;

            //x方向に加速度*質量の力を加える
            rb.AddForce(new Vector2(1, 0) * accelerationAir * mass);

            //x方向の速度に従って空気抵抗が働く
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else if (horizontalKeyRaw < -deadZone)
        {
            anim.SetBool("running", true);

            //左右の向きを変える
            newScale.x = -newScale.x;
            transform.localScale = newScale;


            //-x方向に加速度*質量の力を加える
            rb.AddForce(new Vector2(-1, 0) * accelerationAir * mass);

            //x方向の速度に従って空気抵抗が働く
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);
        }
        else
        {
            anim.SetBool("running", false);


            //x方向の速度に従って摩擦が働く
            rb.AddForce(new Vector2(rb.velocity.x, 0) * -1 * deceleration);

            //ｘ軸方向のスピードの絶対値が極小（SpeedMaxの1/100）になったら０にする
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
            rb.AddForce(new Vector2(0, 1) * initialForce, ForceMode2D.Impulse);
            jumpingTimeCount = 0f;
            anim.SetBool("jumping", true);
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
}