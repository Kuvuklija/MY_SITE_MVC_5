using System;
using System.Web.Mvc;
using System.Text;
using MyFirstSite.WebUI.Models;

namespace MyFirstSite.WebUI.HtmlHelpers{
    //содержит расширяющий метод PagingHelper (расширяет системный HtmlHelper), генерирующий разметку для ссылок на страницы сайта
    public static class PagingHelper {
        public static MvcHtmlString PageLinks(this HtmlHelper html, //этот парамет при вызове опускаем
            PagingInfo pagingInfo,
            Func<int,string> pageUrl){
            StringBuilder result = new StringBuilder();
            for(int i = 1; i <= pagingInfo.TotalPages; i++){
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i)); //вызов метода делегата, описанного в UnitTest
                tag.InnerHtml = i.ToString(); 
                tag.AddCssClass("btn btn-default");
                if (i == pagingInfo.CurrentPage){
                    tag.AddCssClass("btn-primary");
                    tag.AddCssClass("selected"); //выделяем номер текущей страницы
                }
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}