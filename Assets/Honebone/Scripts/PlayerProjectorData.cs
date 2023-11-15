using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "p_pjtor", menuName = "ScriptableObject/ProjectorData_P")]


public class PlayerProjectorData : ScriptableObject
{
    public GameObject projectile;

    public AudioClip SE_GengeratePjtor;
    public AudioClip SE_Fire;
    //[Header("projector���v���C���[�߂����Ĕ��˂��邩")] public bool targetPlayer = true;
    //[Header("���ˉ񐔂�1�ȏ�̎��ɎQ�� �v���C���[�̌��݂̈ʒu��ΏۂƂ��邩")] public bool refreshPlayerPos = false;

    //[Header("�v���C���[��ǔ����邩/�����]�����x")] public float followPlayerSpeed;
    //[Header("���݂̃v���C���[�̈ʒu��ǔ����邩 false�Ȃ甭�ˎ��̏ꏊ��")] public bool followCurrentPlayer;

    [Header("���̔��˂Ŏˏo����e��")] public int pellets = 1;
    [Header("�����_���ȕ����ɔ��˂��邩")] public bool fireRandomly;
    [Header("+-(spread/2)���̃u����������")] public float spread;
    [Header("spread��ɓ��Ԋu�ɔ��˂��邩")] public bool equidistant;

    [Header("���ˉ�")] public float fireRounds = 1;
    [Header("���ˉ񐔂�2�ȏ�̎��ɎQ�� 1�����˂��邲�Ƃ̃C���^�[�o��[s] 0�Ȃ瓯������")] public float fireRate;

    //[Header("��]���Ȃ���")]
    //public bool lockRotation;
    [Header("min�`max�̊ԂŃ����_���Ɍ��܂�")]
    public float projectileSpeed_min = 10f;
    public float projectileSpeed_max = 10f;
    public float projectileDuration = 1f;
}
