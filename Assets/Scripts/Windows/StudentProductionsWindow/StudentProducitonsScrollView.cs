using UnityEngine;

namespace Launcher
{
    /// <summary>
    /// 学生作品群を表示するスクロールビュー
    /// </summary>
    public class StudentProducitonsScrollView : MonoBehaviour
    {
        [Header("学生作品マスターデータ"), SerializeField]
        private StudentProductionsMaster _masterData;

        [Header("セル"), SerializeField]
        private GameObject _cell;

        [Header("コンテンツトランスフォーム"), SerializeField]
        private Transform _contentTransform;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // 登録された学生作品分回す
            for(var id = 1;id <= _masterData.Entries.Count;id++)
            {
                // セル生成
                var cell = Instantiate(
                    _cell,
                    _contentTransform
                );
                // セル名を3桁のIDに設定
                cell.name = $"{id:D3}";
                // セルに情報設定
                var gameVideoCell = cell.GetComponent<GameVideoCell>();
                gameVideoCell.SetMovie();
                if(_masterData.TryGet(id,out var entry))
                {
                    gameVideoCell.SetText(entry.ProductionName,entry.StudentName);
                }
            }
        }
    }
}