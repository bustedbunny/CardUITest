using CardUITest.Cards.Hand;
using UnityEngine;

namespace CardUITest.Cards.DefaultImplementation.Implementations
{
    [DisallowMultipleComponent]
    public partial class DefaultHandPositioner : MonoBehaviour, IHandPositionProvider
    {
        [SerializeField] private float spacing = 1f;
        [SerializeField] private float occupiedAngle = 10f;
        [SerializeField] private float maxCardCount = 12f;
        [SerializeField] private float minMod = 0.5f;
        [SerializeField] private float maxMod = 0.9f;

        [SerializeField] private Transform rotCont;
        [SerializeField] private Transform posCont;

        public Transform CreatePosition() => ContainerPool.Get(rotCont);

        public void RemovePosition(Transform position) => ContainerPool.Release(position);

        public void UpdatePositions()
        {
            var tran = rotCont.transform;
            var count = tran.childCount;

            if (count == 0) return;

            CalculateRadius(count, out var radius);

            var stepAngle = occupiedAngle / count;

            for (int i = 0; i < count; i++)
            {
                var child = tran.GetChild(i);

                // Making it negative makes deck holding reversed
                var angle = -stepAngle * i;
                // Since we start at 0 angle (which equals Vector2.right) we do a 90 degrees offset to make it Vector2.up
                var angleRad = Mathf.Deg2Rad * (angle + 90f);

                // x,y of point of circle in local space
                var x = radius * Mathf.Cos(angleRad);
                var y = radius * Mathf.Sin(angleRad);

                // Z here is offset for proper draw order
                var pos = new Vector3(x, y, 0.1f * (count - 1 - i));
                var rot = Quaternion.Euler(0f, 0f, angle);

                child.localPosition = pos;
                child.localRotation = rot;
            }

            tran.localRotation = Quaternion.Euler(0f, 0f, (occupiedAngle - occupiedAngle / count) / 2f);
            posCont.localPosition = new Vector3(0f, -radius, 0f);
        }

        private void CalculateRadius(int count, out float radius)
        {
            // This gets a multiplier for card width on virtual circle
            var mod = Mathf.Lerp(maxMod, minMod, count / maxCardCount);

            // Here we calculate required lenght for whole held deck
            var requiredLength = mod * spacing * count;

            // Part of circle which gets occupied by deck
            var occupiedAnglePart = 360f / occupiedAngle;

            // Result of this calculation is radius of circle that gives a proper arc pattern for deck
            radius = requiredLength * occupiedAnglePart / Mathf.PI;
        }

        private void OnDrawGizmosSelected()
        {
            var tran = rotCont.transform;
            var count = tran.childCount;

            if (count == 0) return;

            CalculateRadius(count, out var radius);

            Gizmos.DrawWireSphere(posCont.position, radius);
        }
    }
}