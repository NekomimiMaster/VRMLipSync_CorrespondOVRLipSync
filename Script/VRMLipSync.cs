using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class VRMLipSync : MonoBehaviour {

    //LipSyncTargetNameにはLipSyncを適用させるVRMアバターのGameObject名を入れます。
    //公式サンプルのVRMローダーでは VRM という名前でシーンに配置されるので...
    //デフォルト設定は VRM になってます。
    [SerializeField]
    private string LipSyncTargetName = "VRM";

    private VRMBlendShapeProxy VrmProxy;
    private GameObject VrmAvatar;
    private OVRLipSyncContextMorphTarget morphTarget;

    //LipSyncの感度を調整 0.1が鈍感 2.0が敏感
    [SerializeField]
    [Range(0.1f, 2.0f)]
    private float LipSyncSensitivity = 1.0f;
    
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
        VrmAvatar = GameObject.Find(LipSyncTargetName);
        VrmProxy = GetVrmProxy(VrmAvatar);
        if (VrmProxy == null) { return; }

        if (VrmAvatar.GetComponent<AudioSource>() == null)
        {
            VrmAvatar.AddComponent<AudioSource>();
        }

        if (VrmAvatar.GetComponent<OVRLipSyncMicInput>() == null)
        {
            VrmAvatar.AddComponent<OVRLipSyncMicInput>();
        }

        if (VrmAvatar.GetComponent<OVRLipSyncContext>() == null)
        {
            VrmAvatar.AddComponent<OVRLipSyncContext>();
        }

        if (VrmAvatar.GetComponent<OVRLipSyncContextMorphTarget>() == null)
        {
            VrmAvatar.AddComponent<OVRLipSyncContextMorphTarget>();
        }

        morphTarget = VrmAvatar.GetComponent<OVRLipSyncContextMorphTarget>();
        Debug.Log("VrmAvatarにLipSyncをセットアップしました。");
    }
    
    //nameでVRMを検索してVRMBlendShapeProxyを取得する。
    private VRMBlendShapeProxy GetVrmProxy(GameObject vrmAvatar)
    {
        VRMBlendShapeProxy vrmProxy = null;

        vrmProxy = vrmAvatar.GetComponent<VRMBlendShapeProxy>();
        //存在しない場合のみチェック
        if (vrmProxy == null)
        {
            Debug.Log("VRM に VRMBlendShapeProxy がアタッチされていません。再度セットアップしてください");
            return null;
        }
        Debug.Log("VRM の VRMBlendShapeProxy を取得しました");
        return vrmProxy;
    }

    //OVRLiySyncの処理を VRM の VRMBlendShapeProxy に対応させる
    private void LipSyncConversion()
    {
        int LipType = 0;
        float LipValue = 0.0f;
        float Value;
        // VRMLipValue[0] は「無音時に1を返す処理」の為、iに0を含めない
        for (int i = 1; i < 15; i++)
        {
            Value = morphTarget.VRMLipValue[i];
            //1番大きい値の時にLipTypeを更新
            if (LipValue < Value && i != 0)
            {
                LipValue = Value;
                LipType = i;
            }
        }

        switch (LipType)
        {
            case 10:
                VrmProxy.SetValue(BlendShapePreset.A, LipValue / LipSyncSensitivity);
                break;
            case 12:
                VrmProxy.SetValue(BlendShapePreset.I, LipValue / LipSyncSensitivity);
                break;
            case 14:
                VrmProxy.SetValue(BlendShapePreset.U, LipValue / LipSyncSensitivity);
                break;
            case 11:
                VrmProxy.SetValue(BlendShapePreset.E, LipValue / LipSyncSensitivity);
                break;
            case 13:
                VrmProxy.SetValue(BlendShapePreset.O, LipValue / LipSyncSensitivity);
                break;
            default:
                VrmProxy.SetValue(BlendShapePreset.A, 0);
                VrmProxy.SetValue(BlendShapePreset.I, 0);
                VrmProxy.SetValue(BlendShapePreset.U, 0);
                VrmProxy.SetValue(BlendShapePreset.E, 0);
                VrmProxy.SetValue(BlendShapePreset.O, 0);
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
