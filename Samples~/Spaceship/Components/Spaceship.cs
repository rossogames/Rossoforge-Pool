using RossoForge.Core.Pool;
using RossoForge.Pool.Data;
using RossoForge.Services;
using UnityEngine;

namespace RossoForge.Pool.Spaceship
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField]
        private PooledGameobjectData _missilePoolData;

        private IPoolService _poolService;
        private float _speed = 5f;

        void Start()
        {
            _poolService = ServiceLocator.Get<IPoolService>();
            //_poolService.Populate(_missilePoolData);
        }

        void Update()
        {
            Shoot();
            Move();
        }

        private void Shoot()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _poolService.Get(_missilePoolData, transform.parent, transform.position, Space.World);
        }
        private void Move()
        {
            float axis = Input.GetAxis("Horizontal");
            transform.Translate(axis * Time.deltaTime * _speed * Vector3.right, Space.World);
        }
    }
}