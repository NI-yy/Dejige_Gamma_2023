using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // �v���n�u�̎Q�Ƃ̂��߂̕ϐ�
    public GameObject prefab;
    [SerializeField] int maxHP;

    void Start()
    {
        for (int i = 0; i < maxHP; i++)
        {
            // �v���n�u���C���X�^���X��
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
            // �C���X�^���X�������I�u�W�F�N�g�����̃I�u�W�F�N�g�̎q�Ƃ��Đݒ�
            instance.transform.SetParent(transform);
        }
    }
}
