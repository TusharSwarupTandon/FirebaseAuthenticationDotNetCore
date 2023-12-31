using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace FirebaseAuthenticationDotNetCore.Website.Infrastructure.Extensions;

/// <summary>
/// Provides some helper methods for working with sections.
/// Credit: <see href="http://blogs.msdn.com/b/marcinon/archive/2010/12/15/razor-nested-layouts-and-redefined-sections.aspx"/>
/// </summary>
public static class SectionExtensions
{
    private static readonly object Obj = new object();

    public static async Task<HtmlString> RenderSectionAsync(this RazorPage page, string sectionName,
        Func<object, HelperResult> defaultContent)
    {
        if (page.IsSectionDefined(sectionName))
        {
            return await page.RenderSectionAsync(sectionName);
        } 

        var html= defaultContent.Invoke(Obj); 
        await html.WriteAction(page.ViewContext.Writer); 
        return default;
    }

    /// <summary>
    /// This allows an intermediary view to conditionally redefine a section, only if the section was defined in a
    /// child view. If no section was defined in the child view, this method will NOT redefine the section in the 
    /// intermediary view, which allows the parent view to test if the section was defined or not and provide default
    /// content. This is the only way I know of to have a parent view define default content for optional sections
    /// when an intermediary view is utilized.
    /// </summary>
    /// <param name="page">The page this method is being used from</param>
    /// <param name="sectionName">The section name</param>
    /// <returns>Redefines the section if defined in the child view, otherwise does not redefine the section</returns>
    public static HtmlString RedefineSection(this RazorPage page, string sectionName)
    {
        return RedefineSection(page, sectionName, defaultContent: null);
    }

    public static HtmlString RedefineSection(this RazorPage page, string sectionName,
                                               Func<object, HelperResult> defaultContent)
    {
        if (page.IsSectionDefined(sectionName))
        {
            page.DefineSection(sectionName, () => page.RenderSectionAsync(sectionName));
        }

        return new HtmlString(string.Empty);
    }
}