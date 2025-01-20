using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Connection _photonConnector;
    [SerializeField] private Image image;
    [SerializeField] private int sceneIndexToLoad;

    private void Start()
    {
        _photonConnector.OnPhotonConnected += OnPhotonConnected;
        _photonConnector.Connect();
    }

    private void OnPhotonConnected()
    {
        StartCoroutine(LoadSceneAsync(sceneIndexToLoad));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        SetGraphicsQuality();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            if (image != null)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                image.fillAmount = progress;
            }

            yield return null;
        }
    }
    private void SetGraphicsQuality()
    {
        int index = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.QualityKey, QualitySettings.GetQualityLevel());

        QualitySettings.SetQualityLevel(index, true);
        Debug.Log($"Graphics quality set to: {QualitySettings.names[index]}");
    }
}