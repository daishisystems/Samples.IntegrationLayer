using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Samples.IntegrationLayer.Models;

namespace Samples.IntegrationLayer.Controllers
{
    public class ClaimsController : Controller
    {
        // number of messages to be sent to the queue
        private const int numOfMessages = 3;

        // connection string to your Service Bus namespace
        private static readonly string connectionString =
            "Endpoint=sb://gmst-message-backbone.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=GChRDAE1QIgdQmBMAZw2dHkJgk+Mg7AhDK363dxBhj4=";

        // name of your Service Bus queue
        private static readonly string queueName = "Default";

        // the client that owns the connection and can be used to create senders and receivers
        private static ServiceBusClient client;

        // the sender used to publish messages to the queue
        private static ServiceBusSender sender;
        private readonly ClaimDBContext db = new();

        // GET: Claims
        public ActionResult Index()
        {
            return View(db.Claims.ToList());
        }

        // GET: Claims/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var claim = db.Claims.Find(id);
            if (claim == null) return HttpNotFound();
            return View(claim);
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ClaimId,MemberId,IssuedDate,Description")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                claim.ClaimId = Guid.NewGuid();
                db.Claims.Add(claim);
                await db.SaveChangesAsync();

                // The Service Bus client types are safe to cache and use as a singleton for the lifetime
                // of the application, which is best practice when messages are being published or read
                // regularly.
                //
                // Create the clients that we'll use for sending and processing messages.
                client = new ServiceBusClient(connectionString);
                sender = client.CreateSender(queueName);

                // create a batch 
                using var messageBatch = await sender.CreateMessageBatchAsync();
                // try adding a message to the batch
                var messagePayload = JsonConvert.SerializeObject(claim);
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage(messagePayload)))
                        // if it is too large for the batch
                        throw new Exception($"The message is too large to fit in the batch.");

                try
                {
                    // Use the producer client to send the batch of messages to the Service Bus queue
                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
                }
                finally
                {
                    // Calling DisposeAsync on client types is required to ensure that network
                    // resources and other unmanaged objects are properly cleaned up.
                    await sender.DisposeAsync();
                    await client.DisposeAsync();
                }

                return RedirectToAction("Index");
            }

            return View(claim);
        }

        // GET: Claims/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var claim = db.Claims.Find(id);
            if (claim == null) return HttpNotFound();
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimId,MemberId,IssuedDate,Description")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claim);
        }

        // GET: Claims/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var claim = db.Claims.Find(id);
            if (claim == null) return HttpNotFound();
            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}