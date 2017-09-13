﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DataAccess
{
    public class RequestCollection : IEnumerable<Request>
    {
        private static IEnumerable<Request> empty = new Request[0];
        private IEnumerable<Request> items;

        public RequestCollection()
        {
            this.items = empty;
        }

        public RequestCollection(IEnumerable<Request> items)
        {
            this.items = items;
        }

        public RequestCollection(Request item)
        {
            this.items = new Request[] { item };
        }

        public static RequestCollection operator +(RequestCollection left, IEnumerable<Request> right)
        {
            IEnumerable<Request> left_items = empty;
            IEnumerable<Request> right_items = empty;

            if (left != null) left_items = left;
            if (right != null) right_items = right;

            return new RequestCollection(left_items.Concat(right_items));
        }

        public static RequestCollection operator +(RequestCollection left, Request right)
        {
            IEnumerable<Request> left_items = empty;
            IEnumerable<Request> right_items = empty;

            if (left != null) left_items = left;
            if (right != null) right_items = new Request[] { right };

            return new RequestCollection(left_items.Concat(right_items));
        }

        public IEnumerator<Request> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public string ToXml()
        {
            var sb = new StringBuilder();
         
            using (var writer = XmlWriter.Create(sb))
            {
                writer.WriteStartElement("RequestList");

                foreach (var request in items)
                {
                    writer.WriteStartElement("Request");
                    writer.WriteElementString("Id", Convert.ToString(request.Id));

                    writer.WriteStartElement("Sections");

                    foreach (var section in request.Sections)
                    {
                        writer.WriteStartElement(section.Key);

                        foreach (var item in section.Value)
                        {
                            writer.WriteStartElement("Item");
                            writer.WriteElementString("Name", Convert.ToString(item.Name));
                            writer.WriteElementString("IsNull", Convert.ToString(item.IsNull));
                            writer.WriteElementString("Value", Convert.ToString(item.Value));
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
              
            }

            return sb.ToString();
        }

        public static RequestCollection FromXml(string xml)
        {
            var result = new RequestCollection();

           
            var docX=XDocument.Load(xml);
            var requests=docX.Descendants("//Request");
            
            foreach (XElement node in requests)
            {
                
                var request = new Request();
                //var id = node.Value;
              
                
                ////var id = node.SelectSingleNode("Id").InnerText;

                //request.Id = new Guid(id);


                //var sections = node.SelectSingleNode("Sections");

                //var sections = node.SelectSingleNode("Sections");
                //if (sections != null)
                //{
                //    foreach (XNode section_node in sections.ChildNodes)
                //    {
                //        var section = section_node.Name;

                //        foreach (XNode item in section_node.ChildNodes)
                //        {
                //            var item_isnull = Convert.ToBoolean(item.SelectSingleNode("IsNull").InnerText);
                //            var item_name = item.SelectSingleNode("Name").InnerText;
                //            var item_value = item.SelectSingleNode("Value").InnerText;

                //            if (item_isnull)
                //                item_value = null;

                //            request[section].Add(item_name, item_value);
                //        }
                //    }
                //}

                result += request;
            }


           

            return result;
        }
    }
}