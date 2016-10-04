using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using ServiceBus_SharedSettings;


namespace WebMessageSubmit.Controllers
{
    public class MessageController : Controller
    {


        // GET: Message
        public ActionResult Index()
        {
            return View();
        }
 

        // GET: Message/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        [HttpPost]
     public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string ConnectionString = Settings.ConnectionString;
                string QueueName = Settings.QueueName;
                var client = QueueClient.CreateFromConnectionString
                        (ConnectionString, QueueName);
                var message = new BrokeredMessage();
                message.Label = Request.Form["MessageBody"];
                message.SessionId = "test";
                client.Send(message);
                client.Close();
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Message/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Message/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
