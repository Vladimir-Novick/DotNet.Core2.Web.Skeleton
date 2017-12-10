using System;
using System.Runtime.Serialization;

namespace DotNet.Core2.Web.Skeleton.Code
{
    [DataContract(Namespace = ""), Serializable]
    public class UserDataItem
    {

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public String Key { get; set; }

        [DataMember]
        public int IntData { get; set; }


        [DataMember]
        public string TextData { get; set; }

    }
}
