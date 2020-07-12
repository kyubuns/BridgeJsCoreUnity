﻿using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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

            inputField.text = @"function fib(n) {
  return n <= 1 ? n : fib(n - 1) + fib(n - 2);
}

fib(10);";

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

            var stopWatch = Stopwatch.StartNew();
            var (value, error) = engine.EvaluateScript(inputFieldText);
            if (!string.IsNullOrWhiteSpace(error)) Log($"Error! {error}");

            Debug.Log($"value.IsUndefined() {value.IsUndefined()}");
            Debug.Log($"value.IsNull() {value.IsNull()}");
            Debug.Log($"value.IsNumber() {value.IsNumber()}");
            Debug.Log($"value.IsString() {value.IsString()}");
            Debug.Log($"value.IsArray() {value.IsArray()}");
            Debug.Log($"value.IsObject() {value.IsObject()}");
            Debug.Log($"value.ToDouble() {value.ToDouble()}");
            Debug.Log($"value.ToString() {value.ToString()}");

            if (value.IsArray())
            {
                for (var i = 0; i < value.ForProperty("length").ToInt32(); ++i)
                {
                    Debug.Log($"{i} = {value.AtIndex(i)}");
                }
            }

            Log($"{stopWatch.ElapsedMilliseconds}ms | {value}");
        }

        private void Log(string message)
        {
            Debug.Log(message);
            resultText.text = $"{message}\n{resultText.text}";
        }
    }
}

