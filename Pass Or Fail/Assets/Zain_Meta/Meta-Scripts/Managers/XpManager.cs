using System.Collections;
using AssetKits.ParticleImage;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class XpManager : MonoBehaviour
    {
        public static XpManager Instance;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            xpSlider.minValue = 0;
            xpSlider.maxValue = xpOffsetPerRank;
            GetRankData();
        }

        [SerializeField] private ParticleImage xpParticles;
        [SerializeField] private Text rankText;
        [SerializeField] private Text curXpText;
        [SerializeField] private int xpOffsetPerRank, curXpCount, myRankCount;
        [SerializeField] private Slider xpSlider;
        private readonly YieldInstruction _delay = new WaitForSeconds(.5f);

        private void OnEnable()
        {
            xpParticles.onStop.AddListener(IncreaseXpOnComplete);
        }

        private void OnDisable()
        {
            xpParticles.onStop.RemoveListener(IncreaseXpOnComplete);
        }

        private void GetRankData()
        {
            myRankCount = PlayerPrefs.GetInt("RankNumber", 1);
            curXpCount = PlayerPrefs.GetInt("XpAmount", 0);
            xpSlider.value = curXpCount;
            SetText();
        }

        public void AddXp(int amount)
        {
            print("Give Xp of " + amount);
            curXpCount += amount;
            xpParticles.SetBurst(0, 0, amount);
            StartCoroutine(nameof(PlayTheParticlesWithNormalDelay));
        }

        private IEnumerator PlayTheParticlesWithNormalDelay()
        {
            yield return _delay;
            xpParticles.Play();
        }

        private void IncreaseXpOnComplete()
        {
            DOVirtual.Float(xpSlider.value, curXpCount, .5f,
                (valueToGive => { xpSlider.value = valueToGive; })).OnComplete(() =>
            {
                if (curXpCount >= xpOffsetPerRank)
                {
                    curXpCount = 0;
                    myRankCount++;
                }

                SetText();
            });
        }

        private void SaveRankData()
        {
            PlayerPrefs.SetInt("RankNumber", myRankCount);
            PlayerPrefs.SetInt("XpAmount", curXpCount);
        }

        private void SetText()
        {
            rankText.text = myRankCount.ToString();
            curXpText.text = curXpCount + "/" + xpOffsetPerRank;
            SaveRankData();
        }
    }
}