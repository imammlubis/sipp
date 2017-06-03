﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipp.Web.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Index() {
            return View();
        }

        //[HttpPost]
        //public ActionResult Submit(IEnumerable<HttpPostedFileBase> files)
        //{
        //    if (files != null)
        //    {
        //        TempData["UploadedFiles"] = GetFileInfo(files);
        //    }

        //    return RedirectToRoute("Demo", new { section = "upload", example = "result" });
        //}

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, string id)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(fileName);

                    var physicalPath = Path.Combine(Server.MapPath("~/Documents/SkAwal/"), id + ext);

                    // The files are not actually saved in this demo
                    try {

                        file.SaveAs(physicalPath);
                    }
                    catch (Exception) { }
                   
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }
    }
}