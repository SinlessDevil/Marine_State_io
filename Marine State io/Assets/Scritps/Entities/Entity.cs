using System.Collections;
using UnityEngine;
using TMPro;
using Scripts.Islands;
using Extensions;

namespace Scripts.Entities
{
    public abstract class Entity : Visitor
    {
        [SerializeField] private int _countUnit;
        public int CountUnit
        {
            get { return _countUnit; }
            set
            {
                _countUnit = value;
                UpdateTextCountUnit(_countUnit);
            }
        }
        public int maxCountUnit = 25;

        private TMP_Text _textCountUnit;
        public Island Island { get; private set; }

        public IEnumerator coroutine;

        private void Awake()
        {
            coroutine = AccumulationUnitRoutine();
        }

        public void Initialize(int countUnit)
        {
            InitComponents();
            CountUnit = countUnit;
            Asserts();
        }
        private void InitComponents()
        {
            Island = GetComponent<Island>();
            _textCountUnit = GetComponentInChildren<TMP_Text>();
        }
        private void UpdateTextCountUnit(int countUnit)
        {
            if(_textCountUnit != null)
                _textCountUnit.text = countUnit.ToString();
        }
        private void Asserts()
        {
            _textCountUnit.LogErrorIfComponentNull();
        }

        public void StartAccumulationUnit()
        {
            if(coroutine != null)
                StartCoroutine(coroutine);
        }

        public void StopAccumulationUnit()
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        public IEnumerator AccumulationUnitRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.75f);

                if(CountUnit < maxCountUnit)
                    CountUnit++;
            }
        }
    }
}