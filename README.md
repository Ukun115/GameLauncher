# 【ゲーム追加フロー】
## 【準備物編】
- ROM(zip)
  - Releaseでビルドしたやつ
  - エラー落ちしないかどうか必ず確認
- ゲーム動画
  - PVが望ましい
  - 100MB以下の制限あり。60sくらいの動画だったら大丈夫なはず。
 
## 【GitGUI編】
- 下記リンクからリポジトリを落としてくる。
  - https://github.com/Ukun115/GameLauncher
  - privateアクセスなので、伊関からコラボレーターとしてリポジトリに参加しないと「404 not found」が出てきます。
  - 【注意】zipをDLしてこないよう注意。リポジトリをクローンしてください。
- GitGUIはおそらく「Fork」を使用するはず
- 最新mainから作業ブランチ作成
  - ブランチ名は「苗字/作業内容」
  - ex) iseki/addGames

![alt text](ReadmeImages/image-2.png)

- UnityHubからGameLauncher起動。
  - 起動出来たらAssets/Scenes/Launch.unityからシーンを開く。
  - 【注意】Unity6000.0.58f2を使用しています。下記リンクからDLしましょう。
  - https://unity.com/ja/releases/editor/whats-new/6000.0.58f2

- Assets/Resources/Videosの中にゲーム動画を格納
  - 動画ファイル名をゲームのナンバリングに沿って命名変更
  - ex) Video013.mp4

![alt text](ReadmeImages/image-3.png)

- Assets/StudentProductions/MasterData/StudentProductionMaster.jsonの最後尾にマスターデータを追加
  - 書き方は既存のマスターデータをマネてください。
  - マスターデータの各項目は基本持たせてください。無い場合はnullでOK。

![alt text](ReadmeImages/image-4.png)

![alt text](ReadmeImages/image-5.png)

- Assets/StudentProductions/MasterData/StudentProductionsMaster.assetを削除
  - 古いScriptableObjectなので、Jsonに追加したマスターデータを含むScriptableObjectを作らないといけない

![alt text](ReadmeImages/image-1.png)

- 下の画像からAssets/StudentProductions/MasterData/StudentProductionMaster.jsonを選択しJsonデータをインポート。新しいScriptableObjectを作成。

![alt text](ReadmeImages/image.png)

![alt text](ReadmeImages/image-6.png)

- Launch.sceneのヒエラルキービューのMainCanvas/VersionTextオブジェクトのTextMeshPro-Textのテキストを次のバージョン表記に変更し、シーンを保存。

![alt text](ReadmeImages/image-7.png)

![alt text](ReadmeImages/image-8.png)

- 以上でUnity側の設定は終わり。

- 次にGitHub側の設定を行う。

- 下記リポジトリへアクセス
  - https://github.com/Ukun115/StudentProductions

- Releasesをクリック

![alt text](ReadmeImages/image-9.png)

- 「Draft a new release」をクリック

![alt text](ReadmeImages/image-10.png)

- タグ作成。「Create new tag」から新しいタグを作成。
  - ゲームごとに3桁でナンバリングしています。最新のナンバリングに+1した半角の値にしてください。

![alt text](ReadmeImages/image-11.png)

![alt text](ReadmeImages/image-12.png)

- タイトルはタグと同じく番号を半角入力。

![alt text](ReadmeImages/image13.png)

- 概要記載
  - 卒年、作品名、制作者の3点を同じ形式で記入してください。
  
【サンプル】

卒年：27卒

作品名：KAWAHARA GAME

制作者：河原 太郎

![alt text](ReadmeImages/image-14.png)

- ROM(zip)を画像の箇所にD&D。
  - この時、ROM(zip)のフォルダ名はナンバリングと同じ名前にしておく。
  - ex) ナンバリングが013の場合、「013.zip」とする
  - アップロードには少々時間かかる

![alt text](ReadmeImages/image-15.png)

- アップロード完了したら、「Publish release」を押下

![alt text](ReadmeImages/image-16.png)

- 以上でゲームの格納は完了。

- 次に実機でのゲーム起動確認を行う。