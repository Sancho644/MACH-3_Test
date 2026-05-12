using Core.Yandex.Services;
using TMPro;
using UnityEngine;

namespace UI.Windows.Records
{
    public class RecordItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date;
        [SerializeField] private TextMeshProUGUI points;

        public void Setup(RecordEntry entry)
        {
            date.text = entry.name;
            points.text = entry.score.ToString();
        }
    }
}