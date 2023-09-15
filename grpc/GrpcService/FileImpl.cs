using Grpc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcService
{
    /// <summary>
    /// 文件传输类
    /// </summary>
    public class FileImpl : GrpcService.FileTransfer.FileTransferBase
    {
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="request">下载请求</param>
        /// <param name="responseStream">文件写入流</param>
        /// <param name="context">站点上下文</param>
        /// <returns></returns>
        public override async Task FileDownload(FileRequest request, global::Grpc.Core.IServerStreamWriter<FileReply> responseStream, global::Grpc.Core.ServerCallContext context)
        {
            // 传输成功的文件
            List<string> lstSuccessFile = new List<string>();
            // 传输文件的开始时间
            DateTime startTime = DateTime.Now;
            // 每次读取的数据
            int chunkSize = 1024 * 1024;
            // 数据缓冲区
            var buffer = new byte[chunkSize];
            // 文件流
            FileStream fs = null;
            try
            {
                for (int i = 0; i < request.FileNames.Count; i++)
                {
                    var fileName = request.FileNames[i];
                    var filePath = Path.GetFullPath($".//Files\\{fileName}");
                    // 应答数据
                    FileReply fileReply = new FileReply
                    {
                        FileName = fileName,
                        Mark = request.Mark,
                    };
                    // 写入日志，下载文件
                    Console.WriteLine($"{request.Mark},下载文件：{filePath}");
                    if (File.Exists(filePath))
                    {
                        fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, chunkSize, useAsync: true);

                        // fs.Length  可以告诉客户端所传文件的大小
                        int readTimes = 0;
                        while (true)
                        {
                            int readSize = fs.Read(buffer, 0, buffer.Length);
                            if (readSize > 0)
                            {
                                fileReply.Block = ++readTimes;
                                fileReply.Content = Google.Protobuf.ByteString.CopyFrom(buffer);
                                await responseStream.WriteAsync(fileReply);
                            }
                            else
                            {
                                fileReply.Block = 0;
                                fileReply.Content = Google.Protobuf.ByteString.Empty;
                                await responseStream.WriteAsync(fileReply);
                                lstSuccessFile.Add(fileName);
                                Console.WriteLine($"{request.Mark},完成发送文件{fileName}");
                                break;
                            }
                        }
                        fs?.Close();
                    }
                    else
                    {
                        Console.WriteLine($"文件[{fileName}]不存在");
                        fileReply.Block = -1;
                        await responseStream.WriteAsync(fileReply);
                    }
                }

                // 告诉客户端，文件传输完成
                await responseStream.WriteAsync(new FileReply
                {
                    FileName = string.Empty,
                    Block = -2,
                    Content = Google.Protobuf.ByteString.Empty,
                    Mark = request.Mark
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{request.Mark}，发生异常({ex.GetType()})：{ex.Message}");
            }
            finally
            {
                fs?.Dispose();
            }

            Console.WriteLine($"{request.Mark}，文件传输完成。共计【{lstSuccessFile.Count / request.FileNames.Count}】，耗时：{DateTime.Now - startTime}");
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task FileUpload(IAsyncStreamReader<FileReply> requestStream, IServerStreamWriter<FileReturn> responseStream, ServerCallContext context)
        {
            List<string> lstFilesName = new List<string>();//文件名
            List<FileReply> lstContents = new List<FileReply>();//数据集合

            FileStream fs = null;
            DateTime startTime = DateTime.Now;//开始时间
            string mark = string.Empty;
            string savePath = string.Empty;
            try
            {
                //reply.Block数字的含义是服务器和客户端约定的
                while (await requestStream.MoveNext())//读取数据
                {
                    var reply = requestStream.Current;
                    mark = reply.Mark;
                    if (reply.Block == -2)//传输完成
                    {
                        Console.WriteLine($"{mark}，完成上传文件。共计【{lstFilesName.Count}】个，耗时：{DateTime.Now - startTime}");
                        break;
                    }
                    else if (reply.Block == -1)//取消了传输
                    {
                        Console.WriteLine($"文件【{reply.FileName}】取消传输！");//写入日志
                        lstContents.Clear();
                        fs?.Close();//释放文件流
                        if (!string.IsNullOrEmpty(savePath) && File.Exists(savePath))//如果传输不成功，删除该文件
                        {
                            File.Delete(savePath);
                        }
                        savePath = string.Empty;
                        break;
                    }
                    else if (reply.Block == 0)//文件传输完成
                    {
                        if (lstContents.Any())//如果还有数据，就写入文件
                        {
                            lstContents.OrderBy(c => c.Block).ToList().ForEach(c => c.Content.WriteTo(fs));
                            lstContents.Clear();
                        }
                        lstFilesName.Add(savePath);//传输成功的文件
                        fs?.Close();//释放文件流
                        savePath = string.Empty;

                        //告知客户端，已经完成传输
                        await responseStream.WriteAsync(new FileReturn
                        {
                            FileName = reply.FileName,
                            Mark = mark
                        });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(savePath))//有新文件来了
                        {
                            savePath = Path.GetFullPath($".//Files\\{reply.FileName}");//文件路径
                            fs = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite);
                            Console.WriteLine($"{mark}，上传文件：{savePath}，{DateTime.UtcNow.ToString("HH:mm:ss:ffff")}");
                        }
                        lstContents.Add(reply);//加入链表
                        if (lstContents.Count() >= 20)//每个包1M，20M为一个集合，一起写入数据。
                        {
                            lstContents.OrderBy(c => c.Block).ToList().ForEach(c => c.Content.WriteTo(fs));
                            lstContents.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{mark}，发生异常({ex.GetType()})：{ex.Message}");
            }
            finally
            {
                fs?.Dispose();
            }
        }
    }
}
