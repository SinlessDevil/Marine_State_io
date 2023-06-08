using Scripts.Islands;
using UnityEngine;

namespace Scripts.TouchControl
{
    public class PointTouch : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent(out Island entity)){
                entity.outline.SetColorByTarget();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Island entity))
            {
                entity.outline.SetColorByDefault();
            }
        }
    }
}