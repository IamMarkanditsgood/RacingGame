using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
}