namespace WIYCLibrary
{
    using System;

    public class CacheEntryFake
    {
        private WIYCLibrary.DependencyInfo _dependencyInfo = new WIYCLibrary.DependencyInfo();
        private DateTime _utcCreated;
        private DateTime _utcExpire;

        public WIYCLibrary.DependencyInfo DependencyInfo
        {
            get
            {
                return this._dependencyInfo;
            }
            set
            {
                this._dependencyInfo = value;
            }
        }

        public DateTime UtcCreated
        {
            get
            {
                return this._utcCreated;
            }
            set
            {
                this._utcCreated = value;
            }
        }

        public DateTime UtcExpire
        {
            get
            {
                return this._utcExpire;
            }
            set
            {
                this._utcExpire = value;
            }
        }
    }
}

