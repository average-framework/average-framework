using CitizenFX.Core;
using System.Collections.Generic;

namespace Client.Core.UI.Menu
{
    public class Menu : Script
    {
        protected List<MenuContainer> menus = new List<MenuContainer>();
        protected List<string> history = new List<string>();

        public MenuContainer MainMenu { get; private set; }
        public MenuContainer OldMenu { get; private set; }
        public MenuContainer CurrentMenu { get; private set; }
        public bool CanCloseMenu { get; set; } = true;
        public bool IsOpen { get; private set; }

        public delegate void OnMenuChange(MenuContainer oldMenu, MenuContainer currentMenu);
        public delegate void OnMenuClose(MenuContainer currentMenu);
        public event OnMenuChange OnMenuChangeHandler;
        public event OnMenuClose OnMenuCloseHandler;

        public Menu(Main main) : base(main) => RegisterNuiEvents();

        protected virtual void OnMenuChangeReached(MenuContainer oldMenu, MenuContainer currentMenu) => OnMenuChangeHandler?.Invoke(oldMenu, currentMenu);
        protected virtual void OnMenuCloseReached(MenuContainer currentMenu) => OnMenuCloseHandler?.Invoke(currentMenu);

        #region Nui Callback

        CallbackDelegate OnItemClicked(IDictionary<string, object> data, CallbackDelegate result)
        {
            var name = data["name"].ToString();
            var item = CurrentMenu.GetItem(name);

            switch (item)
            {
                case MenuItem menuItem:
                    if (menuItem.TargetContainer != null)
                    {
                        history.Add(CurrentMenu.Name);

                        OpenMenu(menuItem.TargetContainer.Name);
                    }

                    if (menuItem.Action != null) menuItem.Action.DynamicInvoke();
                    break;
                case MenuStatsItem menuItem:
                    break;
                case MenuItemCheckbox menuItem:
                    menuItem.Checked = !menuItem.Checked;

                    if (menuItem.Action != null)
                    {
                        menuItem.Action.Invoke(menuItem.Checked);
                        result(menuItem.Checked);
                    }
                    break;
                case MenuItemSlider<int> menuItem:
                    menuItem.Value = int.Parse(data["value"].ToString());

                    if (menuItem.Action != null) menuItem.Action.Invoke(menuItem.Value);
                    break;
                case MenuItemSlider<float> menuItem:
                    menuItem.Value = float.Parse(data["value"].ToString());

                    if (menuItem.Action != null) menuItem.Action.Invoke(menuItem.Value);
                    break;
                case MenuItemList menuItem:
                    if (menuItem.Action != null)
                    {
                        if (data["operator"].ToString() == "-")
                        {
                            if (menuItem.Index != 0)
                                menuItem.Index--;
                        }
                        else if (data["operator"].ToString() == "+")
                        {
                            if (menuItem.Index != menuItem.Values.Count - 1)
                                menuItem.Index++;
                        }

                        menuItem.Action.Invoke(menuItem.Index, menuItem.Values[menuItem.Index]);
                        result(menuItem.Values[menuItem.Index].Value);
                    }
                    break;
                case MenuTextboxItem menuItem:
                    menuItem.Value = data["value"];

                    if (menuItem.Action != null) menuItem.Action.Invoke(menuItem.Value);
                    break;
                case MenuSliderSelectorItem<int> menuItem:
                    if (data.ContainsKey("value")) menuItem.Value = int.Parse(data["value"].ToString());

                    if (data.ContainsKey("operator"))
                    {
                        var op = data["operator"].ToString();

                        if (op == "-")
                        {
                            if (menuItem.Value <= menuItem.MinValue)
                                menuItem.Value = menuItem.MinValue;
                            else
                                menuItem.Value -= menuItem.Step;
                        }
                        else if (op == "+")
                        {
                            if (menuItem.Value >= menuItem.MaxValue)
                                menuItem.Value = menuItem.MaxValue;
                            else
                                menuItem.Value += menuItem.Step;
                        }

                        result(menuItem.Value);
                    }

                    if (menuItem.Action != null) menuItem.Action.Invoke(menuItem.Value);
                    break;
                case MenuSliderSelectorItem<float> menuItem:
                    if (data.ContainsKey("value")) menuItem.Value = float.Parse(data["value"].ToString());

                    if (data.ContainsKey("operator"))
                    {
                        var op = data["operator"].ToString();

                        if (op == "-")
                        {
                            if (menuItem.Value <= menuItem.MinValue)
                                menuItem.Value = menuItem.MinValue;
                            else
                                menuItem.Value -= menuItem.Step;
                        }
                        else if (op == "+")
                        {
                            if (menuItem.Value >= menuItem.MaxValue)
                                menuItem.Value = menuItem.MaxValue;
                            else
                                menuItem.Value += menuItem.Step;
                        }

                        result(menuItem.Value);
                    }

                    if (menuItem.Action != null) menuItem.Action.Invoke(menuItem.Value);
                    break;
            }

            return result;
        }

        CallbackDelegate OnPrevious(IDictionary<string, object> data, CallbackDelegate result)
        {
            if (IsOpen)
            {
                if (history.Count > 0)
                {
                    var containerIndex = history.Count - 1;
                    var parent = history[containerIndex];

                    OpenMenu(parent);

                    history.RemoveAt(containerIndex);
                }
                else
                {
                    if (CanCloseMenu)
                    {
                        CloseMenu();
                        ClearHistory();
                        NUI.Focus(false, false);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Nui Methods

        void RegisterNuiEvents()
        {
            NUI.RegisterNUICallback(Events.Menu.OnItemClicked, OnItemClicked);
            NUI.RegisterNUICallback(Events.Menu.OnPrevious, OnPrevious);
        }

        public void UpdateRender(MenuContainer menuContainer)
        {
            var items = new List<object>();

            foreach (var item in menuContainer.Items)
            {
                switch (item)
                {
                    case MenuItem menuItem:
                        items.Add(new
                        {
                            type = "menu_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            hasTarget = menuItem.TargetContainer != null ? true : false,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuItemCheckbox menuItem:
                        items.Add(new
                        {
                            type = "menu_checkbox_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            isChecked = menuItem.Checked,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuItemSlider<int> menuItem:
                        items.Add(new
                        {
                            type = "menu_slider_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            min = menuItem.MinValue,
                            max = menuItem.MaxValue,
                            step = menuItem.Step,
                            value = menuItem.Value,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuItemSlider<float> menuItem:
                        items.Add(new
                        {
                            type = "menu_slider_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            min = menuItem.MinValue,
                            max = menuItem.MaxValue,
                            step = menuItem.Step,
                            value = menuItem.Value,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuSliderSelectorItem<int> menuItem:
                        items.Add(new
                        {
                            type = "menu_slider_selector_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            min = menuItem.MinValue,
                            max = menuItem.MaxValue,
                            step = menuItem.Step,
                            value = menuItem.Value,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuSliderSelectorItem<float> menuItem:
                        items.Add(new
                        {
                            type = "menu_slider_selector_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            min = menuItem.MinValue,
                            max = menuItem.MaxValue,
                            step = menuItem.Step,
                            value = menuItem.Value,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuItemList menuItem:
                        items.Add(new
                        {
                            type = "menu_list_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            itemName = menuItem.Values[menuItem.Index].Value,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuTextboxItem menuItem:
                        items.Add(new
                        {
                            type = "menu_textbox_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            placeholder = menuItem.Placeholder,
                            pattern = menuItem.Pattern,
                            minLength = menuItem.MinLength,
                            maxLength = menuItem.MaxLength,
                            value = menuItem.Value,
                            visible = menuItem.Visible
                        });
                        break;
                    case MenuStatsItem menuItem:
                        items.Add(new
                        {
                            type = "menu_stats_item",
                            name = menuItem.Name,
                            text = menuItem.Text,
                            step = menuItem.Step,
                            value = menuItem.Value,
                            visible = menuItem.Visible
                        });
                        break;
                }
            }

            NUI.Execute(new
            {
                request = "menu.updateRender",
                items
            });
        }

        public void OpenMenu(string name)
        {
            if (Exists(name))
            {
                IsOpen = true;

                if (OldMenu != CurrentMenu)
                {
                    OldMenu = CurrentMenu;
                }

                CurrentMenu = GetContainer(name);
                MainMenu = CurrentMenu;

                UpdateRender(CurrentMenu);

                NUI.Execute(new
                {
                    request = "menu.open",
                    name = CurrentMenu.Name,
                    title = CurrentMenu.Title
                });

                OnMenuChangeReached(OldMenu, CurrentMenu);
            }
        }

        public void OpenMenu(MenuContainer menu)
        {
            if (Exists(menu))
            {
                IsOpen = true;

                if (OldMenu != CurrentMenu)
                {
                    OldMenu = CurrentMenu;
                }

                CurrentMenu = menu;
                MainMenu = CurrentMenu;

                UpdateRender(CurrentMenu);

                NUI.Execute(new
                {
                    request = "menu.open",
                    name = CurrentMenu.Name,
                    title = CurrentMenu.Title
                });

                OnMenuChangeReached(OldMenu, CurrentMenu);
            }
        }

        public void CloseMenu()
        {
            IsOpen = false;

            NUI.Execute(new
            {
                request = "menu.close"
            });

            OnMenuCloseReached(CurrentMenu);
        }

        #endregion

        #region Methods

        public void ClearHistory() => history.Clear();
        public bool Exists(MenuContainer menuContainer) => menus.Contains(menuContainer);
        public bool Exists(string menuName) => menus.Exists(x => x.Name == menuName);
        public MenuContainer GetContainer(string menuName) => menus.Find(x => x.Name == menuName);

        public void CreateSubMenu(MenuContainer menuContainer)
        {
            if (!Exists(menuContainer))
            {
                menus.Add(menuContainer);
            }
        }

        public void RemoveSubMenu(MenuContainer menuContainer)
        {
            if (Exists(menuContainer))
            {
                menus.Remove(menuContainer);
            }
        }

        public void RemoveSubMenu(string menuName)
        {
            if (Exists(menuName))
            {
                menus.Remove(menus.Find(x => x.Name == menuName));
            }
        }

        #endregion
    }
}
