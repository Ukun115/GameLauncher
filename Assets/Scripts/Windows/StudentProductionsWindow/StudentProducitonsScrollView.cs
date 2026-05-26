using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Launcher
{
    /// <summary>
    /// 学生作品群を表示するスクロールビュー
    /// </summary>
    public class StudentProducitonsScrollView : MonoBehaviour
    {
        [Header("セル"), SerializeField]
        private GameObject _cell;

        [Header("コンテンツトランスフォーム"), SerializeField]
        private Transform _contentTransform;

        [Header("オフライン時エラーテキスト(ScreenCanvas/Text(TMP))"), SerializeField]
        private TMP_Text _offlineErrorText;

        private void Start()
        {
            if (_offlineErrorText != null)
                _offlineErrorText.gameObject.SetActive(false);

            var loader = MasterDataLoader.Instance;
            if (loader == null)
            {
                Debug.LogError("[StudentProducitonsScrollView] MasterDataLoader が見つかりません");
                return;
            }

            // 既にロード済みなら即表示、まだなら OnLoaded を待つ
            if (loader.Rows != null)
            {
                BuildCellsAsync(loader.Rows).Forget();
            }
            else
            {
                loader.OnLoaded += OnMasterDataLoaded;
            }
        }

        private void OnDestroy()
        {
            if (MasterDataLoader.Instance != null)
                MasterDataLoader.Instance.OnLoaded -= OnMasterDataLoaded;
        }

        private void OnMasterDataLoaded()
        {
            MasterDataLoader.Instance.OnLoaded -= OnMasterDataLoaded;
            BuildCellsAsync(MasterDataLoader.Instance.Rows).Forget();
        }

        private async UniTaskVoid BuildCellsAsync(StudentProductionRow[] rows)
        {
            if (rows == null)
            {
                ShowOfflineError();
                return;
            }

            // 動画を一括DLしてからセルを生成する
            if (Launch.Instance != null)
                await Launch.Instance.PrefetchVideosAsync(rows);
            BuildCells(rows);
        }

        private void BuildCells(StudentProductionRow[] rows)
        {
            // 既存セルをクリア（データ更新時の再構築に対応）
            foreach (Transform child in _contentTransform)
                Destroy(child.gameObject);

            foreach (var row in rows)
            {
                var cell = Instantiate(_cell, _contentTransform);
                cell.name = $"{row.ProductionID:D3}";

                var gameVideoCell = cell.GetComponent<GameVideoCell>();
                gameVideoCell.Initialize(row);
            }
        }

        private void ShowOfflineError()
        {
            if (_offlineErrorText == null) return;
            _offlineErrorText.text = "オフラインのため、ゲーム一覧を表示できません。\nインターネットに接続して再起動してください。";
            _offlineErrorText.gameObject.SetActive(true);
        }
    }
}
