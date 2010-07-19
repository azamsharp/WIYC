namespace WIYCLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.SessionState;

    public class SharpHandler : IHttpHandler, IRequiresSessionState
    {
        private void FillApplicationObjects(List<IObject> objects, HttpContext context)
        {
            NameObjectCollectionBase.KeysCollection keys = context.Application.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                ApplicationObject item = new ApplicationObject(keys[i], context.Application[keys[i]], context.Application[keys[i]].GetType().Name);
                objects.Add(item);
            }
        }

        private void FillCacheObjects(List<IObject> objects, HttpContext context)
        {
            IDictionaryEnumerator enumerator = context.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CacheObject item = new CacheObject((string) enumerator.Key, enumerator.Value, enumerator.Value.GetType().Name);
                objects.Add(item);
            }
        }

        private void FillSessionObjects(List<IObject> objects, HttpContext context)
        {
            NameObjectCollectionBase.KeysCollection keys = context.Session.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                SessionObject item = new SessionObject(keys[i], context.Session[keys[i]], context.Session[keys[i]].GetType().Name);
                objects.Add(item);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            List<IObject> objects = new List<IObject>();
            string userSelection = string.Empty;
            if (context.Request.QueryString["menuItem"] != null)
            {
                userSelection = context.Request.QueryString["menuItem"];
            }
            if (userSelection.Equals("Cache"))
            {
                if (context.Request.QueryString["deleteKey"] != null)
                {
                    context.Cache.Remove(context.Request.QueryString["deleteKey"]);
                }
                this.FillCacheObjects(objects, context);
            }
            if (userSelection.Equals("Session"))
            {
                if (context.Request.QueryString["deleteKey"] != null)
                {
                    context.Session.Remove(context.Request.QueryString["deleteKey"]);
                }
                this.FillSessionObjects(objects, context);
            }
            if (userSelection.Equals("Application"))
            {
                if (context.Request.QueryString["deleteKey"] != null)
                {
                    context.Application.Remove(context.Request.QueryString["deleteKey"]);
                }
                this.FillApplicationObjects(objects, context);
            }
            string key = context.Request.QueryString["key"];
            string s = TableHelper.GeneratePage(objects, key, userSelection);
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

