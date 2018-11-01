using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Workflow.ComponentModel.Compiler;

namespace Imi.SupplyChain.UX.Rules
{
    public class RuleSetManager
    {
        private RuleSet ruleSet;
        private RuleExecution ruleExecution;

        public RuleSet RuleSet
        {
            get
            {
                return ruleSet;
            }
        }

        public object Execute()
        {
            if (ruleExecution == null)
                throw new InvalidOperationException("The rule must have a valid context object.");

            ruleSet.Execute(ruleExecution);
            
            return context;
        }

        public void Load(Stream stream)
        {
            XmlReader reader = XmlReader.Create(stream);

            try
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                ruleSet = serializer.Deserialize(reader) as RuleSet;
                ruleExecution = null;
                context = null;
            }
            finally
            {
                reader.Close();
            }
        }

        private object context;

        public object Context
        {
            get
            {
                return context;
            }
            set
            {
                if (ruleSet == null)
                    throw new InvalidOperationException("No RuleSet has been loaded.");

                ruleExecution = Validate(value);
                context = value;
            }
        }

        private RuleExecution Validate(object context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            RuleValidation ruleValidation = new RuleValidation(context.GetType(), null);

            if (!ruleSet.Validate(ruleValidation))
            {
                string errors = null;

                foreach (ValidationError error in ruleValidation.Errors)
                    errors += error.ErrorText + "\n";

                throw new RuleSetValidationException(errors);
            }
            else
            {
                return new RuleExecution(ruleValidation, context);
            }
        }
    }
}
