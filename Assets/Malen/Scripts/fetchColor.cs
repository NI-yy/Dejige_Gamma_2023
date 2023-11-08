using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class FetchColor : MonoBehaviour
{

    private bool fetchFlag = false;
    private Tween circleAnimation;
    private CirclePainter painter;
    [Header("�F�z�����[�h�̍ő唼�a")]
    [SerializeField] float radiusMax;
    [Header("���C���J�����S�̂𕢂����a")]
    [SerializeField] float radiusCoverScreenAll;
    [Header("�F�z�����[�h�ڍs�ɂ����鎞��(s)")]
    [SerializeField] float duration;
    [SerializeField] PostProcessLayer PPL;

    private void Start()
    {
        painter = GetComponent<CirclePainter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (painter.scale == 0f)
                painter.scale = radiusCoverScreenAll;
            ChangeAnimation(true);
            PPL.enabled = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ChangeAnimation(false);
            Time.timeScale = 1f;
        }
    }

    void ChangeAnimation(bool isKeyPushed)
    {
        circleAnimation.Kill();
        Debug.Log(circleAnimation);
        if (isKeyPushed)
        {
            circleAnimation = DOTween.To(() => painter.scale, x => painter.scale = x, radiusMax, duration);
            Time.timeScale = 0.1f;
        }
        else{
            circleAnimation = DOTween.To(() => painter.scale, x => painter.scale = x, radiusCoverScreenAll, duration * 10).OnComplete(OnCompleteExtension);            
        }
    }

    void OnCompleteExtension()
    {
        painter.scale = 0f;
        PPL.enabled = false;
    }
}
