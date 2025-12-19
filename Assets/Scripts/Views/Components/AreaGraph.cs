using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components
{
    public class AreaGraph : Graphic
    {
        [Header("Data")]
        [SerializeField] List<float> _values;
        [Space]
        [Header("Visual Padding")]
        [Range(0f, 0.4f)] public float _topPadding = 0.3f;
        [Range(0f, 0.4f)] public float _bottomPadding = 0.3f;
        [Space]
        [Header("Corner Rounding")]
        [Range(0f, 30f)] public float _cornerRadius = 16f;
        [Range(2, 8)] public int _cornerSegments = 4;
        [Header("Zero values content")]
        [SerializeField] private RectTransform _zeroValuesContent;

        public void SetValues(IEnumerable<float> newValues)
        {
            _values = new List<float>(newValues);
            _zeroValuesContent.gameObject.SetActive(_values.Count < 2);
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (_values == null || _values.Count < 2) return;

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            float min = _values.Min();
            float max = _values.Max();

            float paddedMin = min - (max - min) * _bottomPadding;
            float paddedMax = max + (max - min) * _topPadding;

            List<Vector2> rawPoints = BuildRawPoints(width, height, paddedMin, paddedMax);
            List<Vector2> roundedPoints = RoundCorners(rawPoints);

            DrawArea(vh, roundedPoints);
        }

        List<Vector2> BuildRawPoints(float width, float height, float min, float max)
        {
            List<Vector2> points = new();
            for (int i = 0; i < _values.Count; i++)
            {
                float x = (i / (float)(_values.Count - 1)) * width;
                float y = Mathf.InverseLerp(min, max, _values[i]) * height;
                points.Add(new Vector2(x, y));
            }
            return points;
        }

        List<Vector2> RoundCorners(List<Vector2> pts)
        {
            List<Vector2> result = new();
            result.Add(pts[0]);

            for (int i = 1; i < pts.Count - 1; i++)
            {
                Vector2 prev = pts[i - 1];
                Vector2 curr = pts[i];
                Vector2 next = pts[i + 1];

                Vector2 dirA = (curr - prev).normalized;
                Vector2 dirB = (next - curr).normalized;

                float distA = Vector2.Distance(prev, curr);
                float distB = Vector2.Distance(curr, next);

                float r = Mathf.Min(_cornerRadius, distA * 0.4f, distB * 0.4f);

                Vector2 p1 = curr - dirA * r;
                Vector2 p2 = curr + dirB * r;

                result.Add(p1);

                for (int s = 1; s <= _cornerSegments; s++)
                {
                    float t = s / (float)(_cornerSegments + 1);
                    result.Add(QuadraticBezier(p1, curr, p2, t));
                }

                result.Add(p2);
            }

            result.Add(pts[^1]);
            return result;
        }

        Vector2 QuadraticBezier(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            return Vector2.Lerp(Vector2.Lerp(a, b, t), Vector2.Lerp(b, c, t), t);
        }

        void DrawArea(VertexHelper vh, List<Vector2> points)
        {
            Vector2 prevTop = Vector2.zero;
            Vector2 prevBottom = Vector2.zero;

            for (int i = 0; i < points.Count; i++)
            {
                Vector2 top = points[i];
                Vector2 bottom = new Vector2(top.x, 0);

                if (i > 0)
                    AddQuad(vh, prevBottom, prevTop, top, bottom);

                prevTop = top;
                prevBottom = bottom;
            }
        }

        void AddQuad(VertexHelper vh, Vector2 bl, Vector2 tl, Vector2 tr, Vector2 br)
        {
            int i = vh.currentVertCount;

            vh.AddVert(bl, color, Vector2.zero);
            vh.AddVert(tl, color, Vector2.zero);
            vh.AddVert(tr, color, Vector2.zero);
            vh.AddVert(br, color, Vector2.zero);

            vh.AddTriangle(i, i + 1, i + 2);
            vh.AddTriangle(i, i + 2, i + 3);
        }
    }
}
