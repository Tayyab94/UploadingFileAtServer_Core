using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;

namespace UploadingFileAtServer_Core.Controllers
{
    public class VideoController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly string dir;


        // if u are working on saving the Videos you have to installed the xabe.ffmpeg library from the  nuget pakage.. 



        public VideoController(IHostingEnvironment environment)
        {
            this.environment = environment;
           dir = environment.WebRootPath;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Index(IFormFile file, double start, double end)
        //{

        //    using (var fileStreem = new FileStream(Path.Combine(dir, "file.mp4"), FileMode.Create, FileAccess.Write))
        //    {

        //        await file.CopyToAsync(fileStreem);



        //        var input = Path.Combine(dir, "file.mp4");

        //        var output = Path.Combine(dir, "converted.mp4");


        //        FFmpeg.ExecutablesPath = Path.Combine(dir, "ffmpeg");


        //        var startSpen = TimeSpan.FromSeconds(start);
        //        var endSpan = TimeSpan.FromSeconds(end);

        //        var duration = endSpan - startSpen;



        //        var info = await MediaInfo.Get(input);

        //        var videoStream = info.VideoStreams.First()
        //            .SetCodec(VideoCodec.H264).SetSize(VideoSize.Hd480)
        //            .Split(startSpen,endSpan);   // Splict fucntion is use to Split the Vide with Specific timeSpan

        //        await Conversion.New()
        //            .AddStream(videoStream).SetOutput(output).Start();

        //    }

        //    //     await ConvertVideo();
        //    return RedirectToAction("Index");
        //}




        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit =409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<IActionResult> Index(IFormFile file)
        {

            using (var fileStreem = new FileStream(Path.Combine(dir, "file.mp4"), FileMode.Create, FileAccess.Write))
            {

                await file.CopyToAsync(fileStreem);

                var input = Path.Combine(dir, "file.mp4");

                var output = Path.Combine(dir, "converted.mp4");


                FFmpeg.ExecutablesPath = Path.Combine(dir, "ffmpeg");


                var info = await MediaInfo.Get(input);

                var videoStream = info.VideoStreams.First()
                    .SetCodec(VideoCodec.H264).SetSize(VideoSize.Hd480);

                await Conversion.New()
                    .AddStream(videoStream).SetOutput(output).Start();

            }

            //     await ConvertVideo();
            return RedirectToAction("Index");
        }



        public async Task<bool> ConvertVideo()
        {

            try
            {
          
                var input = Path.Combine(dir, "file.mp4");

                var output = Path.Combine(dir, "converted.mp4");


                FFmpeg.ExecutablesPath = Path.Combine(dir, "ffmpeg");


                var info = await MediaInfo.Get(input);

                var videoStream = info.VideoStreams.First()
                    .SetCodec(VideoCodec.H264).SetSize(VideoSize.Hd480);

                await Conversion.New()
                    .AddStream(videoStream).SetOutput(output).Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return true;
        }
    }
}