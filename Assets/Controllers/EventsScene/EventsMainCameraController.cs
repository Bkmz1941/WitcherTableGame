using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.EventsScene
{
    public class EventsMainCameraController : MonoBehaviour
    {
        private Camera _camera;
        private float _willageY = 0f;
        private float _cityY = 90f;
        private bool isCity = false;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void ChangeDirectional()
        {
            if (!isCity) _camera.transform.Rotate(new Vector3(0, -90f, 0));
            else _camera.transform.Rotate(new Vector3(0, 90f, 0));

            isCity = !isCity;
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
