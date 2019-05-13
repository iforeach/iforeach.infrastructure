using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.FileProviders;

namespace org.iForeach.Modules.FileProviders
{
    public class EmbeddedDirectoryContents : IDirectoryContents
    {
        private readonly IList<IFileInfo> _entries;

        public EmbeddedDirectoryContents(IEnumerable<IFileInfo> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            this._entries = entries.ToList();
        }

        public bool Exists
        {
            get { return this._entries.Any(); }
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return this._entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._entries.GetEnumerator();
        }
    }
}