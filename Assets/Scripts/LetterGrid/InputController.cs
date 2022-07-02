using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class InputController : MonoBehaviour
{
    private const int startColumns = 3;
    private const int startRows = 5;
    private const int maxLength = 30;

    private int currentColumns;
    private int currentRows;

    [SerializeField] private Button generateButton;
    [SerializeField] private Button shuffleButton;
    [SerializeField] private Button shuffleRowButton;

    [Space(20)]
    [SerializeField] private TMP_InputField columnsInput;
    [SerializeField] private TMP_InputField rowsInput;

    [Header("DummyError")]
    [SerializeField] private GameObject dummyError;

    private GridGenerator gridGenerator;

    public void Init(GridGenerator gridGenerator, LetterGridController gridController)
    {
        this.gridGenerator = gridGenerator;

        currentColumns = startColumns;
        currentRows = startRows;

        columnsInput.text = startColumns.ToString();
        columnsInput.onEndEdit.AddListener(delegate { CheckInput(columnsInput, ref currentColumns);});

        rowsInput.text = startRows.ToString();
        rowsInput.onEndEdit.AddListener(delegate { CheckInput(rowsInput, ref currentRows); });

        generateButton.onClick.AddListener(UnlockShuffles);
        generateButton.onClick.AddListener(GeneretaNewGrid);

        shuffleButton.onClick.AddListener(gridController.ShuffleElements);

        gridController.OnBusyStarted += SetGridBusyState;
        gridController.OnBusyFinished += UnsetGridBusyState;
        shuffleRowButton.onClick.AddListener(gridController.ShuffleRandomRow);
    }

    private void UnlockShuffles()
    {
        shuffleButton.interactable = true;
        shuffleRowButton.interactable = true;

        generateButton.onClick.RemoveListener(UnlockShuffles);
    }

    private void SetGridBusyState()
    {
        shuffleRowButton.interactable = false;
    }

    private void UnsetGridBusyState()
    {
        shuffleRowButton.interactable = true;
    }

    private void CheckInput(TMP_InputField input, ref int targetValue)
    {
        try
        {
            int result = int.Parse(input.text);

            if(result <= 0)
            {
                DisplayInputError();
                input.text = targetValue.ToString();
                return;
            }

            if(result > maxLength)
            {
                result = maxLength;
                input.text = result.ToString();
            }

            targetValue = result;
        }
        catch
        {
            input.text = targetValue.ToString();
        }
    }

    private void DisplayInputError()
    {
        dummyError.SetActive(true);
    }

    private void GeneretaNewGrid()
    {
        gridGenerator.GenerateNewGrid(currentColumns, currentRows);
    }

    private void OnDisable()
    {
        generateButton.onClick.RemoveAllListeners();
        shuffleButton.onClick.RemoveAllListeners();
        shuffleRowButton.onClick.RemoveAllListeners();

        columnsInput.onEndEdit.RemoveAllListeners();
        columnsInput.onEndEdit.RemoveAllListeners();
    }
}
