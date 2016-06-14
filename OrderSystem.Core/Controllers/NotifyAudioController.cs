using OrderSystem.Core.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSystem.Core.Controllers
{
    public class NotifyAudioController : Controller
    {
        // GET: NotifyAudio
        public FileContentResult Index(string msg)
        {
            Text2AudHelper t2a = new Text2AudHelper();
            return File(t2a.GetAudio(msg).ToArray(), "audio/mp3");
        }
    }
}