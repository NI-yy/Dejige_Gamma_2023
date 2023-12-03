using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoitanLib;
using Scrtwpns.Mixbox;
using System.Drawing;

public class TwoPlayerManager : MonoBehaviour
{
    [SerializeField]
    GameObject person;
    [SerializeField] GameObject bird;
    [SerializeField] Material birdMaterial;

    [SerializeField]
    GameObject UI_ColorOrb;
    [SerializeField] ParticleSystem particle_wand;//�p�[�e�B�N�����Q��

    public bool wand_init = true;
    public bool Birdenabled = false; //����get������F�����\�ɂ���

    private UnityEngine.Color color_wand;
    private UnityEngine.Color color_bird;
    private UnityEngine.Color color_mix;
    private float h, s, v;
    private bool enableBird = true;

    public enum WandColor { Other, White, Orange, Yellow, Green, Blue, Purple, Red, Black }

    private void Start()
    {
        //�ŏ���̐F�̓I�����W�F�A���̐F�͔��Ƃ���
        color_wand = new UnityEngine.Color(1f, 0.64f, 0f);
        color_bird = UnityEngine.Color.white;

        //��̃G�t�F�N�g��UI�̃I�[�u�ɃI�����W�F�𔽉f
        ChangeUIOrbColor(color_wand);
        var main = particle_wand.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color_bird);

        //���ɔ��𔽉f
        bird.GetComponent<BirdParticleColorController>().SetBirdParticleColor(color_bird);
        birdMaterial.color = color_bird;

        UnityEngine.Color.RGBToHSV(color_wand, out h, out s, out v);
        h = Remap(h, 0, 1, 0, 360);
        s = Remap(s, 0, 1, 0, 100);
        v = Remap(v, 0, 1, 0, 100);
    }

    private void Update()
    {
        if (Birdenabled && (KoitanInput.Get(ButtonCode.RB) || Input.GetKey(KeyCode.Alpha1)))
        {
            if (enableBird)
            {
                MoveBird();
                enableBird = false;
            }
        }
        else if (Birdenabled && (KoitanInput.GetUp(ButtonCode.RB) || Input.GetKeyUp(KeyCode.Alpha1)))
        {
            MovePerson();
            enableBird = true;
        }
        else if (Birdenabled && (KoitanInput.GetDown(ButtonCode.LB) || Input.GetKeyDown(KeyCode.Alpha2)))
        {
            ExchangeColor();
        }
    }

    public void MovePerson()
    {
        bird.GetComponent<BirdParticleColorController>().enabled = false;
        person.GetComponent<Player>().enabled = true;
    }

    public void MoveBird()
    {
        bird.GetComponent<BirdParticleColorController>().enabled = true;
        person.GetComponent<Player>().enabled = false;
    }

    void ExchangeColor()
    {

        //��ƒ��̐F������

        //���̐F����̐F��
        bird.GetComponent<BirdParticleColorController>().SetBirdParticleColor(color_wand);
        birdMaterial.color = color_wand;

        //��̐F�𒹂̐F��
        var main = particle_wand.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color_bird);
        particle_wand.Clear();
        particle_wand.Play();

        //UI�I�[�u�̐F�𒹂̐F��
        ChangeUIOrbColor(color_bird);

        UnityEngine.Color.RGBToHSV(color_bird, out h, out s, out v);
        h = Remap(h, 0, 1, 0, 360);
        s = Remap(s, 0, 1, 0, 100);
        v = Remap(v, 0, 1, 0, 100);

        //���Ə�̐F���������Ă���
        var color_temp = color_bird;
        color_bird = color_wand;
        color_wand = color_temp;


        wand_init = false;
    }

    public void MixColor()
    {
        //�F���������A��͍������F�ɁA���̐F�͔��ɂ��Ă���
        //������O�̐F�͕ێ����Ă��������̂�color_wand,color_bird�͕ς��Ȃ�
        color_mix = Mixbox.Lerp(color_wand, color_bird, 0.5f);

        //����
        bird.GetComponent<BirdParticleColorController>().SetBirdParticleColor(UnityEngine.Color.white);
        birdMaterial.color = UnityEngine.Color.white;

        //��������F
        var main = particle_wand.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color_mix);
        particle_wand.Clear();
        particle_wand.Play();

        //UI�������F��
        ChangeUIOrbColor(color_mix);

        UnityEngine.Color.RGBToHSV(color_mix, out h, out s, out v);
        h = Remap(h, 0, 1, 0, 360);
        s = Remap(s, 0, 1, 0, 100);
        v = Remap(v, 0, 1, 0, 100);

        
    }

    void ChangeUIOrbColor(UnityEngine.Color color)
    {
        UI_ColorOrb.GetComponent<Image>().color = color;
    }

    public void DevideMixedColor()
    {
        //���U����������A�F�����ɖ߂�

        //��
        bird.GetComponent<BirdParticleColorController>().SetBirdParticleColor(color_bird);
        birdMaterial.color = color_bird;

        //��
        var main = particle_wand.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color_wand);
        particle_wand.Clear();
        particle_wand.Play();

        //UI
        ChangeUIOrbColor(color_wand);
    }

    // ���}�b�v���s���֐�
    float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // ���}�b�v���v�Z���ĕԂ�
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    public void SetBirdColor(UnityEngine.Color color)
    {
        color_bird = color;
    }

    //��, ��, ��, ��, ��, ��, ��, ��������
    public WandColor GetWandColor()
    {
        if (s >= 50 && v >= 50)
        {
            if (0 <= h && h < 45)
            {
                return WandColor.Orange;
                //return WandColor.Red;
            }
            else if (45 <= h && h < 75)
            {
                return WandColor.Yellow;
            }
            else if (75 <= h && h < 165)
            {
                return WandColor.Green;
            }
            else if (165 <= h && h < 270)
            {
                return WandColor.Blue;
            }
            else if (270 <= h && h < 310)
            {
                return WandColor.Purple;
            }
            else if (310 <= h && h < 360)
            {
                return WandColor.Red;
            }
        }
        else if (v < 50)
        {
            return WandColor.Black;
        }
        else
        {
            return WandColor.White;
        }

        return WandColor.Other;
    }
}
