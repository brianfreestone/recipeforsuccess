using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace RecipeForSuccess_mvc
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.5.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include("~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootswatch/litera/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css"));




            //bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include("~/Scripts/jquery-3.5.1.js", "~/Scripts/umd/popper.js", "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Styles/bootstrap").Include("~/Content/bootstrap.min.css"));
            //bundles.Add(new StyleBundle("~/Styles/site").Include("~/Content/site.css"));
            //BundleTable.EnableOptimizations = true;

        }

    }
}