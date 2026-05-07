using ClosedXML.Excel;
using Delwings.Models.Basic;

namespace Delwings.Services
{
    public class FileMakerService
    {
        private readonly OrdersService _ordersSerice;
        private readonly AccountService _accountService;
        private readonly PlaceService _placesService;

        public FileMakerService(AccountService accountService, OrdersService ordersService, PlaceService placeService)
        {
            _ordersSerice = ordersService;
            _accountService = accountService;
            _placesService = placeService;
        }

        private IXLWorksheet GenerateSheetByType<T>(IXLWorksheet ordersSheet, List<T> orders)
        {
            var properties = typeof(T).GetProperties();
            int propertiesLength = properties.Length;

            for (int i=0;i < propertiesLength;i++)
            {
                ordersSheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            for (int i=0;i < orders.Count;i++)
            {
                for (int j=0;j < propertiesLength;j++)
                {
                    var value = properties[j].GetValue(orders[i]);
                    ordersSheet.Cell(i + 2, j + 1).Value = XLCellValue.FromObject(value);
                }
            }

            return ordersSheet;
        }

        public async Task<byte[]> GenerateExcelReport()
        {
            List<Order> orders = await _ordersSerice.GetAllOrdersAsync();
            List<Account> accounts = await _accountService.GetAllAccountsAsync();
            List<Place> places = await _placesService.GetAllPlacesAsync();

            using var book = new XLWorkbook();

            var ordersSheet = book.Worksheets.Add("Orders");
            var accountsSheet = book.Worksheets.Add("Accounts");
            var placesSheet = book.Worksheets.Add("Places");

            var filledOrders = GenerateSheetByType<Order>(ordersSheet, orders);
            filledOrders.Columns().AdjustToContents();
            var filledAccounts = GenerateSheetByType<Account>(accountsSheet, accounts);
            filledAccounts.Columns().AdjustToContents();
            var filledPlaces = GenerateSheetByType<Place>(placesSheet, places);
            filledPlaces.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            book.SaveAs(stream);

            var content = stream.ToArray();

            return content;
        }
    }
}
