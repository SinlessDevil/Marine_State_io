using UnityEngine;

namespace Scripts.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Level", fileName = "LevelConfig", order = 1)]
    public class LevelStaticData : ScriptableObject
    {
        [Header("------ Islands ------")]
        public Sprite[] islandSprites;
        [Space(10)]
        [Header("------ Players ------")]
        public Sprite[] iconPlayerSprites;
        public Color[] PlayerColors;
        [Space(10)]
        [Header("------ Island Empty by Default ------")]
        public int[] countsUnitByDefault;
        public Color IslandByDefaultColor;
        [Header("------ Main Player ------")]
        [Space(10)]
        public Sprite mainIconPlayerSprite;
        public Color mainPlayerColor;
    }
}