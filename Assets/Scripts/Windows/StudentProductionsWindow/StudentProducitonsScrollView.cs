using UnityEngine;

namespace Lancher
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
            for (var id = 0; id < _masterData.Entries.Count; id++)
            {
                // セル生成
                Instantiate(
                    _cell,
                    _contentTransform
                );

                if (_masterData.TryGet(id, out var entry))
                {
                    Debug.Log($"ID: {entry.ProductionID}, Name: {entry.GameName}");
                }
            }
        }
    }
}