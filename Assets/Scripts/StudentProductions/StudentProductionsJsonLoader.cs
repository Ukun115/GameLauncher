using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Launcher
{
    public static class StudentProductionsJsonLoader
    {
        private const string FILE_NAME = "StudentProductionMaster.json";

        public static IEnumerator Load(Action<StudentProductions> onLoaded,Action<string> onError = null)
        {
            string path = Path.Combine(Application.streamingAssetsPath,FILE_NAME);

#if UNITY_ANDROID && !UNITY_EDITOR
            using var req = UnityWebRequest.Get(path);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"Jsonロード失敗: {req.error}\npath={path}");
                onLoaded?.Invoke(null);
                yield break;
            }

            string json = req.downloadHandler.text;
#else
            if(!File.Exists(path))
            {
                onError?.Invoke($"Jsonが存在しません: {path}");
                onLoaded?.Invoke(null);
                yield break;
            }
            string json = File.ReadAllText(path);
#endif
            var data = JsonUtility.FromJson<StudentProductions>(json);
            onLoaded?.Invoke(data);
        }
    }
}