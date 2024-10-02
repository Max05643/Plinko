using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    GameObject ballObj;

    [SerializeField]
    float ballRadius = 0.5f;

    [SerializeField]
    float animationStepTime = 0.3f;

    [SerializeField]
    PyramidController pyramidController;


    Sequence currentAnimation = null;

    List<bool> GenerateBallWay(int height)
    {
        List<bool> way = new List<bool>();
        for (int i = 0; i < height; i++)
        {
            way.Add(Random.Range(0, 2) == 0);
        }
        return way;
    }

    Tween RotateAround(Transform transform, Vector2 center, float radius, float angle, float duration, Ease ease)
    {
        float currentAngle = 0;
        return DOTween.To(() => currentAngle, x => currentAngle = x, angle, duration).From(0).SetEase(ease).OnUpdate(() =>
        {
            transform.localPosition = center + (Vector2)(Quaternion.Euler(0, 0, currentAngle) * Vector2.up * radius);
        });
    }

    Sequence AddJumpSequence(Transform transform, Vector2 targetPos, float baseJumpHeight, float duration, int count)
    {
        var sequence = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            sequence.Append(transform.DOLocalJump(targetPos, baseJumpHeight, 1, duration / count).SetEase(Ease.Linear));
            baseJumpHeight /= 2;
        }

        return sequence.SetEase(Ease.Linear);
    }

    Sequence GenerateBallAnimation(List<bool> way, Vector2 startPos, PyramidController pyramidController)
    {
        Sequence sequence = DOTween.Sequence();

        var firstPos = pyramidController.PositionAboveBlock(2, 1) + Vector2.up * ballRadius;

        sequence.Append(ballObj.transform.DOLocalMove(firstPos, animationStepTime / 2).From(startPos).SetEase(Ease.Linear));
        sequence.Append(AddJumpSequence(ballObj.transform, firstPos, 0.2f, animationStepTime, 4));

        int lastColumn = 1;
        int currentRow = 2;

        for (int i = 0; i < way.Count; i++)
        {
            int currentColumn = lastColumn;

            if (way[i])
            {
                currentColumn++;
            }

            var finalPosition = pyramidController.PositionAboveBlock(currentRow + 1, currentColumn) + Vector2.up * ballRadius;


            sequence.Append(RotateAround(ballObj.transform, pyramidController.PositionForBlock(currentRow, lastColumn), pyramidController.BlockSize + ballRadius, 90f * (lastColumn == currentColumn ? 1 : -1), animationStepTime, Ease.InCirc));
            sequence.Append(ballObj.transform.DOLocalMove(finalPosition, animationStepTime / 4).SetEase(Ease.Linear));

            if (i < way.Count - 1)
                sequence.Append(AddJumpSequence(ballObj.transform, finalPosition, 0.2f, animationStepTime, 4));

            lastColumn = currentColumn;
            currentRow++;
        }

        return sequence.SetEase(Ease.Linear);
    }

    [ContextMenu("Do Random Ball Animation")]
    public void DoRandomBallAnimation()
    {
        if (currentAnimation != null && currentAnimation.IsActive() && currentAnimation.IsPlaying())
        {
            return;
        }

        var ballWay = GenerateBallWay(pyramidController.Height - 1);

        ballObj.SetActive(true);

        currentAnimation = GenerateBallAnimation(ballWay, Vector2.zero, pyramidController);

        currentAnimation.onComplete += () =>
        {
            ballObj.SetActive(false);
        };

    }

}
