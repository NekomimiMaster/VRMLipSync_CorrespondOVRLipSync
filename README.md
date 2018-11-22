# VRMLipSync_CorrespondOVRLipSync
OVRLipSyncをVRMアバターに適用させます。<br>
・実行中に読み込んだVRMアバターに対して、OVRLipSyncを動かすのに必要なコンポーネントを動的にアタッチします。<br>
・OVRLipSyncContextMorphTargetからLipSyncの状態を読み取り、VRMのLipSyncとして適用します。<br>
・詳細は<a href="https://qiita.com/Nekomasu/items/104808f7ecd26f1e362a">VRMモデルをOVRLipSyncに対応させる方法</a> に載ってます。<br>
<br>
# 下記のライブラリに依存しています。<br>
１．<a href="https://developer.oculus.com/downloads/package/oculus-lipsync-unity/">Oculus Lipsync Unity 1.28.0</a><br>
２．<a href="https://github.com/dwango/UniVRM/releases">UniVRM</a><br>
※事前にインポートしてください。<br>
<br>
# 使用方法等<br>
<a href="https://qiita.com/Nekomasu/items/104808f7ecd26f1e362a">VRMモデルをOVRLipSyncに対応させる方法</a> を参考にしてセットアップしてください。<br>
また、実行中にVRMを読み込んだ場合は「L」キーを押すと、処理がはじまります。<br>
(もしくは、VrmLipSyncSetup()をUIなどから呼んでください。)<br>
