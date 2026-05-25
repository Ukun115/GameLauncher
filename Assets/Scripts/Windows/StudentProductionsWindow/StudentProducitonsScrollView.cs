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

        private void Start()
        {
            var loader = MasterDataLoader.Instance;
            if (loader == null)
            {
                Debug.LogError("[StudentProducitonsScrollView] MasterDataLoader が見つかりません");
                return;
            }

            // 既にロード済みなら即表示、まだなら OnLoaded を待つ
            if (loader.Rows != null)
            {
                BuildCells(loader.Rows);
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
            BuildCells(MasterDataLoader.Instance.Rows);
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
                gameVideoCell.Initialize(row.ProductionID, row.GameName, row.StudentName, row.VideoFileId, row.ExeFileId);
            }
        }
    }
}
