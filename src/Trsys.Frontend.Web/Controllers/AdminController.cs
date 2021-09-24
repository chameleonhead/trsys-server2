using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Trsys.Frontend.Application.Admin.Clients;
using Trsys.Frontend.Application.Admin.Dashboard;
using Trsys.Frontend.Application.Dtos;
using Trsys.Frontend.Web.Models.Admin;

namespace Trsys.Frontend.Web.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult> Dashboard()
        {
            var response = await mediator.Send(new DashboardSearchRequest());
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
                        Value = $"{response.ConnectedKeys.Where(e => e.KeyType == "Publisher").Count():#,0} 台",
                    },
                    new()
                    {
                        Title = "Subscriber",
                        Value = $"{response.ConnectedKeys.Where(e => e.KeyType == "Subscriber").Count():#,0} 台",
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
        public ActionResult Clients([FromQuery] ClientsSearchRequest request)
        {
            var vm = new ClientsViewModel()
            {
                SearchConditions = request ?? new ClientsSearchRequest(),
                Clients = new()
                {
                    new SecretKeyDto()
                    {
                        Id = "1",
                        Key = "MT4/OANDA Corporation/811631031/2",
                        KeyType = "Publisher",
                        Desctiption = "山根さん",
                        IsActive = false,
                        IsConnected = false,
                    },
                    new SecretKeyDto()
                    {
                        Id = "2",
                        Key = "MT4/OANDA Corporation/811653730/2",
                        KeyType = "Subscriber",
                        Desctiption = "大川さん",
                        IsActive = true,
                        IsConnected = true,
                    },
                }
            };
            return View(vm);
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
