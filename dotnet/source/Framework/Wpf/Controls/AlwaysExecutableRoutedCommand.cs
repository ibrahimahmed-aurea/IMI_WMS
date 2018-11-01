using System;
using System.Windows;
using System.Windows.Input;

namespace Imi.Framework.Wpf.Controls
{
   
   /// <summary>
   /// A wrapper for a <see cref="RoutedCommand"/> that always returns true for <see cref="ICommand.CanExecute"/>.
   /// </summary>
   /// <remarks>
   /// A <see cref="RoutedCommand"/> attaches its <see cref="ICommand.CanExecuteChanged"/> event
   /// to the <see cref="CommandManager.RequerySuggested"/> event. That means that each user
   /// interaction causes the <see cref="RoutedEvent"/>s <see cref="CommandManager.PreviewCanExecuteEvent"/>
   /// and <see cref="CommandManager.CanExecuteEvent"/> to be raised. But <see cref="RoutedEvent"/>s
   /// have a quite bad performance with complex visual trees which means that <see cref="RoutedCommand"/>s
   /// also have that problem.
   /// This wrapper can be used to optimize the application performance in cases where it doesn't
   /// matter if <see cref="ICommand.CanExecute"/> always returns true.
   /// </remarks>
   internal sealed class AlwaysExecutableRoutedCommand : ICommand {
      private readonly RoutedCommand _originalCommand;
      private readonly IInputElement _target;

      public AlwaysExecutableRoutedCommand(
         RoutedCommand originalCommand,
         IInputElement target
      ) {
         if (originalCommand == null) {
            throw new ArgumentNullException("originalCommand");
         }

         if (target == null) {
            throw new ArgumentNullException("target");
         }

         _originalCommand = originalCommand;
         _target = target;
      }

      /// <summary>
      /// This event is never raised and it also doesn't save references to handlers that are
      /// attached to it (no-op in add and remove).
      /// </summary>
      public event EventHandler CanExecuteChanged {
         add { }
         remove { }
      }

      /// <summary>
      /// Always returns true.
      /// </summary>
      public bool CanExecute(object parameter) {
         return true;
      }

      /// <summary>
      /// Executes the wrapped <see cref="RoutedCommand"/>.
      /// </summary>
      public void Execute(object parameter) {
         _originalCommand.Execute(parameter, _target);
      }
   }
}
