using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrtwpns.Mixbox;
using KoitanLib;

public class ObjectController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �ړ����x
    private Texture2D tex = null;
    [SerializeField]
    Color color;
    [SerializeField]
    Color color_2;
    [SerializeField]
    Color color_mix;
    Camera mainCamera;
    bool flag = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h�̓��͂��󂯎��
        float horizontalInput = Input.GetAxis("Horizontal");
        //float horizontalInput = KoitanInput.GetStick(StickCode LeftStick, 1);
        float verticalInput = Input.GetAxis("Vertical");

        // �I�u�W�F�N�g���ړ�������
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ColorPicker());
        }

        if (Input.GetMouseButtonDown(1))
        {
            color_mix = Mixbox.Lerp(color, color_2, 0.5f);
            SpriteRenderer spriterenderer = GetComponent<SpriteRenderer>();
            spriterenderer.color = color_mix;
        }
    }

    IEnumerator ColorPicker()
    {
        tex = new Texture2D(1, 1, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();

        Vector3 pos = transform.position;
        pos = pos + new Vector3(0.51f, 0.25f, 0f);
        // ���[���h���W���X�N���[�����W�ɕϊ����܂��B
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(pos);

        Vector2 pos_2d = new Vector2(screenPosition.x, screenPosition.y);
        tex.ReadPixels(new Rect(pos_2d.x, pos_2d.y, 1, 1), 0, 0); //�ʐ^�B����tex�ɕۑ�

        if (flag)
        {
            color = tex.GetPixel(0, 0);
            flag = !flag;
        }
        else
        {
            color_2 = tex.GetPixel(0, 0);
            flag = !flag;
        }
        //Debug.Log((pos_2d.x + 0.51f, pos_2d.y + 0.25f));
    }
}
