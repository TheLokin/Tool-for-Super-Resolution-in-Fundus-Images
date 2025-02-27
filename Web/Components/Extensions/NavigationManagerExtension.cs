﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace Web.Components.Extensions
{
    public static class NavigationManagerExtension
    {
        public static bool TryGetQueryString<T>(this NavigationManager navigationManager, string key, out T value)
        {
            Uri uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();

                    return true;
                }

                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;

                    return true;
                }

                if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    value = (T)(object)valueAsDecimal;

                    return true;
                }
            }

            value = default;

            return false;
        }

        public static void NavigateToAndReturn(this NavigationManager navigationManager, string uri, bool forceLoad = false)
        {
            if (navigationManager.TryGetQueryString("returnUrl", out string returnUrl))
            {
                navigationManager.NavigateTo(string.Concat(uri, "?returnUrl=", returnUrl), forceLoad);
            }
            else
            {
                navigationManager.NavigateTo(uri, forceLoad);
            }
        }
    }
}