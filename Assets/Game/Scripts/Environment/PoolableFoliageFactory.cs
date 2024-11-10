using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Tap_a_Tree.Environment
{
    public class PoolableFoliageFactory<T> where T : Component
    {
        private readonly ObjectPool<T> _pool;
        private readonly Func<T> _factoryMethod;

        private readonly Transform _parentContainer;

        public PoolableFoliageFactory(Func<T> createFunc, Transform parentContainer,
            int defaultCapacity = 10)
        {
            _pool = new ObjectPool<T>(
                createFunc: createFunc,
                actionOnGet: component => component.gameObject.SetActive(true),
                actionOnRelease: component => component.gameObject.SetActive(false),
                actionOnDestroy: component => Object.Destroy(component.gameObject),
                collectionCheck: false,
                defaultCapacity: defaultCapacity
            );

            _factoryMethod = createFunc;
            _parentContainer = parentContainer;
        }

        public PooledTransform<T> Create(Vector3 position)
        {
            T component = _pool.Get();
            component.transform.parent = _parentContainer;
            component.transform.position = position;

            return new PooledTransform<T>(component, _pool);
        }
    }

    public interface IPooledTransform
    {
        Transform Transform { get; }
        void Release();
    }

    public struct PooledTransform<T> : IPooledTransform where T : Component
    {
        public readonly T Value;
        private readonly ObjectPool<T> _pool;

        public Transform Transform { get; private set; }

        public PooledTransform(T value, ObjectPool<T> pool)
        {
            Value = value;
            _pool = pool;
            Transform = value.transform;
        }

        public void Release()
        {
            _pool.Release(Value);
        }
    }
}