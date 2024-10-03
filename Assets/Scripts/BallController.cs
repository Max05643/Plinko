using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class BallController : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        [Range(0.1f, 3f)]
        public float animationStepTime = 0.3f;
    }

    [SerializeField]
    GameObject ballObj;

    [SerializeField]
    float ballRadius = 0.5f;

    [Inject]
    Settings settings;

    [Inject]
    PyramidController pyramidController;


    Sequence currentAnimation = null;

    /// <summary>
    /// Generates random way for the ball. The way is repsented as a list of bools where true means right turn and false means left turn
    /// </summary>
    List<bool> GenerateBallWay(int height)
    {
        List<bool> way = new List<bool>();
        for (int i = 0; i < height; i++)
        {
            way.Add(Random.Range(0, 2) == 0);
        }
        return way;
    }


    /// <summary>
    /// Rotates the object around the center with the given radius and angle
    /// </summary>
    Tween RotateAround(Transform transform, Vector2 center, float radius, float angle, float duration, Ease ease)
    {
        float currentAngle = 0;
        return DOTween.To(() => currentAngle, x => currentAngle = x, angle, duration).From(0).SetEase(ease).OnUpdate(() =>
        {
            transform.localPosition = center + (Vector2)(Quaternion.Euler(0, 0, currentAngle) * Vector2.up * radius);
        });
    }

    /// <summary>
    /// Adds jump animation
    /// </summary>
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

    /// <summary>
    /// Converts ball's way (which represents left or right turn on each pyramid level) into the animation
    /// </summary>
    Sequence GenerateBallAnimation(List<bool> way, Vector2 startPos, PyramidController pyramidController)
    {
        Sequence sequence = DOTween.Sequence();

        var firstPos = pyramidController.PositionAboveBlock(2, 1) + Vector2.up * ballRadius;

        sequence.Append(ballObj.transform.DOLocalMove(firstPos, settings.animationStepTime / 2).From(startPos).SetEase(Ease.Linear).OnComplete(() =>
        {
            pyramidController.RegisterBlockReached(2, 1);
        }));

        sequence.Append(AddJumpSequence(ballObj.transform, firstPos, 0.2f, settings.animationStepTime, 4));

        int lastColumn = 1;
        int currentRow = 2;

        for (int i = 0; i < way.Count; i++)
        {
            int currentColumn = lastColumn;

            if (way[i])
            {
                currentColumn++;
            }

            int lastRow = currentRow + 1;

            var finalPosition = pyramidController.PositionAboveBlock(lastRow, currentColumn) + Vector2.up * ballRadius;



            sequence.Append(RotateAround(ballObj.transform, pyramidController.PositionForBlock(currentRow, lastColumn), pyramidController.BlockSize + ballRadius, 90f * (lastColumn == currentColumn ? 1 : -1), settings.animationStepTime, Ease.InCirc));
            sequence.Append(ballObj.transform.DOLocalMove(finalPosition, settings.animationStepTime / 4).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (lastRow <= pyramidController.Height)
                    pyramidController.RegisterBlockReached(lastRow, currentColumn);
            }));

            if (i < way.Count - 1)
                sequence.Append(AddJumpSequence(ballObj.transform, finalPosition, 0.2f, settings.animationStepTime, 4));

            lastColumn = currentColumn;
            currentRow++;
        }

        sequence.onComplete += () =>
        {
            pyramidController.RegisterFinishBlockReached(lastColumn);
        };

        return sequence.SetEase(Ease.Linear);
    }


    /// <summary>
    /// Generates random way for the ball and plays the animation.
    /// Will do nothing if the previous animation is still playing
    /// </summary>
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
