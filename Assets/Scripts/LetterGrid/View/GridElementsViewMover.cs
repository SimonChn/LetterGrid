using System;
using System.Collections;
using UnityEngine;

using URandom = UnityEngine.Random;

public class GridElementsViewMover : MonoBehaviour, IGridElementViewMover
{
    private IEnumerator animationCoroutine;

    [SerializeField] private AnimationCurve moveNormalizedCurve;

    [Range(0.01f,4f)]
    [SerializeField] private float moveTime = 2f;

    [Range(0.01f,2f)]
    [SerializeField] private float generationMoveTime = 1f;

    public Action OnMovingStarted;
    public Action OnMovingCompleted;

    public void ForceStop()
    {
        if(animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    public void MoveElementsToStartPositions(IGridElementView[] elements, GridProperties gridProperties, Action onEnd)
    {
        Vector2[] startPositions = new Vector2[elements.Length];
        Vector2[] targetPositions = new Vector2[elements.Length];

        float eWidth = gridProperties.ElementSize.x;
        float eHeight = gridProperties.ElementSize.y;
        float gridX = gridProperties.GridRectSize.x;
        float gridY = gridProperties.GridRectSize.y;

        for (int i = 0; i < elements.Length; i++)
        {
            targetPositions[i] = gridProperties.GetGridElementPosition(i);

            int roll = URandom.Range(0, 4);

            startPositions[i] = roll switch
            {
                1 => new Vector2(URandom.Range(-0.5f * eWidth, 0.5f * eWidth + gridX),
                        URandom.Range(0.5f * eHeight + gridY, 2 * eHeight + gridY)),

                2 => new Vector2(URandom.Range(0.5f * eWidth + gridX, 2f * eWidth + gridX),
                        URandom.Range(-0.5f * eHeight, 0.5f * eHeight + gridY)),

                3 => new Vector2(URandom.Range(-0.5f * eWidth, 0.5f * eWidth + gridX),
                        URandom.Range(-2f * eHeight, -0.5f * eHeight)),

                _ => new Vector2(URandom.Range(-2f * eWidth, -0.5f * eWidth),
                        URandom.Range(-0.5f * eHeight, 0.5f * eHeight + gridY))
            };

            elements[i].Position = startPositions[i];
        }

        MoveElementsToPositions(elements, startPositions, targetPositions, generationMoveTime, onEnd);
    }

    public void MoveElementsToPositions(IGridElementView[] elements, int[] targetGridIndexes, GridProperties gridProperties, Action onEnd)
    {
        if (elements.Length != targetGridIndexes.Length)
        {
            throw new ArgumentException();
        }

        Vector2[] startPositions = new Vector2[elements.Length];
        Vector2[] targetPositions = new Vector2[elements.Length];

        for (int i = 0; i < elements.Length; i++)
        {
            startPositions[i] = elements[i].Position;
            targetPositions[i] = gridProperties.GetGridElementPosition(targetGridIndexes[i]);
        }

        MoveElementsToPositions(elements, startPositions, targetPositions, moveTime, onEnd);
    }

    private void MoveElementsToPositions(IGridElementView[] elements, Vector2[] startPositions, Vector2[] targetPositions, float moveTime, Action onEnd)
    {
        animationCoroutine = MoveElementsToPositionsCoroutine(elements, startPositions, targetPositions, moveTime, onEnd);
        StartCoroutine(animationCoroutine);
    }

    private IEnumerator MoveElementsToPositionsCoroutine(IGridElementView[] elements, Vector2[] startPositions, Vector2[] targetPositions, 
        float moveTime, Action onEnd)
    {
        OnMovingStarted?.Invoke();

        bool doMovement = false;

        for(int i = 0; i < elements.Length; i++)
        {
            if(startPositions[i] != targetPositions[i])
            {
                doMovement = true;
            }
        }

        if (!doMovement)
        {
            OnMovingCompleted?.Invoke();
            yield break;
        }

        if(moveTime <= 0)
        {
            throw new ArithmeticException();
        }

        yield return null;

        float timer = 0f;

        while(timer < moveTime)
        {
            yield return null;
            timer += Time.deltaTime;

            float calculatedProgress = moveNormalizedCurve.Evaluate(timer / moveTime);

            for(int i = 0; i < elements.Length; i++)
            {
                elements[i].Position = startPositions[i] + (targetPositions[i] - startPositions[i]) * calculatedProgress;
            }
        }

        onEnd?.Invoke();
    }
}
