// Copyright (c) 2012-2019 FuryLion Group. All Rights Reserved.

using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

// TODO раскомментировать при наличии DOTween  
using DG.Tweening;

namespace FuryLion.UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _logo;
        [SerializeField] private SpriteRenderer _background;

        private IEnumerator Start()
        {
            DOTween.Init();

            Application.targetFrameRate = 60;
            
            while (!UnityEngine.Rendering.SplashScreen.isFinished)
                yield return null;

            var remainingTime = 2.0f - Time.realtimeSinceStartup;
            if (remainingTime > 0f)
                yield return new WaitForSeconds(remainingTime);

            yield return SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

            yield return PageManager.LoadLoadingPage();

            LoadingPageComplete();
        }

        private void LoadingPageComplete()
        {
            // TODO раскомментировать при наличии DOTween
            _logo.DOFade(0.3f, 0.0f).OnComplete(() =>
            {
                PageManager.OpenLoadingPage();

                SceneManager.UnloadSceneAsync("Splash");

                _background.DOFade(0.3f, 0.0f);
            });
        }
    }
}