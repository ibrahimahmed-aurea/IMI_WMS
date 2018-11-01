//===============================================================================
// Microsoft patterns & practices
// Smart Client Software Factory
//===============================================================================
// Copyright  Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// The WorkItemController is an abstract base class that contains a WorkItem. 
// This class contains logic that would otherwise exist in the WorkItem. 
// You can use this class to partition your code between a class that derives from WorkItemController and a WorkItem.
// 
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.scsf.2006jun/SCSF/html/03-210-Creating%20a%20Smart%20Client%20Solution.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using System.ServiceModel;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.Framework.UX
{
	/// <summary>
	/// Base class for a WorkItem controller.
	/// </summary>
	public abstract class WorkItemController : IWorkItemController
	{
        
        
		/// <summary>
		/// Gets or sets the work item.
		/// </summary>
		/// <value>The work item.</value>
        [ServiceDependency]
        public WorkItem WorkItem
        {
            get;
            set;
        }
                        		
		public virtual void Run()
		{
		}

        protected virtual void OnRunStarted()
        {
            
        }
                
    }
}