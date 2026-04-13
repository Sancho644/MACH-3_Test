using Core.Records;
using TMPro;
using UnityEngine;

namespace UI.Windows.Records
{
    public class RecordItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date;
        [SerializeField] private TextMeshProUGUI points;
        [SerializeField] private float newRecordFontSize = 35f;

        public void Setup(RecordEntry entry)
        {
            date.text = entry.Date.ToString("dd/MM/yyyy");
            points.text = entry.Score.ToString();
            if (entry.NewRecord)
            {
                date.fontSize = newRecordFontSize;
                points.fontSize = newRecordFontSize;
            }
        }
    }
}