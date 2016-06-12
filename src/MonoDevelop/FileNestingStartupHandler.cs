
using MadsKristensen.FileNesting;
using MonoDevelop.Components.Commands;

namespace  MonoDevelop.FileNesting
{
    class FileNestingStartupHandler : CommandHandler
    {
        protected override void Run ()
        {
            FileNestingFactory.Enable();
        }
    }
}

