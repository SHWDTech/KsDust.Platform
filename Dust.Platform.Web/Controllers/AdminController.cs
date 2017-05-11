using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Account;
using Dust.Platform.Web.Models.Admin;
using Dust.Platform.Web.Models.Table;
using Microsoft.Ajax.Utilities;

namespace Dust.Platform.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public AdminController()
        {
            _ctx = new KsDustDbContext();
        }

        // GET: Admin
        public ActionResult Message()
        {
            return View();
        }

        public ActionResult MessageTable(MessageTablePost post)
        {
            var userId = ((DustPrincipal)User).Id;
            var clientNotices = _ctx.UserClientNotices.Where(n => n.User.ToString() == userId && n.NoticeClientType == NoticeClientType.WebApp);
            if (post.Status != null)
            {
                clientNotices = clientNotices.Where(c => c.IsReaded == post.Status.Value);
            }

            var query = (from clientNotice in clientNotices
                from notice in _ctx.Notices
                where clientNotice.Notice == notice.Id
                select new
                {
                    notice.Id,
                    NoticeTitle = notice.Title,
                    Time = notice.NoticeDateTime,
                    Type = notice.NoticeType,
                    NoticeContent = notice.Content,
                    clientNotice.IsReaded
                });

            if (post.Type != null)
            {
                query = query.Where(n => n.Type == post.Type.Value);
            }

            var total = query.Count();
            var rows = query.OrderByDescending(obj => obj.Time).Skip(post.offset).Take(post.limit).ToList().Select(n => new
            {
                n.Id,
                n.NoticeTitle,
                NoticeTime = $"{n.Time:yyyy-MM-dd HH:mm:ss}",
                NoticeType = GetNoticeTypeName(n.Type),
                n.NoticeContent,
                n.IsReaded
            });

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NoticeReaded(NoticeClientType type, long noticeId)
        {
            var userId = Guid.Parse(((DustPrincipal)User).Id);
            var clientNotice =
                _ctx.UserClientNotices.FirstOrDefault(u => u.User == userId && u.Notice == noticeId &&
                                                           u.NoticeClientType == type);
            if (clientNotice != null)
            {
                clientNotice.IsReaded = true;
            }

            _ctx.SaveChanges();

            return null;
        }

        public ActionResult DeleteNotice(NoticeClientType type, long[] notices)
        {
            if (notices == null) return null;
            var userId = Guid.Parse(((DustPrincipal)User).Id);
            var clientNotices =
                _ctx.UserClientNotices.Where(n => n.User == userId && n.NoticeClientType == type &&
                                                  notices.Contains(n.Notice));
            _ctx.UserClientNotices.RemoveRange(clientNotices);
            _ctx.SaveChanges();

            return null;
        }

        private static string GetNoticeTypeName(NoticeType noticeType)
        {
            var memberInfo = typeof(NoticeType).GetMember(noticeType.ToString())
                .FirstOrDefault();

            if (memberInfo == null) return null;
            var attribute = (DisplayAttribute)
                memberInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
                    .FirstOrDefault();
            return attribute?.Name;
        }

        public ActionResult GetObjectTargets()
        {
            var obj = new
            {
                dis = _ctx.Districts.Where(item => item.Id != Guid.Empty).Select(d => new
                {
                    id = d.Id,
                    text = d.Name
                }).ToList(),
                ent = _ctx.Enterprises.Where(item => item.Id != Guid.Empty).Select(e => new
                {
                    id = e.Id,
                    text = e.Name
                }).ToList(),
                prj = _ctx.KsDustProjects.Where(item => item.Id != Guid.Empty).Select(p => new
                {
                    id = p.Id,
                    text = p.Name
                }).ToList(),
                dev = _ctx.KsDustDevices.Where(item => item.Id != Guid.Empty).Select(de => new
                {
                    id = de.Id,
                    text = de.Name
                }).ToList()
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneralComparsionResult(GeneralComparsionResultViewModel model)
        {
            var query = _ctx.AverageMonitorDatas.Where(m => m.Type == model.DataType &&
                                                            m.AverageDateTime > model.StartDateTime &&
                                                            m.AverageDateTime < model.EndDateTime &&
                                                            model.TargetObjects.Contains(m.TargetId)).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}