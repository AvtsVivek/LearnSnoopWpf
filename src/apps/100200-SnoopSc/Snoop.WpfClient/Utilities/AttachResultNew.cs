namespace Snoop.WpfClient.Utilities
{
    using System;

    public class AttachResultNew
    {
        public AttachResultNew()
        {
            this.Success = true;
        }

        public AttachResultNew(Exception attachException)
        {
            this.Success = false;

            this.AttachException = attachException;
        }

        public bool Success { get; }

        public Exception? AttachException { get; }
    }
}
