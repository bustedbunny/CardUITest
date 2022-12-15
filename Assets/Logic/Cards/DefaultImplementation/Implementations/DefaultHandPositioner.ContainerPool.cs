using UnityEngine;
using UnityEngine.Pool;

namespace CardUITest.Cards.DefaultImplementation.Implementations
{
    public partial class DefaultHandPositioner
    {
        private static class ContainerPool
        {
            private const int DefaultDeckCapacity = 12;

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void Init()
            {
                _pool = new ObjectPool<Transform>(Create, OnGet, OnRelease, OnDestroy,
                    defaultCapacity: DefaultDeckCapacity);
            }

            private static void OnDestroy(Transform obj)
            {
                if (obj is not null)
                {
                    Object.Destroy(obj);
                }
            }

            private static void OnGet(Transform obj)
            {
                obj.gameObject.SetActive(true);
#if UNITY_EDITOR
                obj.hideFlags = HideFlags.None;
#endif
            }

            private static void OnRelease(Transform obj)
            {
                obj.parent = null;
                obj.localPosition = Vector3.zero;
                obj.localRotation = Quaternion.identity;
                obj.localScale = Vector3.one;
                obj.gameObject.SetActive(false);

#if UNITY_EDITOR
                obj.hideFlags = HideFlags.HideInHierarchy;
#endif
            }

            private static Transform Create()
            {
                return new GameObject().transform;
            }

            private static ObjectPool<Transform> _pool;

            public static Transform Get(Transform parent)
            {
                var pooled = _pool.Get();
                pooled.SetParent(parent);
                return pooled;
            }

            public static void Release(Transform obj)
            {
                _pool.Release(obj);
            }
        }
    }
}