using UnityEngine;

namespace Rossoforge.Pool.SpaceshipAsync
{
    public class Missile : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        private float _speed = 10f;

        void Start()
        {

        }

        private void Update()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.up, Space.World);
            OutOfBounds();
        }

        private void OutOfBounds()
        {
            if (!_renderer.isVisible)
                gameObject.SetActive(false); // return to pool
        }
    }
}