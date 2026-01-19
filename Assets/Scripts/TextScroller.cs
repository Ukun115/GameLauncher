using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Launcher
{
    /// <summary>
    /// テキストスクロール
    /// NOTE:iseki TMP_Textがアタッチされているオブジェクト用
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class TextScroller : MonoBehaviour
    {
        [Header("スクロール速度"), SerializeField]
        private float _scrollSpeed = 100f;

        [Header("スクロール終了判定までのマージン、0だと全て表示された時点で終了する"), SerializeField]
        private float _scrollFinishLineAddValue = 50;

        [Header("スクロール開始前の待機時間"), SerializeField]
        private float _waitTimeBeforeScroll = 2f;

        [Header("スクロール完了後、フェードアウトするまでの待機時間"), SerializeField]
        private float _waitTimeAfterScroll = 2f;

        [Header("テキストのフェードインアウト時間"), SerializeField]
        private float _fadeDuration = 0.4f;

        [Header("フェードアウトしてからフェードインするまでの待機時間"), SerializeField]
        private float _waitTimeFade = 0.2f;

        [Header("コンテンツサイズフィッター"), SerializeField]
        private ContentSizeFitter _contentSizeFitter;

        [Header("キャンバスグループ"), SerializeField]
        private CanvasGroup _canvasGroup;

        [Header("テキストのRectTransform"), SerializeField]
        private RectTransform _textRectTransform;

        [Header("親のRectTransform"), SerializeField]
        private RectTransform _parentRectTransform;

        /// <summary>
        /// スタート位置
        /// </summary>
        private Vector3 _startPosition;

        /// <summary>
        /// スクロールが停止する位置、scrollValueがこの値を超えたら停止する
        /// </summary>
        private float _finishLineValue;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            if(!TryGetComponent<TMP_Text>(out var textComponent))
            {
                Debug.LogWarning("TMP_Textがアタッチされていないのでスクロールは機能しません");
                return;
            }

            // ContentSizeFitter手動更新
            UpdateContentSizeFitter();

            // スクロール開始
            StartScroll();
        }

        /// <summary>
        /// スクロール開始
        /// </summary>
        private void StartScroll()
        {
            // テキスト幅と親幅取得
            var textWidth = _textRectTransform.rect.width;
            var parentWidth = _parentRectTransform.rect.width;

            // スクロール位置計算
            _startPosition = _textRectTransform.anchoredPosition;
            _finishLineValue = _startPosition.x - (textWidth + _startPosition.x + _scrollFinishLineAddValue - parentWidth);

            if(textWidth + _startPosition.x > parentWidth)
            {
                ScrollTask(this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        /// <summary>
        /// スクロール処理タスク
        /// </summary>
        private async UniTaskVoid ScrollTask(CancellationToken token)
        {
            while(true)
            {
                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(_waitTimeBeforeScroll),cancellationToken: token);

                // スクロール
                await _textRectTransform.DOAnchorPosX(_finishLineValue,_scrollSpeed).SetSpeedBased().SetEase(Ease.Linear).ToUniTask(cancellationToken: token);

                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(_waitTimeAfterScroll),cancellationToken: token);

                // フェードイン
                await _canvasGroup.DOFade(0,_fadeDuration).SetEase(Ease.Linear);

                // 位置リセット
                _textRectTransform.anchoredPosition = _startPosition;

                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(_waitTimeFade),cancellationToken: token);

                // フェードアウト
                await _canvasGroup.DOFade(1,_fadeDuration).SetEase(Ease.Linear).ToUniTask(cancellationToken: token);
            }
        }

        /// <summary>
        /// ContentSizeFitter手動更新
        /// </summary>
        private void UpdateContentSizeFitter()
        {
            _contentSizeFitter.SetLayoutHorizontal();
            _contentSizeFitter.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_textRectTransform);
        }
    }
}