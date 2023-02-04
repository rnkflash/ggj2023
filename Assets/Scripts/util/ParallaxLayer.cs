using UnityEngine;

public class ParallaxLayer : MonoBehaviour
    {
        public Vector3 movementScale = Vector3.one;

        private float width;
        private Transform _camera;

        void Awake()
        {
            _camera = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.position = Vector3.Scale(new Vector3(_camera.position.x, _camera.position.y, 0), movementScale);
        }

    }