using UnityEngine;

namespace Scripts.UI
{
    public class GameHud : MonoBehaviour
    {
        [field: SerializeField] public GameObject PanelLoader { get; private set; }
        [field: SerializeField] public GameObject PanelSettingsLevel { get; private set; }
    }
}