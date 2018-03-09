#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 08:52:51
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BridgeUI;
/// <summary>
/// 传动播放面板
/// <summary>
public class PlayingPanel : SingleCloseAblePanel
{
    [SerializeField]
    private Button m_playBtn;
    [SerializeField]
    private Button m_stopBtn;
    private ObjCharge objChage { get { return ObjCharge.Instence; } }
    private Robot robot { get { return Robot.Instence; } }

    protected override void Awake()
    {
        base.Awake();
        robot.Reset();
        RegistBtnEvent();
    }
    private void RegistBtnEvent()
    {
        m_playBtn.onClick.AddListener(OnPlayBtnClicked);
        m_stopBtn.onClick.AddListener(OnStopBtnClicked);
    }

    public void OnPlayBtnClicked()
    {
        m_playBtn.gameObject.SetActive(false);
        m_stopBtn.gameObject.SetActive(true);
        objChage.active = true;
        robot.Reset();
    }
    private void OnStopBtnClicked()
    {
        m_playBtn.gameObject.SetActive(true);
        m_stopBtn.gameObject.SetActive(false);
        robot.Stop();
        objChage.active = false;
    }

    protected override void OnDestroy()
    {
        OnStopBtnClicked();
        base.OnDestroy();
    }
}
