using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch.Model
{
    public class NotationModel
    {
        /// <summary>
        /// 二进制值
        /// </summary>
        [JsonProperty(PropertyName = "bin")]
        public string Bin { get; set; }
        /// <summary>
        /// 八进制值
        /// </summary>
        [JsonProperty(PropertyName = "oct")]
        public string Oct { get; set; }
        /// <summary>
        /// 十进制值
        /// </summary>
        [JsonProperty(PropertyName = "dec")]
        public string Dec { get; set; }
        /// <summary>
        /// 十六进制值
        /// </summary>
        [JsonProperty(PropertyName = "hex")]
        public string Hex { get; set; }
        /// <summary>
        /// 字符串值
        /// </summary>
        [JsonProperty(PropertyName = "val")]
        public string Value { get; set; }
    }
}
