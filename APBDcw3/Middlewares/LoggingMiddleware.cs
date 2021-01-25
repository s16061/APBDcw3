using APBDcw3.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APBDcw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string log = "requestsLog.txt";

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            if(httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method;
                string query = httpContext.Request.QueryString.ToString();
                string bodyStr = "";
                var date = DateTime.Now;

                using (var reader=new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;

                }

                using (var streamwriter = new StreamWriter(log, true))
                {
                    streamwriter.WriteLine($"date: {date}, path: {path}, method: {method}, query: {query}, bodyStr: {bodyStr}");
                }

            }


            await _next(httpContext);
        }
    }
}
