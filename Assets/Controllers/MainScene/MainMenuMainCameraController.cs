using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.MainScene
{
    public class MainMenuMainCameraController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadSceneAsync("Scenes/EventsScene");
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
