using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace InfoAppMvc.Controllers
{
    public class HomeController : Controller
    {
        const string path = @"C:\Users\sahar\source\repos\InfoAppMvc\Saved.txt";
        private List<string> Inputs;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController()
        {
            this.Inputs = new List<string>();
        }

        [AcceptVerbs("GET")]
        public ActionResult Index(bool? wasRedirected)
        {
            try
            {
                if (wasRedirected is true)
                {
                    ViewBag.name = "Your informations have been deleted successfully!";
                }
                if (System.IO.File.Exists(path))
                {
                    if (System.IO.File.ReadAllText(path).Length == 0)
                    {
                        log.Info("File was already there but it was empty!");
                        return View();
                    }
                    log.Info("File was already there but it was not empty!");
                    return RedirectToAction("Continue", "Home");
                    

                }
                else
                {
                    log.Info("File was not there!");
                    return View();

                }
            }
            catch (Exception ex)
            {
                log.Error("Error trying to do something", ex);
                return View();
            }

        }

        [AcceptVerbs("POST")]
        public ActionResult Index(string MyFirstName, string MyLastName)
        {
            try
            {
                if (MyFirstName.Trim().Length > 0 && MyLastName.Trim().Length > 0)
                {
                    Inputs.Add(MyFirstName);
                    Inputs.Add(MyLastName);
                    if (!System.IO.File.Exists(path))
                    {
                        System.IO.File.CreateText(path).Close();
                        System.IO.File.WriteAllLines(path, Inputs);
                        log.Info("Created the file and wrote the informations!");
                        return RedirectToAction("Continue", "Home");
                    }
                    else
                    {
                        log.Info("Wrote the informations on the file for the first time!");
                        System.IO.File.WriteAllLines(path, Inputs);
                        return RedirectToAction("Continue", "Home");
                    }
                }
                
                return View();
            }
            catch (Exception ex)
            {
                log.Error("Error trying to do something", ex);
                return View();
            }

        }
              
        [AcceptVerbs("POST")]
        public ActionResult Continue(string MyAddress,string MyEmail)
        {
            try
            {
                string oldstr;
                Inputs.Add(MyAddress);
                Inputs.Add(MyEmail);
                if (MyAddress.Trim().Length > 0 && MyEmail.Trim().Length > 0)
                {
                    string[] lines = System.IO.File.ReadAllLines(path);
                    if (lines.Length == 4 && lines[2] != null && lines[3] != null)
                    {

                        oldstr = lines[2];
                        lines[2] = lines[2].Replace(oldstr, Inputs[0]);
                        oldstr = lines[3];
                        lines[3] = lines[3].Replace(oldstr, Inputs[1]);
                        log.Info("Edited Address and Email!");
                        ViewBag.message = "Your informations have been edited successfully!";
                        System.IO.File.WriteAllLines(path, lines);
                        log.Info("Showed the information");
                        var prevInformation = System.IO.File.ReadAllLines(path);
                        ViewBag.Name = String.Format("Your name is: {0}, your last name is: {1}, your address is: {2}, your email is: {3}", prevInformation[0], prevInformation[1], prevInformation[2], prevInformation[3]);
                        return View();
                    }
                    else
                    {
                        log.Info("Wrote the informations on the file for the second time!");
                        ViewBag.message = "Your informations have been saved successfully!";
                        System.IO.File.AppendAllLines(path, Inputs);
                        log.Info("Showed the information!");
                        var prevInformation = System.IO.File.ReadAllLines(path);
                        ViewBag.Name = String.Format("Your name is: {0}, your last name is: {1},your address is: {2}, your email is: {3}", prevInformation[0], prevInformation[1], prevInformation[2], prevInformation[3]);
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                log.Error("Error trying to do something", ex);
                return View();
            }
        }

        [AcceptVerbs("GET")]
        public ActionResult Continue()
        {
            try
            {
                log.Info("Showed the information!");
                var prevInformation = System.IO.File.ReadAllLines(path);
                ViewBag.Name = String.Format("Your name is: {0}, your last name is: {1}", prevInformation[0], prevInformation[1]);
                return View();
            }
            catch (Exception ex)
            {
                log.Error("Error trying to do something", ex);
                return View();
            }
        }
        public ActionResult ClearAllData()
        {
            try
            {
                System.IO.File.WriteAllText(path, string.Empty);
                log.Info("Cleared all the informations and retured back to the first page!");
                return RedirectToAction("Index", "Home", new { wasRedirected = true });
            }
            catch (Exception ex)
            {
                log.Error("Error trying to do something", ex);
                return View();
            }
        }

        [AcceptVerbs("GET")]
        public ActionResult Edit()
        {
            var prevInformation = System.IO.File.ReadAllLines(path);
            ViewBag.FirstName = prevInformation[0];
            ViewBag.LastName = prevInformation[1];
            ViewBag.Address = prevInformation[2];
            ViewBag.Email = prevInformation[3];
            return View();
        }
        [AcceptVerbs("POST")]
        public ActionResult Edit(string MyFirstName,string MyLastName,string MyAddress,string MyEmail)
        {
            System.IO.File.WriteAllText(path, string.Empty);
            Inputs.Add(MyFirstName);
            Inputs.Add(MyLastName);
            Inputs.Add(MyAddress);
            Inputs.Add(MyEmail);
            System.IO.File.WriteAllLines(path,Inputs);

            ViewBag.FirstName = MyFirstName;
            ViewBag.LastName = MyLastName;
            ViewBag.Address = MyAddress;
            ViewBag.Email = MyEmail;

            return View();
        }
    }
}