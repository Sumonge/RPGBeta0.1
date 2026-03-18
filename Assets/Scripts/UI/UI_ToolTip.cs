using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    // Tooltip 的 RectTransform（可在 Inspector 指定，若为空则使用当前组件的 RectTransform）
    [SerializeField] private RectTransform skillToolTip;

    // 最小像素偏移（当 Canvas 很小或未按比例时作为下限）
    [SerializeField] private float minXOffset = 32f;
    [SerializeField] private float minYOffset = 32f;

    // 相对于 Canvas 的偏移比例（默认 8%）
    [SerializeField, Range(0.01f, 0.3f)] private float canvasOffsetRatioX = 0.08f;
    [SerializeField, Range(0.01f, 0.3f)] private float canvasOffsetRatioY = 0.08f;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        // 尝试获取 tooltip 的 RectTransform
        RectTransform tooltipRT = skillToolTip ?? GetComponent<RectTransform>();
        if (tooltipRT == null)
            return;

        // 获取父级 Canvas（若没有则退化为屏幕坐标设置）
        Canvas canvas = tooltipRT.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            // 退化方案：直接使用屏幕坐标（保持兼容）
            tooltipRT.position = mousePosition + new Vector2(minXOffset, minYOffset);
            return;
        }

        RectTransform canvasRT = canvas.GetComponent<RectTransform>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        // 将屏幕坐标转换为 Canvas 本地坐标（以 canvasRT 的中心为原点）
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, mousePosition, cam, out localPoint);

        // 基于 Canvas 大小计算偏移（按比例，并保证不小于最小像素偏移）
        float offsetX = Mathf.Max(minXOffset, canvasRT.rect.width * canvasOffsetRatioX);
        float offsetY = Mathf.Max(minYOffset, canvasRT.rect.height * canvasOffsetRatioY);

        // 根据鼠标在画布中心的相对位置决定偏移方向（鼠标在右侧则 tooltip 向左，鼠标在上方则 tooltip 向下）
        float dirX = localPoint.x > 0 ? -1f : 1f;
        float dirY = localPoint.y > 0 ? -1f : 1f;

        Vector2 desired = localPoint + new Vector2(dirX * offsetX, dirY * offsetY);

        // 计算可用最小/最大值以防 tooltip 超出画布边界
        Vector2 tooltipSize = tooltipRT.rect.size;
        Vector2 min = canvasRT.rect.min + (tooltipSize * tooltipRT.pivot);
        Vector2 max = canvasRT.rect.max - (tooltipSize * (Vector2.one - tooltipRT.pivot));

        // 夹紧到画布内
        desired = new Vector2(
            Mathf.Clamp(desired.x, min.x, max.x),
            Mathf.Clamp(desired.y, min.y, max.y)
        );

        tooltipRT.anchoredPosition = desired;

        //ui.skillToolTip.transform.position = new Vector2(mousePosition.x + XOffset, mousePosition.y + YOffset);
    }
}