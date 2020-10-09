using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace FischBot.Services
{
    public class UsFlagHalfMastService
    {
        private readonly string starsAndStripesDailyBaseUrl = "https://starsandstripesdaily.org/";
        private readonly string pageTitleCssSelector = "h1.page-title";
        private readonly string titleCssSelector = "h1.entry-title";
        private readonly string descriptionCssSelector = "div.entry-content";

        private readonly HtmlWeb _htmlWeb;

        public UsFlagHalfMastService(HtmlWeb htmlWeb)
        {
            _htmlWeb = htmlWeb;
        }

        /// <summary>
        /// Returns half mast info about the US flag for the provided date from starsandstripesdaily.org
        /// </summary>
        /// <param name="date">Date to get half mast info for.</param>
        /// <returns>Half mast info about the US flag for the provided date.</returns>
        public UsFlagHalfMastInfo GetUsFlagHalfMastInfoAsync(DateTime date)
        {
            var formattedDate = date.ToString("MMMM-d-yyyy");
            var url = $"{starsAndStripesDailyBaseUrl}half-staff-alert-{formattedDate}/";
            var htmlDocument = _htmlWeb.Load(url);

            bool flagIsHalfMast;
            var title = string.Empty;
            var description = string.Empty;


            flagIsHalfMast = IsHtmlDocumentFlagAtHalfMastAsync(htmlDocument);

            if (flagIsHalfMast)
            {
                title = htmlDocument.DocumentNode.CssSelect(titleCssSelector).Single().InnerText;
                description = htmlDocument.DocumentNode.CssSelect(descriptionCssSelector).Single().InnerText;
            }

            return new UsFlagHalfMastInfo()
            {
                Title = title,
                Description = description,
                FlagIsHalfMast = flagIsHalfMast,
                ReadMoreUrl = url
            };
        }

        private bool IsHtmlDocumentFlagAtHalfMastAsync(HtmlDocument htmlDocument)
        {
            var pageTitle = htmlDocument
                .DocumentNode
                .CssSelect(pageTitleCssSelector)
                .FirstOrDefault();

            return pageTitle == null || !pageTitle.InnerText.StartsWith("404:");
        }
    }
}