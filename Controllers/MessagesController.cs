using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNest.Data;
using TradeNest.Models;

namespace TradeNest.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("userId");

            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int id))
            {
                return id;
            }

            return 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatHistory(int withUserId)
        {
            int currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var messages = await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == withUserId) ||
                            (m.SenderId == withUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    m.Id,
                    m.Content,
                    m.SentAt,
                    IsMe = m.SenderId == currentUserId
                })
                .ToListAsync();

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int receiverId, string content)
        {
            int currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();
            if (string.IsNullOrWhiteSpace(content)) return BadRequest("Wiadomość nie może być pusta.");

            var message = new Message
            {
                SenderId = currentUserId,
                ReceiverId = receiverId,
                Content = content.Trim(),
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Json(new { success = true, content = message.Content, sentAt = message.SentAt });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int currentUserId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var allMessages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            var chatList = allMessages
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => {
                    var lastMsg = g.First();
                    var interlocutor = lastMsg.SenderId == currentUserId ? lastMsg.Receiver : lastMsg.Sender;
                    return new ChatConversationViewModel
                    {
                        UserId = interlocutor.Id,
                        UserLogin = interlocutor.Login,
                        LastMessage = lastMsg.Content,
                        SentAt = lastMsg.SentAt
                    };
                })
                .ToList();

            return View(chatList);
        }

        public class ChatConversationViewModel
        {
            public int UserId { get; set; }
            public string UserLogin { get; set; } = string.Empty;
            public string LastMessage { get; set; } = string.Empty;
            public DateTime SentAt { get; set; }
        }
    }
}