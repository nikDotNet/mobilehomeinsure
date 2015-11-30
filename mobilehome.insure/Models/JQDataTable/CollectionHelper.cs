using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace mobilehome.insure.Models.JQDataTable
{
    /// <summary>
    /// Contains sorting helpers for In Memory collections.
    /// </summary>
    public static class CollectionHelper
    {
        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IOrderedEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }
    }

    public static class GenericFilterHelper<T>
        where T : class
    {
        private static T entity;

        public static IList<T> GetFilteredRecords(
            Func<List<T>> runTimeMethod,
            int startIndex, int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount,
            string searchString,
            ReadOnlyCollection<string> searchColumnValues,
            List<string> properties)
        {
            
            var types = runTimeMethod();
           
            totalRecordCount = types.Count;


            var isFilterValue = searchColumnValues.Any(e => !string.IsNullOrWhiteSpace(e));
            if ((properties != null && properties.Count > 0) && properties.Count == searchColumnValues.Count - 1 && isFilterValue) // minus -1 means, skipping action column from search list
            {
                var filterValueProp = new Dictionary<string, string>();
                for (int idx = 0; idx < searchColumnValues.Count; idx++)
                {
                    if (!string.IsNullOrWhiteSpace(searchColumnValues[idx]))
                    {
                        filterValueProp.Add(properties[idx], searchColumnValues[idx]);
                    }
                }

                IEnumerable<T> result = null;
                foreach (var item in filterValueProp)
                {
                    //check property in type, whether its exist or not
                    var checkProp = typeof(T).GetProperties().FirstOrDefault(p => p.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
                    if (checkProp == null)
                        continue;

                    result = WhereQuery<T>((result == null ? types : result), item.Key, item.Value);
                    if (result != null && result.Count() > 0)
                        continue;
                }

                if (result != null && result.Count() > 0)
                {
                    types = result.ToList();
                }
            }

            searchRecordCount = types.Count;
            IEnumerable<T> sortedEntityTypes = null;
            foreach (var sortedColumn in sortedColumns)
            {
                if (sortedColumn.Direction == SortingDirection.Ascending)
                {
                    sortedEntityTypes = types.OrderBy(sortedColumn.PropertyName);
                }
                else
                {
                    sortedEntityTypes = types.OrderByDescending(entity => entity.GetType().GetProperty(sortedColumn.PropertyName).GetValue(entity, null));
                }
            }

            return sortedEntityTypes.Skip(startIndex).Take((pageSize > 0 ? pageSize : totalRecordCount)).ToList();
            //return types.Skip(startIndex).Take(pageSize).ToList();
        }

        public static IList<T> GetFilteredRecords(
            List<T>  sourceData,
            int startIndex, int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,    
            int totalRecordCount,
            string searchString,
            ReadOnlyCollection<string> searchColumnValues,
            bool isSearch,
            List<string> properties)
        {

            
            IEnumerable<T> sortedEntityTypes = null;
            foreach (var sortedColumn in sortedColumns)
            {
                if (sortedColumn.Direction == SortingDirection.Ascending)
                {
                    sortedEntityTypes = sourceData.OrderBy(sortedColumn.PropertyName);
                }
                else
                {
                    sortedEntityTypes = sourceData.OrderByDescending(entity => entity.GetType().GetProperty(sortedColumn.PropertyName).GetValue(entity, null));
                }
            }
            if (!isSearch)
            {
                return sortedEntityTypes.ToList();
            }
            else
            {
                return sortedEntityTypes.Skip(startIndex).Take((pageSize > 0 ? pageSize : totalRecordCount))
                    .ToList();
            }
            
        }

        public static IEnumerable<T> WhereQuery<T>(IEnumerable<T> source, string columnName, string propertyValue)
        {
            var filtered = source.Where(m =>
                                            {
                                                var prop = m.GetType().GetProperty(columnName);
                                                if (prop != null && prop.GetValue(m, null) != null && 
                                                prop.GetValue(m, null).ToString().ToUpper().StartsWith(propertyValue.ToUpper()))
                                                {
                                                    return true;
                                                }
                                                else
                                                    return false;
                                            });
            
                        
            return (filtered != null && filtered.Count() > 0) ? filtered : null;
        }
    }

    
    
}