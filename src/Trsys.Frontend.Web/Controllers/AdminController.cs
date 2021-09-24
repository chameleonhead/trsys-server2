using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trsys.Frontend.Web.Models.Admin;

namespace Trsys.Frontend.Web.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("")]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet("dashboard")]
        public ActionResult Dashboard()
        {
            var vm = new DashboardViewModel();
            vm.DashboardItems.Add(new DashboardItemViewModel()
            {
                Header = "接続中のEA",
                LinkText = "一覧",
                LinkTitle = "シークレットキー一覧",
                LinkUri = Url.Action(nameof(Clients)),
                Lines = new()
                {
                    new()
                    {
                        Title = "Publisher",
                        Value = "1 台",
                    },
                    new()
                    {
                        Title = "Subscriber",
                        Value = "10 台",
                    },
                }
            });
            vm.DashboardItems.Add(new DashboardItemViewModel()
            {
                Header = "コピートレード",
                LinkText = "詳細",
                LinkTitle = "コピートレード",
                LinkUri = Url.Action(nameof(Order)),
                Lines = new()
                {
                    new()
                    {
                        Title = "注文状況",
                        Value = "USDJPY/買い",
                        ValueClass = "text-danger",
                    },
                    new()
                    {
                        Title = "更新時刻",
                        Value = "2021-09-24 12:01:00",
                    },
                }
            });
            vm.DashboardItems.Add(new DashboardItemViewModel()
            {
                Header = "履歴",
                LinkText = "一覧",
                LinkTitle = "注文履歴",
                LinkUri = Url.Action(nameof(History)),
                Lines = new()
                {
                    new()
                    {
                        Title = "取引数",
                        Value = "3 件",
                    },
                    new()
                    {
                        Title = "損益",
                        Value = "- 320,300 JPY",
                        ValueClass = "text-danger",
                    },
                }
            });

            vm.Messages.Add(new DashboardMessageViewModel()
            {
                Message = "承認待ちのシークレットキーがあります。",
                Uri = Url.Action(nameof(ClientDetails), new { id = "1" })
            });
            return View(vm);
        }

        [HttpGet("clients")]
        public ActionResult Clients()
        {
            return View();
        }

        [HttpGet("clients/add")]
        public ActionResult ClientAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientAdd(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Clients));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("clients/{id}")]
        public ActionResult ClientDetails(string id)
        {
            return View();
        }

        [HttpGet("clients/{id}/edit")]
        public ActionResult ClientEdit(string id)
        {
            return View();
        }

        [HttpGet("order")]
        public ActionResult Order()
        {
            return View();
        }

        [HttpGet("history")]
        public ActionResult History()
        {
            return View();
        }

        [HttpGet("history/{id}")]
        public ActionResult HistoryDetails(string id)
        {
            return View();
        }
    }
}
