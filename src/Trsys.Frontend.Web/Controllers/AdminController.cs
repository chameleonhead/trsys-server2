using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.Frontend.Application.Admin.ClientDetails;
using Trsys.Frontend.Application.Admin.Clients;
using Trsys.Frontend.Application.Admin.Dashboard;
using Trsys.Frontend.Application.Admin.Order;
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
                LinkUri = Url.Action(nameof(Clients), new { connectedOnly = true, }),
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
                        Title = "通貨ペア",
                        Value = response.CurrentOrderText == null ? "" : response.CurrentOrderText.Symbol,
                    },
                    new()
                    {
                        Title = "ポジション",
                        Value = response.CurrentOrderText == null ? "" : response.CurrentOrderText.OrderType == "BUY" ? "買い" : "売り",
                    },
                }
            });
            var totalProfit = response.Trades.Sum(t => t.TotalProfit);
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
                        Value = $"{response.Trades.Count} 件",
                    },
                    new()
                    {
                        Title = "損益",
                        Value = $"{totalProfit:#,#0} JPY",
                        ValueClass = totalProfit == 0 ? null : totalProfit > 0 ? "text-success" : "text-danger",
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
        public async Task<ActionResult> Clients([FromQuery] List<string> keyType, [FromQuery] bool? connectedOnly, [FromQuery] List<bool> isActive, [FromQuery] string text)
        {
            var request = new ClientsSearchRequest()
            {
                KeyType = keyType ?? new(),
                ConnectedOnly = connectedOnly,
                IsActive = isActive ?? new(),
                Text = text,
            };
            var response = await mediator.Send(request);
            var vm = new ClientsViewModel()
            {
                SearchConditions = request,
                Clients = response.Clients
            };
            return View(vm);
        }

        [HttpGet("clients/add")]
        public ActionResult ClientAdd()
        {
            return View();
        }

        [HttpPost("clients/add")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientAddExecute([FromForm] ClientAddViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(nameof(ClientAdd), vm);
                }

                await mediator.Send(vm.Request);

                TempData["Message"] = "登録が完了しました。";
                return RedirectToAction(nameof(Clients));
            }
            catch
            {
                return View(nameof(ClientAdd), vm);
            }
        }

        [HttpGet("clients/{id}")]
        public async Task<ActionResult> ClientDetails(string id, [FromQuery] int? year, [FromQuery] int? month)
        {
            var now = DateTimeOffset.UtcNow;
            var secretKeyRequest = new ClientDetailsSecretKeyRequest()
            {
                SecretKeyId = id,
            };
            var secretKeyResponse = await mediator.Send(secretKeyRequest);
            var tradeHistoryRequest = new ClientDetailsSubscriberTradeHistorySearchRequest()
            {
                SecretKeyId = id,
                Year = year ?? now.Year,
                Month = month ?? now.Month
            };
            var tradeHistoryResponse = await mediator.Send(tradeHistoryRequest);
            var vm = new ClientDetailsViewModel()
            {
                Request = secretKeyRequest,
                SecretKey = secretKeyResponse.SecretKey,
                TradeHistorySearchResult = tradeHistoryResponse.TradeHistorySearchResult,
                YearMonthSelection = tradeHistoryResponse.YearMonthSelection,
            };
            return View(vm);
        }

        [HttpGet("clients/{id}/edit")]
        public async Task<ActionResult> ClientEdit(string id)
        {
            var secretKeyRequest = new ClientDetailsSecretKeyRequest()
            {
                SecretKeyId = id,
            };
            var secretKeyResponse = await mediator.Send(secretKeyRequest);
            var vm = new ClientEditViewModel()
            {
                SecretKey = secretKeyResponse.SecretKey,
                Request = new()
                {
                    SecretKeyId = id,
                    Description = secretKeyResponse.SecretKey.Desctiption,
                    IsActive = secretKeyResponse.SecretKey.IsActive,
                },
            };
            return View(vm);
        }

        [HttpPost("clients/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientEditExecute(string id, [FromForm] ClientEditViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var secretKeyRequest = new ClientDetailsSecretKeyRequest()
                    {
                        SecretKeyId = id,
                    };
                    var secretKeyResponse = await mediator.Send(secretKeyRequest);
                    vm.SecretKey = secretKeyResponse.SecretKey;
                    return View(nameof(ClientEdit), vm);
                }

                await mediator.Send(vm.Request);

                TempData["Message"] = "登録が完了しました。";
                return RedirectToAction(nameof(ClientDetails), new { id });
            }
            catch
            {
                return View(nameof(ClientEdit), vm);
            }
        }

        [HttpGet("order")]
        public async Task<ActionResult> Order()
        {
            var response = await mediator.Send(new OrderGetCurrentOrderRequest());
            var vm = new OrderViewModel()
            {
                OpenRequest = new OrderOpenCurrentOrderRequest(),
                CloseRequest = new OrderCloseCurrentOrderRequest(),
                SymbolSelection = new()
                {
                    "USDJPY",
                    "EURJPY",
                    "GBPJPY",
                    "AUDJPY",
                    "NZDJPY",
                    "EURUSD",
                    "AUDUSD",
                    "GBPUSD",
                    "NZDUSD",
                    "EURGBP",
                    "AUDCAD",
                    "AUDCHF",
                    "AUDNZD",
                    "CADJPY",
                    "CHFJPY",
                    "EURAUD",
                    "EURCAD",
                    "EURCHF",
                    "GBPAUD",
                    "GBPCHF",
                    "USDCHF",
                    "USDCAD",
                    "TRYJPY",
                    "ZARJPY",
                    "GBPCAD",
                    "GBPNZD",
                    "USDTRY",
                    "USDZAR",
                    "CADCHF",
                    "EURNZD",
                    "NZDCAD",
                    "NZDCHF",
                    "MXNJPY",
                    "USDMXN",
                    "USDSGD",
                    "USDCNH",
                    "USDDKK",
                    "USDNOK",
                    "USDSEK",
                    "CNHJPY",
                },
                SubscriberStates = response.SubscriberStates,
                CurrentOrder = response.CurrentOrder,
            };
            return View(vm);
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
