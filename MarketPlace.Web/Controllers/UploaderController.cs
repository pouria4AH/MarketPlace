using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MarketPlace.Web.Controllers
{
    public class UploaderController : SiteBaseController
    {
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncName, string CKEditor, string longCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر وارد کنید";
                var notImage = JsonConvert.DeserializeObject("{'uploaded' : 0, 'error' : {'message' : \" " + notImageMessage + " \" }}");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            upload.AddImageToServer(fileName,PathExtension.UploadeImageServer,null,null);
            return Json(new
            {
                uploade = true,
                url = $"{PathExtension.UploadeImage}{fileName}"
            });
        }
    }
}
