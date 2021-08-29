using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace CarRent.Helpers
{
    [HtmlTargetElement("warning")]
    public class WarningTagHelper : TagHelper
    {
        public string Content { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "Warning";
            output.TagMode = TagMode.StartTagAndEndTag;

            var div = new TagBuilder("div");
            div.Attributes["class"] = "alert alert-dismissible alert-warning";

            var button = new TagBuilder("button");
            button.Attributes["class"] = "close";
            button.Attributes["data-dismiss"] = "alert";
            button.InnerHtml.Append("X");

            var heading = new TagBuilder("h4");
            heading.Attributes["class"] = "alert-heading";
            heading.InnerHtml.Append("Warning");

            var paragraph = new TagBuilder("p");
            paragraph.Attributes["class"] = "mb-0";
            paragraph.InnerHtml.Append(this.Content);

            div.InnerHtml.AppendHtml(button);
            div.InnerHtml.AppendHtml(heading);
            div.InnerHtml.AppendHtml(paragraph);

            var sb = new StringBuilder();
            sb.AppendFormat(div.ToString());

            output.PreContent.SetHtmlContent(div);
        }
    }
}