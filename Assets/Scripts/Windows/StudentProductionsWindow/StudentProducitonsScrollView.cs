using UnityEngine;

namespace Launcher
{
    public class StudentProducitonsScrollView : MonoBehaviour
    {
        [SerializeField] private GameObject _cell;
        [SerializeField] private Transform _contentTransform;

        private void Start()
        {
            StartCoroutine(StudentProductionsJsonLoader.Load(OnLoaded,Debug.LogError));
        }

        private void OnLoaded(StudentProductions data)
        {
            if(data?.Rows == null)
            {
                Debug.LogError("JsonのRowsがnullです。Json形式を確認してください。");
                return;
            }

            foreach(var row in data.Rows)
            {
                var cell = Instantiate(_cell,_contentTransform);
                cell.name = $"{row.ProductionID:D3}";

                var gameVideoCell = cell.GetComponent<GameVideoCell>();
                gameVideoCell.SetMovie();
                gameVideoCell.SetText(row.GameName,row.StudentName);
            }
        }
    }
}