using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Dao;

namespace Cdc.MetaManager.BusinessLogic
{
    public class AnalyzeIssueList : List<Issue>
    {
        private IAnalyzeService analyzeService = null;

        public AnalyzeIssueNode ParentNode { get; private set; }

        public AnalyzeIssueList(AnalyzeIssueNode parentNode)
        {
            ParentNode = parentNode;

            analyzeService = MetaManagerServices.GetAnalyzeService();
        }

        public void Add(IssueSeverityType type, string text, object tag)
        {
            if (type != IssueSeverityType.None)
            {
                // Check if ParentNode has an Application, IssueObjectType and IssueObjectId defined or
                // it's not possible to add to the IssueList. Give an exception!
                if (ParentNode.Application == null ||
                    ParentNode.IssueObjectType == null ||
                    ParentNode.IssueObjectId == null)
                    throw new Exception("You may not create any Issues to a IssueNode that doesn't have Application, IssueObjectType and IssueObjectId set!");

                // Check if issue exist already
                Issue currentIssue = analyzeService.FindIssue(ParentNode.Application.Id, (IssueObjectType)ParentNode.IssueObjectType, ParentNode.IssueObjectId, GetTitle(), text);

                if (currentIssue != null)
                {
                    // Set the tag and the severity type
                    currentIssue.Tag = tag;
                    currentIssue.Severity = type;

                    // Save Issue
                    analyzeService.SaveOrUpdateIssue(currentIssue);
                }
                else
                {
                    // Create the new Issue
                    currentIssue = new Issue();
                    currentIssue.Application = ParentNode.Application;
                    currentIssue.ObjectType = (IssueObjectType)ParentNode.IssueObjectType;
                    currentIssue.ObjectId = ParentNode.IssueObjectId;
                    currentIssue.Severity = type;
                    currentIssue.Tag = tag;
                    currentIssue.Text = text;
                    currentIssue.Title = GetTitle();
                    currentIssue.Hidden = false;

                    // Save the Issue
                    analyzeService.SaveOrUpdateIssue(currentIssue);
                }

                // Add the found or created issue to the list.
                base.Add(currentIssue);
            }
        }

        private string GetTitle()
        {
            AnalyzeIssueNode current = ParentNode;
            string title = string.Empty;

            while (current != null && current.Application != null)
            {
                if (!string.IsNullOrEmpty(current.Name))
                {
                    if (string.IsNullOrEmpty(title))
                    {
                        title = current.Name;
                    }
                    else
                    {
                        title = string.Format("{0} / {1}", current.Name, title);
                    }
                }

                current = current.Parent;
            }

            return title;
        }

        public void AddWarning(string text)
        {
            AddWarning(text, null);
        }

        public void AddWarning(string text, object infoObject)
        {
            Add(IssueSeverityType.Warning, text, infoObject);
        }

        public void AddError(string text)
        {
            AddError(text, null);
        }

        public void AddError(string text, object infoObject)
        {
            Add(IssueSeverityType.Error, text, infoObject);
        }
    }

    public class AnalyzeIssueNodeList : List<AnalyzeIssueNode>
    {
        public new void Add(AnalyzeIssueNode node)
        {
            // Lets check if there are any issues listed on any node.
            // If not we don't add it to the list.
            if (node != null)
            {
                if (!CleanIssuesRemoveMe(node))
                {
                    base.Add(node);
                }
            }
        }

        private bool CleanIssuesRemoveMe(AnalyzeIssueNode node)
        {
            if (node.Children.Count > 0)
            {
                for (int i = node.Children.Count - 1; i >= 0; i--)
                {
                    if (CleanIssuesRemoveMe(node.Children[i]))
                    {
                        node.Children.RemoveAt(i);
                    }
                }
            }

            if (node.IssueList.Count == 0 && node.Children.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public class AnalyzeIssueTree
    {
        public AnalyzeIssueNodeList IssueNodes { get; private set; }

        public IList<Issue> GetAllIssues(IssueObjectType type)
        {
            List<Issue> allIssues = new List<Issue>();

            foreach (AnalyzeIssueNode node in IssueNodes)
            {
                allIssues.AddRange(node.GetAllIssuesBelowNode(type));
            }

            return allIssues;
        }

        public AnalyzeIssueTree()
        {
            IssueNodes = new AnalyzeIssueNodeList();
        }

    }

    public class AnalyzeIssueNode
    {
        public Application Application { get; private set; }

        public IssueObjectType? IssueObjectType { get; private set; }

        public Guid IssueObjectId { get; private set; }

        public AnalyzeIssueNode Parent { get; set; }

        public List<AnalyzeIssueNode> Children { get; set; }

        public string Name { get; set; }

        public object Tag { get; set; }

        public AnalyzeIssueList IssueList { get; set; }

        public AnalyzeIssueNode()
        {
            IssueList = new AnalyzeIssueList(this);
            Children = new List<AnalyzeIssueNode>();
            Application = null;
            IssueObjectType = null;
            IssueObjectId = Guid.Empty;
            Name = string.Empty;
        }

        public AnalyzeIssueNode(string name)
            : this()
        {
            Name = name;
        }

        public AnalyzeIssueNode(Application application, 
                                IssueObjectType? issueObjectType, 
                                Guid issueObjectId) : this()
        {
            SetIssueType(application, issueObjectType, issueObjectId);
        }

        public void SetIssueType(Application application, IssueObjectType? issueObjectType, Guid issueObjectId)
        {
            Application = application;
            IssueObjectType = issueObjectType;
            IssueObjectId = issueObjectId;
        }

        public AnalyzeIssueNode AddChild(string name)
        {
            return AddChild(name, null);
        }

        public AnalyzeIssueNode AddChild(string name, object tag)
        {
            AnalyzeIssueNode node = new AnalyzeIssueNode(Application, IssueObjectType, IssueObjectId);

            node.Name = name;
            node.Tag = tag;

            node.Parent = this;
            Children.Add(node);

            return node;
        }

        public IList<Issue> GetAllIssuesBelowNode(IssueObjectType type)
        {
            List<Issue> issues = new List<Issue>();

            if (Children.Count > 0)
            {
                foreach (AnalyzeIssueNode childNode in Children)
                {
                    issues.AddRange(childNode.GetAllIssuesBelowNode(type));
                }
            }

            if (IssueList.Count > 0 && 
                IssueObjectType.HasValue &&
                IssueObjectType.Value == type)
            {
                issues.AddRange(IssueList);
            }

            return issues;
        }
       
    }

}
