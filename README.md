# my-anime - Isekai Virtual Space

## 実装内容
- `Assets/Scripts/WorldBuilder.cs`
  - Unityシーンで異世界仮想空間を自動生成するスクリプト
- `Assets/Editor/WorldBuilderMenu.cs`
  - Unityエディタ上のメニューから空間を生成できる
- `.gitignore`
  - Unityプロジェクト用の一般的な無視設定を追加

## 使い方
1. Unity 6.0.4.8f1 でこのプロジェクトを開きます。
2. `Assets/MainScene.unity` を開きます。
3. Unityメニューから `Tools > Isekai Virtual World > Complete Setup (World + Particles + Camera)` を選択します。
   - これにより、世界生成、パーティクル、カメラ、サウンド、異世界スカイボックスがすべてセットアップされます。
4. `Tools > Isekai Virtual World > Ensure Main Camera` を実行して、`Main Camera` と `AudioListener` を確実に作成します。
5. `File > Save` でシーンを保存します。
6. `Play` を押して、異世界空間を確認します。

## 追加メニュー
- `Tools > Isekai Virtual World > Create Virtual Space`
  - 基本的な世界生成を実行します。
- `Tools > Isekai Virtual World > Add Auto-Generation`
  - シーン起動時に自動生成する `Isekai Auto Setup` を作成し、すぐに世界を生成します。
- `Tools > Isekai Virtual World > Ensure Main Camera`
  - `Main Camera` と `AudioListener` を強制的に作成します。

## 注意点
- このプロジェクトは現在 GitHub に接続済みです。最新の変更はリモート `origin` にプッシュされています。
- Unity は変更を検出して自動コンパイルします。再起動は通常不要です。
