using System;
using System.Linq;
using FischBot.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace FischBot.Api.StarsAndStripesDailyClient
{
    public class StarsAndStripesDailyClient : IStarsAndStripesDailyClient
    {
        private readonly HtmlWeb _htmlWeb;

        private readonly string starsAndStripesDailyBaseUrl = "https://starsandstripesdaily.org/";
        private readonly string pageTitleCssSelector = "h1.page-title";
        private readonly string titleCssSelector = "h1.entry-title";
        private readonly string descriptionCssSelector = "div.entry-content";

        public StarsAndStripesDailyClient(HtmlWeb htmlWeb)
        {
            _htmlWeb = htmlWeb;
        }

        public UsFlagHalfMastInfo GetUsFlagHalfMastInfo(DateTime date)
        {
            var formattedDate = date.ToString("MMMM-d-yyyy");
            var url = $"{starsAndStripesDailyBaseUrl}half-staff-alert-{formattedDate}/";
            var htmlDocument = _htmlWeb.Load(url);

            var flagIsHalfMast = !IsPage404NotFound(htmlDocument);

            var title = flagIsHalfMast ? htmlDocument.DocumentNode.CssSelect(titleCssSelector).Single().InnerText : string.Empty;
            var description = flagIsHalfMast ? htmlDocument.DocumentNode.CssSelect(descriptionCssSelector).Single().InnerText : string.Empty;

            return new UsFlagHalfMastInfo()
            {
                Title = title,
                Description = description,
                FlagIsHalfMast = flagIsHalfMast,
                ArticleUrl = url
            };
        }

        private bool IsPage404NotFound(HtmlDocument htmlDocument)
        {
            var pageTitle = htmlDocument
                .DocumentNode
                .CssSelect(pageTitleCssSelector)
                .FirstOrDefault();

            return pageTitle != null && pageTitle.InnerText.StartsWith("404:");
        }
    }
}