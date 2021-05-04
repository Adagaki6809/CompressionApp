using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.IO.Compression;

namespace CompressionApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // δξαΰβλÿεμ ρεπβθρ κξμοπερρθθ
            services.AddResponseCompression(options => { 
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add(new DeflateCompressionProvider()); 
            });

            //services.Configure<GzipCompressionProviderOptions>(options =>
            //{
            //    options.Level = CompressionLevel.Optimal;
            //});
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
                }
            });

            // οξδκλώχΰεμ κξμοπερρθώ
            app.UseResponseCompression();

            app.Run(async context =>
            {
                // ξςοπΰβλÿεμϋι ςεκρς
                string loremIpsum = "Lorem Ipsum is simply dummy text ... including versions of Lorem Ipsum.";
                // σρςΰνξβκΰ mime-ςθοΰ ξςοπΰβλÿεμϋυ δΰννϋυ
                context.Response.ContentType = "text/html";
                // ξςοπΰβκΰ ξςβεςΰ
                //await context.Response.SendFileAsync(@"D:\Programs\CompressionApp\CompressionApp\wwwroot\index.html");
                await context.Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/index.html"));
            });
        }
    }
}