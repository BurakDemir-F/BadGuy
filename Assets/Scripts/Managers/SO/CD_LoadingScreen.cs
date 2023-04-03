using UnityEngine;

namespace Managers.SO
{
    [CreateAssetMenu(menuName = "Data/Config/LoadingScreen", fileName = "CD_LoadingScreen", order = 0)]
    public class CD_LoadingScreen : ScriptableObject
    {
        public float FadeDuration;
        public LoadingScreenData MissionPoisonLoading;
        public LoadingScreenData MobBakeryLoading;
    }

    [System.Serializable]
    public class LoadingScreenData
    {
        public Sprite LoadingSprite;
        public GameObject LoadingObject;
    }
}