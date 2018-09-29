using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class VRMLipSync : MonoBehaviour {

    //VrmLipSyncTargetObj にはLipSyncを適用させるVRMアバターのGameObject名を入れます。
    //公式サンプルのVRMローダーでは VRM という名前でシーンに配置されるので...
    //デフォルト設定は VRM になってます。
    [SerializeField]
    private string LipSyncTargetName = "VRM";

    private VRMBlendShapeProxy VrmProxy;
    private GameObject VRMavater;

    //LipSyncの感度を調整 0.1が鈍感 2.0が敏感
    [SerializeField]
    [Range(0.1f, 2.0f)]
    private float LipSyncSensitivity = 1.0f;
    
    private int LipType;
    private float LipValue;

    //trueならLipSynkが有効 falseで無効
    [SerializeField]
    private bool VrmLipSyncIsActive;

    void Start () {
        //VrmLipSyncIsActiveがtrueなら、LipSyncを実行
        VrmLipSyncIsActive = true;
    }

    //クソ雑魚AddComponent
    public void VrmLipSyncSetup()
    {
        //LipSyncTargetNameでVRMを検索、Proxyを取得できなければセットアップの終了
        VrmProxy = GetVrmProxy(LipSyncTargetName);
        if (VrmProxy == null) { return; }

        if (VRMavater.GetComponent<AudioSource>() == null)
        {
            VRMavater.AddComponent<AudioSource>();
        }

        if (VRMavater.GetComponent<OVRLipSyncMicInput>() == null)
        {
            VRMavater.AddComponent<OVRLipSyncMicInput>();
        }

        if (VRMavater.GetComponent<OVRLipSyncContext>() == null)
        {
            VRMavater.AddComponent<OVRLipSyncContext>();
        }

        if (VRMavater.GetComponent<OVRLipSyncContextMorphTarget>() == null)
        {
            VRMavater.AddComponent<OVRLipSyncContextMorphTarget>();
        }
        Debug.Log("VRMavaterにLipSyncをセットアップしました。");
    }

    //nameでVRMを検索してVRMBlendShapeProxyを取得する。
    private VRMBlendShapeProxy GetVrmProxy(string name)
    {
        //nameで検索して見つからなければ終了
        if (GameObject.Find(name) == true)
        {
            VRMavater = GameObject.Find(name);
        }
        else if(GameObject.Find(name) == false)
        {
            Debug.Log(name + " は見つかりませんでした。VRMのGameObjectを " + name + " に変更してください。");
            return null;
        }

        //検索したVRMにVRMBlendShapeProxyがアタッチされてなければ終了
        if (VRMavater.GetComponent<VRMBlendShapeProxy>() == true)
        {
            VrmProxy = VRMavater.GetComponent<VRMBlendShapeProxy>();
        }
        else if (VRMavater.GetComponent<VRMBlendShapeProxy>() == false)
        {
            Debug.Log(name + " に VRMBlendShapeProxy がアタッチされていません。再度セットアップしてください");
            return null;
        }

        //VRMからVRMBlendShapeProxyを取得
        VrmProxy = VRMavater.GetComponent<VRMBlendShapeProxy>();
        Debug.Log(name + " の VRMBlendShapeProxy を取得しました");
        return VrmProxy;
    }

    //OVRLiySyncの処理を VRM の VRMBlendShapeProxy に対応させる
    private void LipSyncConversion()
    {
        float Value;
        // VRMLipValue[0] は「無音時に1を返す処理」の為、iに0を含めない
        for (int i = 1; i < 15; i++)
        {
            Value = VRMavater.GetComponent<OVRLipSyncContextMorphTarget>().VRMLipValue[i];
            //1番大きい値の時にLipTypeを更新
            if (LipValue < Value)
            {
                LipValue = VRMavater.GetComponent<OVRLipSyncContextMorphTarget>().VRMLipValue[i];
                LipType = i;
            }
        }

        switch (LipType)
        {
            case 10:
                VrmProxy.SetValue(BlendShapePreset.A, LipValue / LipSyncSensitivity);
                Value = 0.0f;
                LipValue = 0.0f;
                break;
            case 12:
                VrmProxy.SetValue(BlendShapePreset.I, LipValue / LipSyncSensitivity);
                Value = 0.0f;
                LipValue = 0.0f;
                break;
            case 14:
                VrmProxy.SetValue(BlendShapePreset.U, LipValue / LipSyncSensitivity);
                Value = 0.0f;
                LipValue = 0.0f;
                break;
            case 11:
                VrmProxy.SetValue(BlendShapePreset.E, LipValue / LipSyncSensitivity);
                Value = 0.0f;
                LipValue = 0.0f;
                break;
            case 13:
                VrmProxy.SetValue(BlendShapePreset.O, LipValue / LipSyncSensitivity);
                Value = 0.0f;
                LipValue = 0.0f;
                break;
            default:
                VrmProxy.SetValue(BlendShapePreset.A, 0);
                VrmProxy.SetValue(BlendShapePreset.I, 0);
                VrmProxy.SetValue(BlendShapePreset.U, 0);
                VrmProxy.SetValue(BlendShapePreset.E, 0);
                VrmProxy.SetValue(BlendShapePreset.O, 0);
                Value = 0.0f;
                LipValue = 0.0f;
                break;
        }
    }

    void Update()
    {
        //LキーでLipSyncのセットアップ
        if (Input.GetKeyDown(KeyCode.L))
        {
            VrmLipSyncSetup();
        }

        //Proxyが参照できなければLipSyncを開始しない
        if (VrmProxy == null) { return; }

        //VrmLipSyncIsActiveがtrueの時は、LipSyncを実行
        if (VrmLipSyncIsActive == true)
        {
            LipSyncConversion();
        }
    }

    //外部からLipSyncの有効/無効を切り替える
    public void LipSyncActiveSwitch()
    {
        VrmLipSyncIsActive = !VrmLipSyncIsActive;
        if (VrmLipSyncIsActive == true)
        {
            Debug.Log("VRMLipSync：有効");
        }
        else if (VrmLipSyncIsActive == false)
        {
            Debug.Log("VRMLipSync：無効");
        }
    }

}
