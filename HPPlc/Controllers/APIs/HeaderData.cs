using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Controllers.APIs
{
	public class HeaderData
	{
		public MediaProp PLCLogo { get; set; }
		public MediaProp HpLogo { get; set; }
		//public LinkProp AboutUs { get; set; }
		//public LinkProp Package { get; set; }
		//public LinkProp SignIn { get; set; }
		//public LinkProp SignOut { get; set; }
		public List<LinkProp> HeadMenus { get; set; }
		public MenusWithTitle contactmenus { get; set; }
		public List<MenusWithTitle> PostLoggedInMenus { get; set; }
		public List<DataNavigation> dataNavigations { get; set; }
		//public List<Menus> Menus { get; set; }
	}

	public class DataNavigation
	{
		public string Title { get; set; }
		public List<FilterationType> filterTitle { get; set; }
		public List<NavigationData> filterData { get; set; }

	}

	public class FilterationType
	{
		public string FilterTitle { get; set; }
	}

	public class NavigationData
	{
		public string Title { get; set; }
		public string Navigation { get; set; }
	}

	public class Menus
	{
		public LinkProp RootNavigation { get; set; }
		public LinkProp SubRootNavigation { get; set; }
		public List<ClassMenu> ClassAndSubjects { get; set; }
		public List<LinkProp> SubjectNavigation { get; set; }

		public LinkProp AllClassNavigation { get; set; }
		public LinkProp AllSubjectNavigation { get; set; }
		public string ClassName { get; set; } = "";
		public string MenuMode { get; set; } = "";

	}

	public class ClassMenu
	{
		public LinkProp ClassDtls { get; set; }
		public List<LinkProp> SubjectsMenu { get; set; }
	}

	public class MenusWithTitle
	{
		public string title { get; set; }
		public string Navigation { get; set; }
		public string IconUrl { get; set; }
	}
	public class SubjectsMenu
	{
		public LinkProp SubjectDtls { get; set; }
	}
}