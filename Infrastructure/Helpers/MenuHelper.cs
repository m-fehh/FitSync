namespace Infrastructure.Helpers
{
    public static class MenuHelper
    {
        public static List<MenuSection> GetMenuSections()
        {
            return new List<MenuSection>
            {
                new MenuSection
                {
                    Title = Engine.Translate("quickMenu"),
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Title = Engine.Translate("home"), Icon = "fa-solid fa-house-chimney", Url = "/home" },
                    }
                },
                new MenuSection
                {
                    Title = "Conteúdo",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Title = "Posts", Icon = "fa-solid fa-square-pen", Url = "#" },
                        new MenuItem { Title = "Media", Icon = "fa-solid fa-image", Url = "#" },
                        new MenuItem { Title = "Pages", Icon = "fa-solid fa-table-columns", Url = "#" },
                        new MenuItem
                        {
                            Title = "comments",
                            Icon = "fa-solid fa-message",
                            SubItems = new List<MenuItem>
                            {
                                new MenuItem { Title = "Pendentes", Icon = "", Url = "#" },
                                new MenuItem { Title = "Aprovados", Icon = "", Url = "#" },
                            }
                        },
                    }
                }
            };
        }
    }



    public class MenuSection
    {
        public string Title { get; set; }
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();
    }

    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public List<MenuItem> SubItems { get; set; } = new List<MenuItem>();
    }
}
