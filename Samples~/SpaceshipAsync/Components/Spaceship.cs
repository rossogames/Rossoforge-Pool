using RossoForge.Pool.Data;
using RossoForge.Pool.Service;
using RossoForge.Services.Locator;
using UnityEngine;

namespace RossoForge.Pool.SpaceshipAsync
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField]
        private PooledObjectAsyncData _missilePoolData;

        private IPoolService _poolService;
        private float _speed = 5f;

        void Start()
        {
            _poolService = ServiceLocator.Get<IPoolService>();
            _poolService.PopulateAsync(_missilePoolData);
        }

        void Update()
        {
            Shoot();
            Move();
        }

        private void Shoot()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _poolService.GetAsync(_missilePoolData, transform.parent, transform.position, Space.World);
        }
        private void Move()
        {
            float axis = Input.GetAxis("Horizontal");
            transform.Translate(axis * Time.deltaTime * _speed * Vector3.right, Space.World);
        }
    }
}