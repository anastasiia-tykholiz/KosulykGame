using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour, ICauldronObserver
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI expressionText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Transform ingredientPanel;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private TextMeshProUGUI movesLeftText;
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite failSprite;
    [SerializeField] private GameObject brewButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject craftingPanelRoot;
    [SerializeField] private TextMeshProUGUI commentText;

    [Header("Logic")]
    private CraftController controller;
    [SerializeField] private Granny granny;
    [SerializeField] private string currentLevel;

    private BurnPlantsQuestStep _questStep;
    private InputEvents _inputEvents => GameEventsManager.inputEvents;

    private List<Ingredient> availableIngredients;
    private List<Button> buttons = new();
    private int selectedIndex = 0;
    private int _targetResult;
    private int maxMoves;
    private int usedMoves = 0;
    private bool craftingResolved = false;
    private bool justOpened = false;
    private string _location;

    private readonly string[] addComments = { "Додаю", "Підкидаю ще", "Ще трохи...", "Спробую додати" };
    private readonly string[] subComments = { "Віднімаю", "Забираю", "Приберу трошки", "Хмм… відніму" };
    private readonly string[] mulComments = { "Множу на", "Підсилюю на", "Потужність ×", "Посилимо на" };
    private readonly string[] divComments = { "Ділю на", "Зменшую у", "Розділю на", "Скорочую на" };
    private readonly string[] firstMultiplyComments = { "Хмм… починати з множення дивна ідея.", "Множити ні на що? Не вийде.", "Немає ще з чим множити." };
    private readonly string[] firstDivideComments = { "Я точно хочу ділити з самого початку?..", "Ділити ніщо на щось? Нелогічно.", "Починати з ділення не найкращий план." };

    private void OnEnable()
    {
        _inputEvents.onSubmitPressed += OnSubmitPressed;
        _inputEvents.onMovePressed += OnMovePressed;
    }

    private void OnDisable()
    {
        _inputEvents.onSubmitPressed -= OnSubmitPressed;
        _inputEvents.onMovePressed -= OnMovePressed;
    }

    private void OnMovePressed(Vector2 direction)
    {
        if (craftingResolved) return;
        if (direction.x < 0) MoveSelection(-1);
        else if (direction.x > 0) MoveSelection(1);
    }

    private void OnSubmitPressed(InputEventContext ctx)
    {
        
        if (craftingResolved)
        {
            if (brewButton.activeSelf) OnClickBrew();
            else if (restartButton.activeSelf) OnClickRestart();
        }
        else
        {
            if (!gameObject.activeInHierarchy || availableIngredients == null || availableIngredients.Count == 0)
                return;
            //if (justOpened)
            //{
            //    justOpened = false;
            //    return;
            //}
            GameEventsManager.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);
            UseSelectedIngredient();
        }
    }




    private void PopulateIngredientUI()
    {
        foreach (var ing in availableIngredients)
        {
            GameObject obj = Instantiate(ingredientButtonPrefab, ingredientPanel);
            Image image = obj.GetComponent<Image>();
            image.sprite = ing.icon;

            TextMeshProUGUI label = obj.GetComponentInChildren<TextMeshProUGUI>();
            label.text = $"{ing.operation}{ing.value}";

            buttons.Add(obj.GetComponent<Button>());
        }
    }

    private void MoveSelection(int direction)
    {
        if (buttons.Count == 0) return;

        for (int i = 0; i < buttons.Count; i++)
            buttons[i].transform.localScale = Vector3.one;

        selectedIndex = (selectedIndex + direction + buttons.Count) % buttons.Count;
        HighlightSelected();
    }

    private void HighlightSelected()
    {
        if (buttons.Count == 0 || selectedIndex >= buttons.Count) return;
        buttons[selectedIndex].transform.localScale = Vector3.one * 1.2f;
    }

    private void UseSelectedIngredient()
    {
        if (craftingResolved) return;

        Ingredient chosen = availableIngredients[selectedIndex];

        if (controller.context.cauldron.ingredientsUsed.Count == 0 &&
            (chosen.operation == "*" || chosen.operation == "/"))
        {
            commentText.text = chosen.operation switch
            {
                "*" => GetRandomComment(firstMultiplyComments),
                "/" => GetRandomComment(firstDivideComments),
                _ => ""
            };
            return;
        }

        controller.AddIngredient(chosen);
        usedMoves++;
        UpdateMovesLeftText();

        commentText.text = chosen.operation switch
        {
            "+" => GetRandomComment(addComments) + $" {chosen.value}",
            "-" => GetRandomComment(subComments) + $" {chosen.value}",
            "*" => GetRandomComment(mulComments) + $" {chosen.value}",
            "/" => GetRandomComment(divComments) + $" {chosen.value}",
            _ => ""
        };

        if (controller.CheckResult())
        {
            resultImage.sprite = happySprite;
            resultImage.gameObject.SetActive(true);
            brewButton.SetActive(true);
            craftingResolved = true;
        }
        else if (usedMoves >= maxMoves)
        {
            resultImage.sprite = failSprite;
            resultImage.gameObject.SetActive(true);
            restartButton.SetActive(true);
            craftingResolved = true;
        }
    }

    private string GetRandomComment(string[] comments)
    {
        if (comments == null || comments.Length == 0) return "";
        int index = Random.Range(0, comments.Length);
        return comments[index];
    }

    public void OnCauldronUpdated(string expression, int result)
    {
        string formatted = BuildExpression(controller.context.cauldron.ingredientsUsed);
        expressionText.text = formatted;
        resultText.text = "= " + _targetResult;
    }

    public void InitForLevel(string location, BurnPlantsQuestStep questStep, Granny granny)
    {
        _location = location;
        _questStep = questStep;
        this.granny = granny;
        usedMoves = 0;
        craftingResolved = false;

        brewButton.SetActive(false);
        restartButton.SetActive(false);
        resultImage.gameObject.SetActive(false);

        if (craftingPanelRoot != null)
            craftingPanelRoot.SetActive(true);
        justOpened = true;

        availableIngredients = IngredientLibrary.Instance.GetIngredientsForLevel(location);
        Cauldron cauldron = new Cauldron();
        controller = new CraftController(cauldron);
        cauldron.AddObserver(this);

        Recipe recipe = GenerateRecipeForLocation(location);
        _targetResult = recipe.targetResult;
        maxMoves = recipe.maxActions;
        controller.StartRecipe(recipe);
        UpdateMovesLeftText();

        for (int i = ingredientPanel.childCount - 1; i >= 0; i--)
            DestroyImmediate(ingredientPanel.GetChild(i).gameObject);
        buttons.Clear();

        PopulateIngredientUI();

        HighlightSelected();
    }

    private void UpdateMovesLeftText()
    {
        int left = maxMoves - usedMoves;
        movesLeftText.text = $"Ходів лишилось: {left}";
    }

    private Recipe GenerateRecipeForLocation(string location)
    {
        return location switch
        {
            "forest" => new Recipe(17, 3),
            "pineForest" => new Recipe(22, 4),
            "swamp" => new Recipe(6, 4),
            _ => new Recipe(0, 0)
        };
    }

    private string BuildExpression(List<Ingredient> ingredients)
    {
        if (ingredients.Count == 0) return "";

        string expression = "";
        string prevOp = "";

        for (int i = 0; i < ingredients.Count; i++)
        {
            var ing = ingredients[i];
            string part;

            if (i == 0)
            {
                part = ing.operation switch
                {
                    "+" => ing.value.ToString(),
                    "-" => "-" + ing.value.ToString(),
                    _ => ing.value.ToString()
                };
            }
            else
            {
                bool prevWasLow = prevOp == "+" || prevOp == "-";
                bool currentIsHigh = ing.operation == "*" || ing.operation == "/";

                if (prevWasLow && currentIsHigh && expression.Contains("+") || expression.Contains("-"))
                {
                    expression = $"({expression})";
                }

                bool needsParens = (ing.operation == "*" || ing.operation == "/") && ing.value < 0;

                part = needsParens
                    ? $"{ing.operation}({ing.value})"
                    : $"{ing.operation}{ing.value}";
            }

            expression += part;
            prevOp = ing.operation;
        }

        return expression;
    }



    public void OnClickBrew()
    {
        if (!craftingResolved) return;

        Potion potion = PotionFactory.CreatePotion(_location);
        potion?.ApplyEffect(granny);

        _questStep.CompleteCraftingStep();
        if (craftingPanelRoot != null)
            craftingPanelRoot.SetActive(false);
    }

    public void OnClickRestart()
    {
        controller.StartRecipe(GenerateRecipeForLocation(_location));
        usedMoves = 0;
        craftingResolved = false;

        resultImage.gameObject.SetActive(false);
        brewButton.SetActive(false);
        restartButton.SetActive(false);

        commentText.text = "Спробуємо ще раз...";
        UpdateMovesLeftText();
    }
}
