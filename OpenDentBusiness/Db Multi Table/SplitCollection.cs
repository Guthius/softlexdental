using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    [Serializable]
    public class SplitCollection : ICollection<PaySplit>, IXmlSerializable
    {
        private List<PaySplit> paySplits = new List<PaySplit>();

        public int Count => paySplits.Count;

        public bool IsReadOnly => false;

        public void Add(PaySplit paySplit)
        {
            if (!Contains(paySplit))
            {
                if (string.IsNullOrEmpty((string)paySplit.ODTag))
                {
                    paySplit.ODTag = Guid.NewGuid().ToString();
                }

                paySplits.Add(paySplit);
            }
        }

        public void AddRange(List<PaySplit> paySplits)
        {
            foreach (PaySplit paySplit in paySplits)
            {
                Add(paySplit);
            }
        }

        public void Clear() => paySplits.Clear();
        

        public bool Contains(PaySplit paySplit) =>
            paySplits.Any(x => x.ODTag == paySplit.ODTag || (x.SplitNum == paySplit.SplitNum && x.SplitNum != 0));

        public void CopyTo(PaySplit[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < paySplits.Count; i++)
            {
                array[i] = paySplits[i];
            }
        }

        public bool Remove(PaySplit paySplit)
        {
            return paySplits.RemoveAll(x => x.ODTag == paySplit.ODTag || (x.SplitNum == paySplit.SplitNum && x.SplitNum != 0)) > 0;
        }

        public IEnumerator<PaySplit> GetEnumerator() => paySplits.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => paySplits.GetEnumerator();

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PaySplit>));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
            {
                return;
            }
            paySplits = (List<PaySplit>)serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PaySplit>));
            serializer.Serialize(writer, paySplits);
        }
    }
}