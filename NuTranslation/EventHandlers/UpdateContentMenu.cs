using System.Linq;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace NuTranslation.EventHandlers
{
    public class UpdateContentMenu : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Umbraco.Web.Trees.ContentTreeController.MenuRendering += ContentTreeController_MenuRendering;
        }

        private void ContentTreeController_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            //If it's the content tree
            if (sender.TreeAlias == "content")
            {
                //If the current user is allowed in the translation section
                if(sender.Services.SectionService.GetAllowedSections(sender.Security.GetUserId())
                    .Count(s => s.Alias.InvariantEquals("translation")) > 0)
                {
                    //Get old translation item and remove it
                    var oldMenuItem = e.Menu.Items.SingleOrDefault(m => m.Alias == "sendToTranslate");
                    if (oldMenuItem != null)
                    {
                        e.Menu.Items.Remove(oldMenuItem);
                    }

                    //Adds 2 new items at the top of the list
                    var sendToTranslationItem = new MenuItem("sendToNuTranslate", "Send to Translatiion");
                    sendToTranslationItem.LaunchDialogView("/App_Plugins/NuTranslation/views/sendforTranslation.html", "Send to Translation");
                    sendToTranslationItem.Icon = "flag-alt";
                    e.Menu.Items.Insert(0,sendToTranslationItem);

                    var translationHistoryItem = new MenuItem("translationHistory", "Translation History");
                    translationHistoryItem.LaunchDialogView("/App_Plugins/NuTranslation/views/translationHistory.html", "Translation History");
                    translationHistoryItem.Icon = "list";
                    e.Menu.Items.Insert(1, translationHistoryItem);

                    //Add separator before next menu item
                    e.Menu.Items[2].SeperatorBefore = true;
                }
            }
        }
    }
}
