using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Imi.Framework.Shared
{
    public class ExceptionHelper
    {
        private ExceptionHelper()
        { 
        }

        public static bool IsCritical(Exception ex)
        {
            if (ex is OutOfMemoryException)
                return true;
            else if (ex is AccessViolationException)
                return true;
            else if (ex is MemberAccessException)
                return true;
            else if (ex is NotImplementedException)
                return true;
            else if (ex is TypeLoadException)
                return true;
            else if (ex is TypeUnloadedException)
                return true;
            else if (ex is AppDomainUnloadedException)
                return true;
            else if (ex is BadImageFormatException)
                return true;
            else if (ex is CannotUnloadAppDomainException)
                return true;
            else if (ex is InvalidProgramException)
                return true;
            else if (ex is ThreadAbortException)
                return true;
            else
                return false;
        }
    }
}
