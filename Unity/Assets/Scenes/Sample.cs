using UnityEngine;
using UnityEngine.UI;

namespace BridgeJsCore.Sample
{
    public class Sample : MonoBehaviour
    {
	    [SerializeField] private InputField inputField = default;
	    [SerializeField] private Button runButton = default;
	    [SerializeField] private Text resultText = default;

	    private Engine engine;

        public void Start()
        {
            engine = new Engine();

            inputField.text = @"function square(number) {
  return number * number;
}

square(5);";

            resultText.text = "";

            runButton.onClick.AddListener(() => Run());

            Log("Initialize Finished.");
        }

        private void Run()
        {
            var inputFieldText = inputField.text;
            if (inputFieldText == "dispose")
            {
                Log("Dispose");
                engine.Dispose();
                engine = new Engine();
                return;
            }

            var (value, error) = engine.EvaluateScript(inputFieldText);
            if (!string.IsNullOrWhiteSpace(error)) Log($"Error! {error}");
            Log(value.ToString());
        }

        private void Log(string message)
        {
            Debug.Log(message);
            resultText.text = $"{message}\n{resultText.text}";
        }
    }
}

