using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClient
{
    /// <summary>
    /// 传输结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TransferResult<T>
    {
        /// <summary>
        /// 传输是否成功
        /// </summary>
        public bool IsSuccessful { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 标记类型
        /// </summary>
        public T Tag { get; set; } = default;
    }
}
