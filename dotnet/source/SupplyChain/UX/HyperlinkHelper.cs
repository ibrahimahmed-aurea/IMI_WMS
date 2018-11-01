using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Deployment.Application;
using System.Collections.Specialized;
using System.Web;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX
{
    public class HyperlinkHelper
    {
        private HyperlinkHelper()
        { 
        }

        public static ShellMenuItem CreateFavoritesItem(string caption, string dialogId, string eventTopic, string assemblyFile, object target)
        {
            var parameters = GetParameters(target);

            ShellMenuItem item = new ShellMenuItem();
            item.IsEnabled = true;
            item.IsAuthorized = true;
            item.Operation = dialogId;
            item.Id = dialogId;
            item.Caption = caption;
            item.EventTopic = eventTopic;
            item.Parameters = BuildQueryString(parameters);
            item.AssemblyFile = assemblyFile;

            return item;
        }
               
        private static Dictionary<string, string> GetParameters(object target)
        {
            var parameters = new Dictionary<string, string>();

            if (target != null)
            {
                foreach (PropertyInfo pi in target.GetType().GetProperties())
                {
                    if (pi.CanRead && pi.CanWrite)
                    {
                        object val = pi.GetValue(target, null);

                        if (val != null)
                        {
                            parameters.Add(pi.Name, val.ToString());
                        }
                    }
                }
            }

            return parameters;
        }

        public static string BuildQueryString(IDictionary<string, string> parameters)
        {
            return string.Join("&", Array.ConvertAll(parameters.Keys.ToArray(), key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(parameters[key]))));
        }

        public static string BuildQueryString(object target)
        {
            if (target != null)
            {
                var parameters = GetParameters(target);

                return string.Join("&", Array.ConvertAll(parameters.Keys.ToArray(), key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(parameters[key]))));
            }
            else
            {
                return "";
            }
        }
                
        public static void MapQueryString(string queryString, object target)
        {
            NameValueCollection args = HttpUtility.ParseQueryString(queryString);

            for (int i = 0; i < args.Count; i++)
            {
                PropertyInfo info = target.GetType().GetProperty(args.GetKey(i));

                if (info != null)
                {
                    if (Nullable.GetUnderlyingType(info.PropertyType) != null)
                    {
                        info.SetValue(target, Convert.ChangeType(args[args.GetKey(i)], Nullable.GetUnderlyingType(info.PropertyType)), null);
                    }
                    else
                    {
                        info.SetValue(target, Convert.ChangeType(args[args.GetKey(i)], info.PropertyType), null);
                    }
                }
            }
        }

        public static ShellHyperlink CreateShellHyperlink(WorkItem workItem, string moduleId, string dialogId, string title, object target)
        {
            string link = "";

            var parameters = GetParameters(target);

            parameters.Add("ModuleId", moduleId);
            parameters.Add("DialogId", dialogId);
            parameters.Add("Title", title);

            IUserSessionService userSessionService = workItem.Services.Get<IUserSessionService>();

            if (userSessionService.ActivationUri != null)
            {
                NameValueCollection args = HttpUtility.ParseQueryString(userSessionService.ActivationUri.Query);

                for (int i = 0; i < args.Count; i++)
                {
                    if (i > 2)
                    {
                        break;
                    }

                    parameters.Add(args.GetKey(i), args[args.GetKey(i)]);
                }
                                               
                int queryPosition = userSessionService.ActivationUri.AbsoluteUri.IndexOf('?');

                link = string.Format("{0}?{1}", userSessionService.ActivationUri.OriginalString.Substring(0, queryPosition), BuildQueryString(parameters));
            }

            return new ShellHyperlink(link, moduleId, parameters);
        }
    }
}
