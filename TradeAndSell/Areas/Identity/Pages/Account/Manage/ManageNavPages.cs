using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradeAndSell.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string MyCard => "MyCard";

        public static string MyPosts => "MyPosts";

        public static string MyOrders => "MyOrders";

        public static string MyWishList => "MyWishList";

        public static string MyTradeRequests => "MyTradeRequests";

        public static string MyMessages => "MyMessages";

        public static string DownloadPersonalData => "DownloadPersonalData";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string MyCardNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyCard);

        public static string MyPostsNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyPosts);

        public static string MyOrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyOrders);

        public static string MyWishListNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyWishList);

        public static string MyTradeRequestsNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyTradeRequests);
        
        public static string MyMessagesNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyMessages);

        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
