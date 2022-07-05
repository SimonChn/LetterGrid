using UnityEngine;

public class LetterGridLauncher : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GridElementsViewMover gridElementsViewMover;

    [SerializeField] private LetterGridController gridController;

    [Space(20)]
    [SerializeField] private InputController inputController;

    public void Launch()
    {
        gridGenerator.Init();
        gridController.Init(gridGenerator, gridElementsViewMover);

        inputController.Init(gridController);

        Destroy(this);
    }

    private void Start()
    {
        Launch();
    }
}