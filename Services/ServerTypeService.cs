using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceDemo.Services
{
    public class ServerTypeService : ServiceTypeDemo.ServiceTypeDemoBase
    {
        public override Task<ExampleResponse> UnaryCall(ExampleRequest request, ServerCallContext context)
        {
            ExampleResponse resp = GetResp();
            return Task.FromResult(resp);
        }

        private static ExampleResponse GetResp()
        {
            var resp = new ExampleResponse();
            resp.Total = new Random().Next(1, 20);
            resp.Result.Add("abc");
            resp.Result.Add("efg");
            return resp;
        }

        public async override Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            var responses = new List<ExampleResponse>();
            responses.Add(GetResp());
            responses.Add(GetResp());
            responses.Add(GetResp());
            foreach (var response in responses)
            {
                //写入流中
                await responseStream.WriteAsync(response);
            }
        }

        public async override Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
        {
            int pointCount = 0;
            //读取传入流
            while (await requestStream.MoveNext())
            {
                var point = requestStream.Current;
                pointCount++;
            }
            var info = GetResp();
            info.Total = pointCount;
            return info;
        }

        public async override Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            //读取内容
            while (await requestStream.MoveNext())
            {
                var note = requestStream.Current;
                var responses = new List<ExampleResponse>();
                responses.Add(GetResp());
                responses.Add(GetResp());
                //循环写入
                foreach (var prevNote in responses)
                {
                    await responseStream.WriteAsync(prevNote);
                }
            }
        }
    }
}
