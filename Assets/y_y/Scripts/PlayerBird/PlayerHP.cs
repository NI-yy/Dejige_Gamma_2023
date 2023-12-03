using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    // �v���n�u�̎Q�Ƃ̂��߂̕ϐ�
    public GameObject prefab;
    [SerializeField] int maxHP;
    List<GameObject> playerHPs = new List<GameObject>();
    int currentHPCount;

    void Start()
    {
        currentHPCount = maxHP;

        for (int i = 0; i < maxHP; i++)
        {
            // �v���n�u���C���X�^���X��
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
            // �C���X�^���X�������I�u�W�F�N�g�����̃I�u�W�F�N�g�̎q�Ƃ��Đݒ�
            instance.transform.SetParent(transform);
            playerHPs.Add(instance);
        }
    }

    public void ReduceHP()
    {
        currentHPCount--;
        if(currentHPCount < 0)
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene("Title");
        }
        else
        {
            Destroy(playerHPs[currentHPCount]);
        }
    }
}
