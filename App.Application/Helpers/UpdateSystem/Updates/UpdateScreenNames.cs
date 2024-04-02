using App.Domain.Entities;
using App.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Helpers.UpdateSystem.Updates
{
    public static class UpdateScreenNames
    {
        public static async Task<List<ScreenName>> UpdateScreens(int[] subformsIds)
        {
            var list = new List<ScreenName>();


            var screens = returnSubForms.returnRules().Where(s => subformsIds.Contains(s.subFormCode)).Select(x => new ScreenName
            {
                Id = x.subFormCode,
                ScreenNameAr = x.arabicName,
                ScreenNameEn = x.latinName
            });
            list.AddRange(screens);
            return list;
        }
    }
}
