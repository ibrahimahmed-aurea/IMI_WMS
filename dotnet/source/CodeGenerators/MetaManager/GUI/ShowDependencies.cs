using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;

using Spring.Data.NHibernate.Support;
using QuickGraph;
using NHibernate;
using System.Threading;

namespace Cdc.MetaManager.GUI
{
    public partial class ShowDependencies : MdiChildForm
    {
        private IModelService modelService;
        private BidirectionalGraph<IDomainObject, IEdge<IDomainObject>> graph;
        private EventWaitHandle waitEvent;
        private bool abort;
        
        public ShowDependencies()
        {
            InitializeComponent();

            modelService = MetaManagerServices.GetModelService();
            graph = new BidirectionalGraph<IDomainObject, IEdge<IDomainObject>>();
            waitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
            
            ((DependencyGraph)elementHost.Child).VertexClicked += (s, ev) =>
            {
                pgDependencyObject.SelectedObject = ev.Vertex;
                PropertyGridcontextMenuStrip.Tag = ev.Vertex;

                if (ev.Button == System.Windows.Input.MouseButton.Right)
                {
                    PropertyGridcontextMenuStrip.Show(elementHost, (int)ev.MouseDevice.GetPosition(null).X, (int)ev.MouseDevice.GetPosition(null).Y);
                }
            };
        }

        private void ShowServiceMethodDependencies_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ThreadPool.QueueUserWorkItem(BuildGraph);
        }

        private void BuildGraph(object state)
        {
            try
            {
                using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                {
                    IDomainObject currentObject = modelService.GetDomainObject(ContaindDomainObjectIdAndType.Key, ContaindDomainObjectIdAndType.Value);
                    FindDependencies(currentObject, null);
                }

                if (!abort)
                {
                    Invoke(new System.Action(() =>
                    {
                        RenderGraph();
                    }));
                }

            }
            finally
            {
                waitEvent.Set();

                if (!abort)
                {
                    Invoke(new System.Action(() =>
                    {
                        this.Cursor = Cursors.Default;
                    }));
                }
            }
        }

        private void RenderGraph()
        {
            pgDependencyObject.SelectedObject = graph.Vertices.ToList()[0];

            if (graph.Vertices.Count() > 1)
            {
                ((DependencyGraph)elementHost.Child).Graph = graph;
                ((DependencyGraph)elementHost.Child).HighlightVertex(graph.Vertices.ToList()[0]);


                lbDependencies.Visible = false;
            }
            else
            {
                lbDependencies.Text = "No dependencies found";
            }

            statusLabel.Text = "";
        }

        private void FindDependencies(IDomainObject currentObject, IDomainObject child, bool halt = false)
        {
            if (abort || currentObject == null)
            {
                return;
            }

            if (graph.ContainsVertex(currentObject))
            {
                if (child != null)
                {
                    graph.AddEdge(new Edge<IDomainObject>(currentObject, child));
                }

                return;
            }

            statusLabel.Invoke(new System.Action(() => 
            {
                statusLabel.Text = string.Format("Analyzing {0}...", currentObject.ToString());
            }));
                        
            NHibernateUtil.Initialize(currentObject);

            graph.AddVertex(currentObject);

            if (child != null)
            {
                graph.AddEdge(new Edge<IDomainObject>(currentObject, child));
            }
                                                
            if (currentObject is Cdc.MetaManager.DataAccess.Domain.Application || halt)
            {
                return;
            }

            List<IDomainObject> list = modelService.GetReferencingObjects(currentObject) as List<IDomainObject>;

            if (currentObject is ServiceMethod || (currentObject is Dialog && ((Dialog)currentObject).Type == DialogType.Find))
            {
                statusLabel.Invoke(new System.Action(() =>
                {
                    statusLabel.Text = string.Format("Finding XML references for {0}...", currentObject.ToString());
                }));

                string query = "select v from View v where v.VisualTreeXml like '%" + currentObject.Id + "%'";
                IList<IDomainObject> xmlReferences = modelService.GetDomainObjectsByQuery<IDomainObject>(query);

                list.AddRange(xmlReferences);
            }

            IList<IVersionControlled> vcParents = new List<IVersionControlled>();

            foreach (IDomainObject domainObject in list)
            {
                halt = false;

                ViewAction vc = domainObject as ViewAction;

                if (vc != null && vc.Type == ViewActionType.JumpTo)
                {
                    halt = true;
                }

                IVersionControlled vcObject = domainObject as IVersionControlled;
                
                if (vcObject == null)
                {
                    List<IDomainObject> allParents;
                    vcObject = modelService.GetVersionControlledParent(domainObject, out allParents).LastOrDefault();
                }

                if (!vcParents.Contains(vcObject))
                {
                    vcParents.Add(vcObject);
                    FindDependencies(vcObject, currentObject, halt);
                }
            }
        }
        
        private void tvDependencies_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pgDependencyObject.SelectedObject = e.Node.Tag;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            abort = true;
            waitEvent.WaitOne();

            Close();
        }
    }
}
