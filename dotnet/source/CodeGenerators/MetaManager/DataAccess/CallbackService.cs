using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    // Callback delegate
    public delegate void CallbackDelegate(string passText,
                                          int maxSteps,
                                          int currentStep,
                                          string currentStepText);

    public class CallbackService
    {
        private string PassText { get; set; }
        private int MaxSteps { get; set; }
        private int CurrentStep { get; set; }
        private string StepText { get; set; }
        private string NextStepPrefixText { get; set; }

        public const string C_MAXSTEP = "<MAX>";
        public const string C_CURRENTSTEP = "<CURR>";

        private CallbackDelegate Callback { get; set; }

        public CallbackService()
        {
            Callback = null;
        }

        public void SetCallback(CallbackDelegate callback) { Callback = callback; }

        public void Initialize(int maxSteps)
        {
            Initialize(string.Empty, maxSteps);
        }

        public void Initialize(string passText, int maxSteps)
        {
            PassText = passText;
            MaxSteps = maxSteps;
            CurrentStep = 0;
            StepText = string.Empty;
            NextStepPrefixText = string.Empty;
        }

        private void DoCallback()
        {
            if (Callback != null)
            {
                Callback(PassText, MaxSteps, CurrentStep, ProcessCurrentStepText(StepText));
            }
        }

        private string ProcessCurrentStepText(string currentStepText)
        {
            string processed = currentStepText;

            processed = processed.Replace(C_MAXSTEP, MaxSteps.ToString());
            processed = processed.Replace(C_CURRENTSTEP, CurrentStep.ToString());

            return processed;
        }

        public void Do(string passText,
                       string stepText)
        {
            Do(passText, 0, 0, stepText);
        }

        public void Do(string passText,
                       int maxSteps,
                       int currentStep,
                       string currentStepText)
        {
            PassText = passText;
            MaxSteps = maxSteps;
            CurrentStep = currentStep;
            StepText = currentStepText;

            DoCallback();
        }

        public void Next()
        {
            CurrentStep += 1;

            DoCallback();
        }

        public void Next(string currentStepText)
        {
            Next(currentStepText, false);
        }

        public void Next(string currentStepText, bool setAsPrefixText)
        {
            CurrentStep += 1;

            if (setAsPrefixText)
                NextStepPrefixText = currentStepText;

            // Check if prefix should be added
            if (!string.IsNullOrEmpty(NextStepPrefixText))
            {
                currentStepText = string.Format("{0}{1}", NextStepPrefixText, currentStepText);
            }

            StepText = currentStepText;

            DoCallback();
        }

        public void Update(string currentStepText)
        {
            // Check if prefix should be added
            if (!string.IsNullOrEmpty(NextStepPrefixText))
            {
                currentStepText = string.Format("{0}{1}", NextStepPrefixText, currentStepText);
            }

            StepText = currentStepText;

            DoCallback();
        }

    }
}
