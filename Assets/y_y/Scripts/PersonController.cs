using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class PersonController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �ړ����x
    public float jumpForce = 5.0f; // �W�����v�̗�
    private Rigidbody2D rb;
    float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = KoitanInput.GetStick(StickCode.LeftStick).x;
        float verticalInput = Input.GetAxis("Vertical");

        // �I�u�W�F�N�g���ړ�������
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��
        {
            Jump();
        }

    }

    void Jump()
    {
        // Rigidbody2D�ɏ�����̗͂������ăW�����v����
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
