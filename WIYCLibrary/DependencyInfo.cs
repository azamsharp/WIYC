namespace WIYCLibrary
{
    using System;
    using System.Web.Caching;

    public class DependencyInfo
    {
        private CacheDependency _dependency;
        private string[] _files;
        private bool _isFileDependency;

        public CacheDependency Dependency
        {
            get
            {
                return this._dependency;
            }
            set
            {
                this._dependency = value;
            }
        }

        public string[] Files
        {
            get
            {
                return this._files;
            }
            set
            {
                this._files = value;
            }
        }

        public bool IsFileDependency
        {
            get
            {
                return this._isFileDependency;
            }
            set
            {
                this._isFileDependency = value;
            }
        }
    }
}

