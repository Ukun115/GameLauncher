using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Launcher
{
    public class LogoIntro : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private float _displayDuration = 2f;
        [SerializeField] private float _fadeOutDuration = 1f;

        private void Start()
        {
            PlayAsync().Forget();
        }

        private async UniTaskVoid PlayAsync()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _canvasGroup.alpha = 0f;

            _canvasGroup.DOFade(1f, _fadeInDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeInDuration), cancellationToken: token);

            await UniTask.Delay(TimeSpan.FromSeconds(_displayDuration), cancellationToken: token);

            _canvasGroup.DOFade(0f, _fadeOutDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeOutDuration), cancellationToken: token);

            SceneManager.LoadScene("Launch");
        }
    }
}
