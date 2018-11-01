using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace Imi.SupplyChain.UX
{
    public class DrillDownHelper
    {
        public static string GetActionId(string fieldName, IList<DrillDownArgs> listOfDrillDowns)
        {
            foreach (DrillDownArgs drillDownArgs in listOfDrillDowns)
            {
                if (fieldName == drillDownArgs.FieldName)
                    return drillDownArgs.ActionId;
            }

            return null;
        }

        public static string GetFileContentFieldName(string fieldName, IList<DrillDownArgs> openFileDrillDownList)
        {
            foreach (DrillDownArgs drillDownArgs in openFileDrillDownList)
            {
                if (fieldName == drillDownArgs.FieldName)
                    return drillDownArgs.FileContentFieldName;
            }

            return null;
        }

        public static void SetTextBoxStyle(UserControl view, IList<DrillDownArgs> listOfDrillDowns, Style style)
        {
            List<Visual> textBoxList = FindAllOfType(view.Content as Visual, typeof(Imi.Framework.Wpf.Controls.TextBox));
            List<string> names = GetFieldNames(textBoxList, Imi.Framework.Wpf.Controls.TextBox.TextProperty);

            foreach (DrillDownArgs drillDownArgs in listOfDrillDowns)
            {
                if (names.Contains(drillDownArgs.FieldName))
                {
                    int index = names.IndexOf(drillDownArgs.FieldName);
                    Imi.Framework.Wpf.Controls.TextBox t = (Imi.Framework.Wpf.Controls.TextBox)textBoxList[index];
                    t.Style = style;

                    if (t.ToolTip == null || t.ToolTip.GetType() == typeof(string))
                    {
                        TextBlock txt = new TextBlock();

                        if (t.ToolTip != null)
                        {
                            if (!string.IsNullOrWhiteSpace(((string)t.ToolTip)))
                            {
                                string endline = string.Empty;

                                if (!string.IsNullOrWhiteSpace(drillDownArgs.Caption))
                                {
                                    endline = "\r\n\r\n";
                                }

                                txt.Inlines.Add((string)t.ToolTip + endline);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(drillDownArgs.Caption))
                        {
                            txt.Inlines.Add(new System.Windows.Documents.Bold(new System.Windows.Documents.Run(StringResources.HyperLink_Hint_Header)));
                            txt.Inlines.Add("\r\n" + drillDownArgs.Caption);
                        }

                        t.ToolTip = txt;
                    }

                    t.Tag = drillDownArgs.FieldName;
                }
            }
        }

        private static List<string> GetFieldNames(List<Visual> boxList, DependencyProperty depProperty)
        {
            List<string> namesList = new List<string>();

            foreach (Visual box in boxList)
            {
                string name = string.Empty;

                if (box is DependencyObject)
                {
                    DependencyObject dobj = box as DependencyObject;
                    Binding binding = BindingOperations.GetBinding(dobj, depProperty);
                    name = binding.Path.Path;
                }

                namesList.Add(name);
            }

            return namesList;
        }

        private static List<Visual> FindAllOfType(Visual visual, Type findType)
        {
            if (visual == null)
                return null;

            List<Visual> typeList = new List<Visual>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                // Retrieve child visual at specified index value.
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(visual, i);
                // Do processing of the child visual object.

                if (childVisual.GetType() == findType)
                    typeList.Add(childVisual);

                List<Visual> childList = null;

                // Enumerate children of the child visual object.
                if (childVisual is ContentControl)
                    childList = FindAllOfType((childVisual as ContentControl).Content as Visual, findType);
                else
                    childList = FindAllOfType(childVisual, findType);

                if ((childList != null) && (childList.Count > 0))
                {
                    typeList.AddRange(childList);
                }
            }

            return typeList;
        }
    }

}
