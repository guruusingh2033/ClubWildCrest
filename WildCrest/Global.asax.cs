using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WildCrest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (ClubWildCrestEntities entities = new ClubWildCrestEntities())
                        {
                            var user = entities.tbl_Profile.SingleOrDefault(u => u.Email == username);

                            roles = user.UserType.ToString();

                            if (user.UserType == 2)
                            {
                                var setting = entities.tbl_PermissionsToStaff.SingleOrDefault(ss => ss.UserID == user.ID);
                                if (setting != null)
                                {
                                    HttpCookie httpCookie = new HttpCookie("PageSetting");
                                    httpCookie["MemberPermission"] = setting.MemberPermission.ToString();
                                    httpCookie["MembershipPlanPermission"] = setting.MembershipPlanPermission.ToString();
                                    httpCookie["EventsPermission"] = setting.EventsPermission.ToString();
                                    httpCookie["VendorsPermission"] = setting.VendorsPermission.ToString();
                                    httpCookie["MenuItemsPermission"] = setting.MenuItemsPermission.ToString();
                                    httpCookie["FoodBillingEditPermission"] = setting.FoodBillingEditPermission.ToString();
                                    Response.Cookies.Add(httpCookie);
                                }
                            }
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception ex)
                    {
                        //somehting went wrong
                    }
                }
            }
        }
    }
}