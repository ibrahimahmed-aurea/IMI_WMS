using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Imi.Framework.Wpf.Controls;
using ActiproSoftware.Windows.Controls.Ribbon.UI;
using System.Reflection;
using ActiproSoftware.Windows.Controls.Ribbon;
using Imi.Framework.UX;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for ApplicationBar.xaml
    /// </summary>
    /// 
    public partial class ApplicationBar : UserControl
    {

        public event MenuItemSelectedEventHandler ActionMenuItemSelected;
        public event EventHandler<StartMenuItemExecutedEventArgs> StartMenuItemExecuted;

        public static DummySource DummySource = new DummySource();

        //Flag used to prevent user from double clicking on actions
        bool _idle = true;

        private static System.Collections.ObjectModel.ObservableCollection<DrillDownMenuItem> _favoriteFolders = new System.Collections.ObjectModel.ObservableCollection<DrillDownMenuItem>();
        public static System.Collections.ObjectModel.ObservableCollection<DrillDownMenuItem> FavoriteFolders
        {
            get
            {
                return _favoriteFolders;
            }
        }

        private DrillDownMenuItem _rememberItem = null;

        public ApplicationBar()
        {
            InitializeComponent();

            startMenu.HighlightSelected = true;

            this.startMenu.MenuItemSelected += (s, e) =>
            {
                e.Handled = true;

                if (StartMenuItemExecuted != null)
                {
                    StartMenuItemExecuted(this, new StartMenuItemExecutedEventArgs(e.SelectedItem as ShellDrillDownMenuItem, null, StartOption.Default));
                }
            };

            this.actionsMenu.MenuItemSelected += (s, e) =>
            {
                var stringBuilder = new StringBuilder();
                var xmlSettings = new System.Xml.XmlWriterSettings
                {
                    Indent = true
                };

                using (var xmlWriter = System.Xml.XmlWriter.Create(stringBuilder, xmlSettings))
                {
                    System.Windows.Markup.XamlWriter.Save(actionsMenu.Style, xmlWriter);
                }


                e.Handled = true;

                if (_idle)
                {
                    _idle = false;

                    if (ActionMenuItemSelected != null)
                    {
                        ActionMenuItemSelected(this, e);
                    }

                }
            };

            this.favoritesMenu.MenuItemSelected += (s, e) =>
            {
                e.Handled = true;

                if (StartMenuItemExecuted != null)
                {
                    StartMenuItemExecuted(this, new StartMenuItemExecutedEventArgs(e.SelectedItem as ShellDrillDownMenuItem, null, StartOption.NewWindow));
                }
            };

            ObjectDataProvider p = this.Resources["myDataSource"] as ObjectDataProvider;

            if (p != null)
            {
                MainWindow w = Application.Current.MainWindow as MainWindow;
                p.ObjectInstance = w.ribbon;
            }

            Application.Current.Dispatcher.Hooks.DispatcherInactive += new EventHandler(Hooks_DispatcherInactive);
        }

        void Hooks_DispatcherInactive(object sender, EventArgs e)
        {
            _idle = true;
        }

        private void Menus_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double? favoritesHeight = favoritesExpander.ActualHeight;
            double? actionsHeight = actionsExpander.ActualHeight;

            if (sender == favoritesMenu)
            {
                actionsHeight = null;
            }

            if (sender == actionsMenu)
            {
                favoritesHeight = null;
            }

            SetExpandersMaxSize(favoritesHeight, actionsHeight);
        }

        private void favoritesExpander_CollapsedExpanded(object sender, RoutedEventArgs e)
        {
            if (favoritesExpander != null && favoritesMenu != null)
            {
                double favoritesExpanderNewHeight = 0;

                if (favoritesExpander.IsExpanded)
                {
                    favoritesExpanderNewHeight = favoritesExpander.ActualHeight + favoritesMenu.ActualHeight;
                }
                else
                {
                    favoritesExpanderNewHeight = favoritesExpander.ActualHeight - favoritesMenu.ActualHeight;
                }

                SetExpandersMaxSize(favoritesExpanderNewHeight, actionsExpander.ActualHeight);
            }
        }

        private void actionsExpander_CollapsedExpanded(object sender, RoutedEventArgs e)
        {
            if (actionsExpander != null && actionsMenu != null)
            {
                double actionsExpanderNewHeight = 0;

                if (actionsExpander.IsExpanded)
                {
                    actionsExpanderNewHeight = actionsExpander.ActualHeight + actionsMenu.ActualHeight;
                }
                else
                {
                    actionsExpanderNewHeight = actionsExpander.ActualHeight - actionsMenu.ActualHeight;
                }

                SetExpandersMaxSize(favoritesExpander.ActualHeight, actionsExpanderNewHeight);
            }
        }

        private void SetExpandersMaxSize(double? favoritesExpanderHeight, double? actionsExpanderHeight)
        {
            if (favoritesExpanderHeight != null)
            {
                if (favoritesExpanderHeight < (theGrid.ActualHeight / 3))
                {
                    actionsExpander.MaxHeight = (theGrid.ActualHeight / 3) + ((theGrid.ActualHeight / 3) - favoritesExpanderHeight.Value);
                }
                else
                {
                    actionsExpander.MaxHeight = (theGrid.ActualHeight / 3);
                }
            }

            if (actionsExpanderHeight != null)
            {
                if (actionsExpanderHeight < (theGrid.ActualHeight / 3))
                {
                    favoritesExpander.MaxHeight = (theGrid.ActualHeight / 3) + ((theGrid.ActualHeight / 3) - actionsExpanderHeight.Value);
                }
                else
                {
                    favoritesExpander.MaxHeight = (theGrid.ActualHeight / 3);
                }
            }
        }

        private DrillDownMenuItem GetMenuItem(object item)
        {
            if ((item is ListBoxItem) &&
                ((item as ListBoxItem).Content != null) &&
                ((item as ListBoxItem).Content is DrillDownMenuItem))
            {
                return (item as ListBoxItem).Content as DrillDownMenuItem;
            }
            else
            {
                return null;
            }
        }

        private void ContextCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DrillDownMenuItem menuItem = GetMenuItem(e.OriginalSource);

            if (menuItem != null)
            {
                if (e.Command == ApplicationBarCommands.AddFolder)
                {
                    e.CanExecute = true;
                }
                else if (e.Command == ApplicationBarCommands.Paste)
                {
                    e.CanExecute = (_rememberItem != null);
                }
                else if ((e.Command == ApplicationBarCommands.Rename) ||
                         (e.Command == ApplicationBarCommands.Copy) ||
                         (e.Command == ApplicationBarCommands.Cut) ||
                         (e.Command == ApplicationBarCommands.Delete))
                {
                    e.CanExecute = (!menuItem.IsBackItem);
                }
                else
                {
                    e.CanExecute = ((!menuItem.IsFolder) && (!menuItem.IsBackItem));
                }
            }
        }

        private void AddFolder(DrillDownMenuItem drillDownMenuItem)
        {
            if (favoritesMenu.TopMenuItem != null)
            {
                DrillDownMenuItem newFolder = new DrillDownMenuItem()
                {
                    IsFolder = true,
                    Caption = StringResources.NewFavoritesFolder_Text,
                    Parent = drillDownMenuItem.Parent
                };

                DrillDownMenuItem backButton = new DrillDownMenuItem()
                {
                    IsBackItem = true,
                    Caption = newFolder.Parent.Caption,
                    Parent = newFolder
                };

                newFolder.Children.Add(backButton);

                int idx = drillDownMenuItem.Parent.Children.IndexOf(drillDownMenuItem);

                if (drillDownMenuItem.IsBackItem)
                {
                    idx++;
                }

                drillDownMenuItem.Parent.Children.Insert(idx, newFolder);
            }
        }

        private void PasteFavorite(DrillDownMenuItem pasteTargetItem, DrillDownMenuItem pasteItem)
        {
            if (favoritesMenu.TopMenuItem != null)
            {
                DrillDownMenuItem cloneEntry = pasteItem.Clone() as DrillDownMenuItem;
                int idx = 0;

                if (pasteTargetItem.IsFolder)
                {
                    cloneEntry.Parent = pasteTargetItem;
                    idx = pasteTargetItem.Children.Count;
                }
                else if (pasteTargetItem.IsBackItem)
                {
                    cloneEntry.Parent = pasteTargetItem.Parent.Parent;
                    idx = pasteTargetItem.Parent.Parent.Children.Count;
                }
                else
                {
                    cloneEntry.Parent = pasteTargetItem.Parent ?? favoritesMenu.TopMenuItem;

                    idx = cloneEntry.Parent.Children.IndexOf(pasteTargetItem);

                    if (pasteTargetItem.IsBackItem)
                    {
                        idx++;
                    }
                }

                cloneEntry.Parent.Children.Insert(idx, cloneEntry);
            }
        }

        private void DeleteFavorite(DrillDownMenuItem drillDownMenuItem)
        {
            if (favoritesMenu.TopMenuItem != null)
            {
                RemoveMenuItem(favoritesMenu.TopMenuItem, drillDownMenuItem);
            }
        }

        private bool RemoveMenuItem(DrillDownMenuItem parent, DrillDownMenuItem itemToDelete)
        {
            if (parent.Children.Contains(itemToDelete))
            {
                itemToDelete.Parent = null;
                parent.Children.Remove(itemToDelete);
                return true;
            }
            else
            {
                foreach (DrillDownMenuItem childItem in parent.Children)
                {
                    if (RemoveMenuItem(childItem, itemToDelete))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddToFavorites(DrillDownMenuItem drillDownMenuItem, DrillDownMenuItem targetFolder)
        {
            if (favoritesMenu.TopMenuItem != null)
            {
                DrillDownMenuItem cloneEntry = drillDownMenuItem.Clone() as DrillDownMenuItem;
                DrillDownMenuItem currentFolder = favoritesMenu.CurrentFolder;

                if (targetFolder != null)
                {
                    currentFolder = targetFolder;
                }

                if (currentFolder.IsBackItem)
                {
                    currentFolder = currentFolder.Parent ?? favoritesMenu.TopMenuItem;
                }

                cloneEntry.Parent = currentFolder;

                currentFolder.Children.Add(cloneEntry);
            }
        }

        private void ContextCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DrillDownMenuItem menuItem = GetMenuItem(e.OriginalSource);

            if (menuItem != null)
            {
                if (e.Command == ApplicationBarCommands.AddToFavorites)
                {
                    AddToFavorites(menuItem, (DrillDownMenuItem)e.Parameter);
                }
                else if (e.Command == ApplicationBarCommands.AddFolder)
                {
                    AddFolder(menuItem);
                }
                else if (e.Command == ApplicationBarCommands.Delete)
                {
                    DeleteFavorite(menuItem);
                }
                else if (e.Command == ApplicationBarCommands.Paste)
                {
                    PasteFavorite(menuItem, _rememberItem);
                    _rememberItem = null;
                }
                else if (e.Command == ApplicationBarCommands.Rename)
                {
                    menuItem.IsReadOnly = false;
                }
                else if (e.Command == ApplicationBarCommands.Copy)
                {
                    _rememberItem = menuItem;
                }
                else if (e.Command == ApplicationBarCommands.Cut)
                {
                    _rememberItem = menuItem;
                    DeleteFavorite(menuItem);
                }
                else if (e.Command == ApplicationBarCommands.OpenInNewWindow)
                {
                    StartMenuItemExecuted(this, new StartMenuItemExecutedEventArgs(menuItem as ShellDrillDownMenuItem, null, StartOption.NewWindow));
                }
                else if (e.Command == ApplicationBarCommands.AddToDashboard)
                {
                    StartMenuItemExecuted(this, new StartMenuItemExecutedEventArgs(menuItem as ShellDrillDownMenuItem, null, StartOption.Dashboard));
                }
            }
        }

        private void StartContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _favoriteFolders.Clear();
            foreach (DrillDownMenuItem item in favoritesMenu.GetAllFolders)
            {
                _favoriteFolders.Add(item);
            }
        }

        private void favoritesMenu_Drop(object sender, DragEventArgs e)
        {
            DrillDownMenuItem dropTarget = favoritesMenu.GetDropTarget(e);
            DrillDownMenuItem dropObject = (DrillDownMenuItem)e.Data.GetData(typeof(DrillDownMenuItem));
            if (dropTarget != null && dropObject != null)
            {
                if (dropTarget != dropObject)
                {
                    PasteFavorite(dropTarget, dropObject);
                    DeleteFavorite(dropObject);
                }
            }
        }

        private void Menu_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.MouseDevice.Captured != null)
                {
                    if (e.MouseDevice.Captured.GetType().IsAssignableFrom(typeof(ListBox))
                        && ((ListBox)e.MouseDevice.Captured).SelectedItem != null
                        && typeof(DrillDownMenuItem).IsAssignableFrom(((ListBox)e.MouseDevice.Captured).SelectedItem.GetType())
                        && !((DrillDownMenuItem)((ListBox)e.MouseDevice.Captured).SelectedItem).IsBackItem)
                    {
                        // Package the data.
                        DrillDownMenuItem selected = (DrillDownMenuItem)((ListBox)e.MouseDevice.Captured).SelectedItem;

                        DataObject data = new DataObject();
                        data.SetData(typeof(DrillDownMenuItem), selected);

                        DragDropEffects dEffect;

                        if (sender == favoritesMenu)
                        {
                            dEffect = DragDropEffects.Move;
                        }
                        else
                        {
                            dEffect = DragDropEffects.Copy;
                        }

                        favoritesMenu.AllowDrop = true;
                        // Inititate the drag-and-drop operation.
                        DragDrop.DoDragDrop(this, data, dEffect);

                        favoritesMenu.AllowDrop = false;
                    }
                }
            }
        }
    }

    public class DummySource
    {
        public bool IsKeyTipModeActive
        {
            get
            {
                return false;
            }
        }
    }
}
