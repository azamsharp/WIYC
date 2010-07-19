namespace WIYCLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;

    public class TableHelper
    {
        private static string ConvertPanelToHTML(Panel panel)
        {
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            panel.RenderControl(writer2);
            return writer.ToString();
        }

        private static Table CreateDetailsPane(string key, string selection)
        {
            Table table = new Table {
                CssClass = "detailsTable"
            };
            object o = null;
            if (!string.IsNullOrEmpty(key))
            {
                if (selection.Equals("Cache"))
                {
                    o = HttpContext.Current.Cache[key];
                }
                else if (selection.Equals("Session"))
                {
                    o = HttpContext.Current.Session[key];
                }
                else if (selection.Equals("Application"))
                {
                    o = HttpContext.Current.Application[key];
                }
                string s = SerializeObject(o);
                TableRow row = new TableRow {
                    CssClass = "itemTitle"
                };
                TableCell cell = new TableCell {
                    Text = "Value"
                };
                row.Cells.Add(cell);
                TableRow row2 = new TableRow();
                TableCell cell2 = new TableCell {
                    Text = HttpContext.Current.Server.HtmlEncode(s)
                };
                row2.Cells.Add(cell2);
                table.Rows.Add(row);
                table.Rows.Add(row2);
            }
            return table;
        }

        private static Table CreateMenu()
        {
            string[] strArray = new string[] { "Cache", "Session", "Application" };
            Table table = new Table {
                CssClass = "menu",
                BorderWidth = 2
            };
            TableRow row = null;
            TableCell cell = null;
            foreach (string str in strArray)
            {
                HyperLink child = new HyperLink();
                child.Style.Add("text-decoration", "none");
                child.Text = str;
                child.NavigateUrl = "CacheAndSession.axd?menuItem=" + str;
                row = new TableRow();
                row.Attributes.Add("onmouseover", "this.style.backgroundColor='FF9933'");
                row.Attributes.Add("onmouseout", "this.style.backgroundColor='FFFF99'");
                cell = new TableCell();
                cell.Controls.Add(child);
                row.Cells.Add(cell);
                table.Rows.Add(row);
            }
            return table;
        }

        private static Table CreateTable(List<IObject> cacheObjects, string userSelection)
        {
            string[] strArray = new string[] { "Key", "Type", "Size(KB)", "DateCreated", "DateExpires", "File Dependencies" };
            Table table = new Table {
                CssClass = "tableStyle",
                CellPadding = 5
            };
            TableHeaderRow row = new TableHeaderRow();
            TableHeaderCell cell = null;
            foreach (string str in strArray)
            {
                cell = new TableHeaderCell {
                    Text = str
                };
                row.Cells.Add(cell);
                table.Rows.Add(row);
            }
            TableRow row2 = null;
            TableCell cell2 = null;
            TableCell cell3 = null;
            TableCell cell4 = null;
            TableCell cell5 = null;
            TableCell cell6 = null;
            TableCell cell7 = null;
            TableCell cell8 = null;
            TableCell cell9 = null;
            HyperLink child = null;
            HyperLink link2 = null;
            if (cacheObjects.Count > 0)
            {
                foreach (IObject obj2 in cacheObjects)
                {
                    row2 = new TableRow();
                    cell2 = new TableCell {
                        Text = obj2.Key
                    };
                    cell3 = new TableCell {
                        Text = (obj2.Value is string) ? ((string) obj2.Value) : "[Serialized]"
                    };
                    cell4 = new TableCell {
                        Text = obj2.Value.GetType().Name
                    };
                    cell5 = new TableCell();
                    child = new HyperLink {
                        Text = "view details",
                        NavigateUrl = "CacheAndSession.axd?key=" + obj2.Key + "&&menuItem=" + userSelection
                    };
                    cell5.Controls.Add(child);
                    cell6 = new TableCell();
                    link2 = new HyperLink {
                        Text = "Delete",
                        NavigateUrl = "CacheAndSession.axd?deleteKey=" + obj2.Key + "&&menuItem=" + userSelection
                    };
                    cell6.Controls.Add(link2);
                    cell7 = new TableCell {
                        Text = GetObjectSize(obj2).ToString()
                    };
                    cell8 = new TableCell();
                    cell9 = new TableCell();
                    cell8.Text = "N/A";
                    cell9.Text = "N/A";
                    TableCell cell10 = new TableCell();
                    if (obj2 is CacheObject)
                    {
                        CacheEntryFake cacheEntry = GetCacheEntry(obj2);
                        cell8.Text = cacheEntry.UtcCreated.ToLocalTime().ToShortDateString() + " " + cacheEntry.UtcCreated.ToLocalTime().ToLongTimeString();
                        cell9.Text = cacheEntry.UtcCreated.ToLocalTime().ToShortDateString() + " " + cacheEntry.UtcExpire.ToLocalTime().ToLongTimeString();
                        if (cacheEntry.DependencyInfo.Files != null)
                        {
                            cell10.Text = string.Join(",", cacheEntry.DependencyInfo.Files);
                        }
                    }
                    row2.Cells.Add(cell2);
                    row2.Cells.Add(cell4);
                    row2.Cells.Add(cell7);
                    row2.Cells.Add(cell8);
                    row2.Cells.Add(cell9);
                    row2.Cells.Add(cell10);
                    row2.Cells.Add(cell5);
                    row2.Cells.Add(cell6);
                    table.Rows.Add(row2);
                }
                return table;
            }
            if (!string.IsNullOrEmpty(userSelection))
            {
                row2 = new TableRow();
                TableCell cell11 = new TableCell {
                    Text = "There are no items in " + userSelection
                };
                row2.Cells.Add(cell11);
                table.Rows.Add(row2);
            }
            return table;
        }

        public static string GeneratePage(List<IObject> cacheObjects, string key, string userSelection)
        {
            Panel panel = new Panel();
            panel.Controls.Add(GetStyles());
            panel.Controls.Add(GetCredits());
            panel.Controls.Add(GetSpacing());
            panel.Controls.Add(CreateMenu());
            panel.Controls.Add(GetSpacing());
            panel.Controls.Add(GetItemTitle(userSelection, cacheObjects.Count));
            panel.Controls.Add(CreateTable(cacheObjects, userSelection));
            panel.Controls.Add(GetSpacing());
            panel.Controls.Add(CreateDetailsPane(key, userSelection));
            return ConvertPanelToHTML(panel);
        }

        private static CacheEntryFake GetCacheEntry(IObject cacheObject)
        {
            CacheEntryFake fake = new CacheEntryFake();
            object obj3 = ReflectionHelper.InvokeMethod(ReflectionHelper.GetFieldValue(HttpContext.Current.Cache, "_cacheInternal"), cacheObject.Key, "DoGet", new object[] { true, cacheObject.Key, 1 });
            fake.UtcCreated = (DateTime) ReflectionHelper.GetPropertyValue(obj3, "UtcCreated");
            fake.UtcExpire = (DateTime) ReflectionHelper.GetPropertyValue(obj3, "UtcExpires");
            fake.DependencyInfo.Dependency = (CacheDependency) ReflectionHelper.GetFieldValue(obj3, "_dependency");
            if (fake.DependencyInfo.Dependency != null)
            {
                fake.DependencyInfo.Files = (string[]) ReflectionHelper.InvokeMethod(fake.DependencyInfo.Dependency, cacheObject.Key, "GetFileDependencies", null);
            }
            return fake;
        }

        private static Table GetCredits()
        {
            Table table = new Table {
                CssClass = "creditsTable"
            };
            TableRow row = new TableRow();
            TableCell cell = new TableCell {
                Text = "WIYC (What's in your cache?) developed by <a href='mailto:azamsharp@gmail.com'>Mohammad Azam</a>"
            };
            row.Cells.Add(cell);
            table.Rows.Add(row);
            return table;
        }

        private static Table GetItemTitle(string selection, int noOfItems)
        {
            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            table.CssClass = "itemTitle";
            Literal child = new Literal();
            if (!string.IsNullOrEmpty(selection))
            {
                child.Text = string.Concat(new object[] { selection.ToUpper(), " ITEMS(", noOfItems, ")" });
            }
            cell.Controls.Add(child);
            row.Cells.Add(cell);
            table.Rows.Add(row);
            return table;
        }

        private static double GetObjectSize(object o)
        {
            double num = -1.0;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream serializationStream = new MemoryStream();
            try
            {
                formatter.Serialize(serializationStream, o);
                byte[] buffer = new byte[serializationStream.Length];
                serializationStream.Close();
                num = ((double) buffer.Length) / 1000.0;
            }
            catch (Exception)
            {
            }
            return num;
        }

        private static Literal GetSpacing()
        {
            string str = "<BR><BR>";
            return new Literal { Text = str };
        }

        private static Literal GetStyles()
        {
            string str = "<style type=\"text/css\"> .menu { font-family:Verdana; padding:2em; font-size:10px; background-color:FFFF99; }    \r\n                            .tableStyle { font-family:Verdana; position:relative; font-size:10px; padding:2em; border-width:0px; background-color:CCFFFF; width:80%; } \r\n                            .detailsTable { font-family:Verdana; font-size:10px; width:80%; background-color:CCFFCC;  }\r\n                            .itemTitle { background-color:3399FF; font-weight:bold; font-family:Verdana; padding:2em; width:80%; overflow:hidden; }\r\n                            .creditsTable { font-family:Verdana;font-size:10px; font-weight:bold; }   \r\n                            \r\n                </style>";
            return new Literal { Text = str };
        }

        private static string SerializeObject(object o)
        {
            StringBuilder output = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(output);
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            try
            {
                serializer.Serialize(xmlWriter, o);
            }
            catch (Exception)
            {
                output.Append("Failed to load the details.");
            }
            return output.ToString();
        }
    }
}

